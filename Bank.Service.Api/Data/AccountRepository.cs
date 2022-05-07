using Bank.Service.Api.Models;
using BankLibrary;
using Microsoft.EntityFrameworkCore;

namespace Bank.Service.Api.Data;

public class AccountRepository : IRepository<Account>
{
    private readonly DataBaseContext _context;

    public AccountRepository(DataBaseContext context)
    {
        _context = context;
    }

    public bool SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Account>> GetAllAsync(Guid id)
    {
        return await _context
            .Accounts
            .Where(item => item.UserId == id)
            .ToListAsync();
    }

    public async Task<Account> GetByIdAsync(Guid id)
    {
        return await _context.Accounts.FirstAsync(item => item.Id == id);
    }

    public async Task CreateAsync(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Account object must be not null.");
        }

        SetProperties(account);

        _context.Accounts.Add(account);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    private void SetProperties(Account account)
    {
        account.Id = Guid.NewGuid();
        account.Cvc = new Random((int)DateTime.UtcNow.Ticks).Next(100, 1000);
        account.DataEnd = DateTime.UtcNow.AddYears(100);
        account.DataRegistration = DateTime.UtcNow.ToLocalTime();
        account.NumberAccount = "40702" + (int)account.CurrencyType + "0" + "0000" + _context.Accounts.Count().ToString().PadLeft(7, '0');
        account.NameAccount = $"Счёт {account.NumberAccount}";
        account.NumberCard = "5536913" + _context.Accounts.Count().ToString().PadLeft(7, '0');
    }

    public decimal GetAllSum(Guid id)
    {
        return _context.Accounts
            .Where(x => x.UserId == id)
            .Sum(item => item.Amount);
    }
}