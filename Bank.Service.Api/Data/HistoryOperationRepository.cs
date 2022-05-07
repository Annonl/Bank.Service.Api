using Bank.Service.Api.Dto.HistoryOperation;
using Bank.Service.Api.Enums;
using Bank.Service.Api.Models;
using BankLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bank.Service.Api.Data;

/// <summary>
/// Операции над операциями.
/// </summary>
public class HistoryOperationRepository : IRepository<HistoryOperation>
{
    private readonly DataBaseContext _context;

    public HistoryOperationRepository(DataBaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Сохранение данных в базу данных.
    /// </summary>
    /// <returns>Успешность сохранения данных.</returns>
    public bool SaveChangesAsync()
    {
        return _context.SaveChanges() > 0;
    }

    /// <summary>
    /// Получение списка всех операций по всем счетам.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<IEnumerable<HistoryOperation>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получение всех операций по идентификатору счёта.
    /// </summary>
    /// <param name="id">Идентификатор счёта.</param>
    /// <returns>Список операций.</returns>
    public async Task<IEnumerable<HistoryOperation>> GetAllAsync(Guid id)
    {
        return await _context.Operations
            .Where(item => item.AccountId == id)
            .ToListAsync();
    }

    /// <summary>
    /// Получение операции по идентификатору операции.
    /// </summary>
    /// <param name="id">Идентификатор операции.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<HistoryOperation> GetByIdAsync(Guid id)
    {
        return await _context.Operations
            .FirstAsync(item => item.Id == id);
    }

    /// <summary>
    /// Создание операции.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public async Task CreateAsync(HistoryOperation operation)
    {
        await _context.Operations.AddAsync(operation);
    }

    /// <summary>
    /// Проверка наличия акаунта у пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="accountId">Идентификатор счёта.</param>
    /// <returns>Есть ли данный счёт у пользователя.</returns>
    public bool HasAccount(Guid userId, Guid accountId)
    {
        return _context.Users
            .Select(item => item.Id == userId
                            && item.Accounts
                                .Select(item => item.Id == accountId)
                                .Contains(true))
            .Contains(true);
    }

    /// <summary>
    /// Проверка наличия заданного счёта.
    /// </summary>
    /// <param name="operationNumberCard">Номер карты.</param>
    /// <param name="operationDataEnd">Дата окончания карты.</param>
    /// <param name="operationCvc">Код карты.</param>
    /// <returns>Существует данный счёт или нет.</returns>
    public async Task<bool> HasCard(string operationNumberCard, DateTime operationDataEnd, int operationCvc)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(item => item.NumberCard == operationNumberCard);

        return account != null
               && account.Cvc == operationCvc
               && account.DataEnd.Year == operationDataEnd.Year
               && operationDataEnd.Month == account.DataEnd.Month;
    }

    /// <summary>
    /// Проверка наличия номера карты.
    /// </summary>
    /// <param name="operationNumberCard"></param>
    /// <returns></returns>
    public async Task<bool> HasCard(string operationNumberCard)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(item => item.NumberCard == operationNumberCard);
        return account != null;
    }

    //todo: Понять, почему не работают связи один ко многим.
    /// <summary>
    /// Получение Email адреса по номеру карты.
    /// </summary>
    /// <param name="numberCard">Номер карты.</param>
    /// <returns>Email адрес пользователя.</returns>
    public string GetEmail(string numberCard)
    {
        return _context.Accounts
            .First(item => item.NumberCard == numberCard)
            .User.Email;
    }

    /// <summary>
    /// Перевод средств с одной карты на другую.
    /// </summary>
    /// <param name="operation">Информация о получателе.</param>
    /// <param name="getId">Идентификатор отправившего.</param>
    /// <returns>Результат перевода между картами.</returns>
    public bool SetTransfer(TransferOperation operation, Guid getId)
    {
        var accountFrom = _context.Accounts.First(item => item.NumberCard == operation.NumberCardFrom);
        var accountTo = _context.Accounts.First(item => item.NumberCard == operation.NumberCardTo);

        if (accountFrom == null || accountTo == null || accountFrom.UserId != getId)
        {
            return false;
        }

        if (accountFrom.Amount < operation.Amount)
        {
            _context.Operations.Add(new HistoryOperation()
            {
                AccountId = accountFrom.Id,
                NameOperation = "Перевод " + operation.NumberCardTo,
                Amount = operation.Amount,
                Created = DateTime.Now,
                Operation = TypeOperation.Transfer,
                ResultOperation = ResultOperation.Failure,
                Id = Guid.NewGuid()
            });

            //_context.Accounts.Update(accountFrom);

            SaveChangesAsync();

            return false;
        }

        accountTo.Amount += operation.Amount;
        accountFrom.Amount -= operation.Amount;

        _context.Operations.Add(new HistoryOperation()
        {
            AccountId = accountTo.Id, Amount = operation.Amount, Created = DateTime.Now,
            Operation = TypeOperation.Replenishment, ResultOperation = ResultOperation.Success,
            NameOperation = "Перевод от " + operation.NumberCardFrom, Id = Guid.NewGuid()
        });

        _context.Operations.Add(new HistoryOperation()
        {
            AccountId = accountFrom.Id, NameOperation = "Перевод " + operation.NumberCardTo, Amount = operation.Amount,
            Created = DateTime.Now, Operation = TypeOperation.Transfer, ResultOperation = ResultOperation.Success,
            Id = Guid.NewGuid()
        });


        SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Установка покупки.
    /// </summary>
    /// <param name="operation">Данные о покупке.</param>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Результат покупки.</returns>
    public bool SetBuy(BuyOperation operation, Guid id)
    {
        var account = _context.Accounts.First(item => item.NumberCard == operation.NumberCard);

        if (account == null)
        {
            return false;
        }

        if (account.Amount < operation.Amount)
        {
            _context.Operations.Add(new HistoryOperation()
            {
                AccountId = account.Id, Amount = operation.Amount, Created = DateTime.Now, Id = Guid.NewGuid(),
                NameOperation = operation.NameOperation, Operation = TypeOperation.Purchase,
                ResultOperation = ResultOperation.Failure
            });

            SaveChangesAsync();

            return false;
        }



        _context.Operations.Add(new HistoryOperation()
        {
            AccountId = account.Id,
            Amount = operation.Amount,
            Created = DateTime.Now,
            Id = Guid.NewGuid(),
            NameOperation = operation.NameOperation,
            Operation = TypeOperation.Purchase,
            ResultOperation = ResultOperation.Success
        });

        SaveChangesAsync();

        return true;
    }
}