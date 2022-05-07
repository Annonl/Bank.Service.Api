using System.Security.Claims;
using AutoMapper;
using Bank.Service.Api.Auth;
using Bank.Service.Api.Data;
using Bank.Service.Api.Dto.HistoryOperation;
using Bank.Service.Api.Models;
using BankLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Service.Api.Controllers;

/// <summary>
/// Действия над операциями.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OperationsController : ControllerBase
{
    /// <summary>
    /// Методы, доступные для работы с операциями.
    /// </summary>
    private readonly HistoryOperationRepository _repository;

    /// <summary>
    /// Связь моделей Dto.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализация полей.
    /// </summary>
    public OperationsController(IRepository<HistoryOperation> repository, IMapper mapper)
    {
        _repository = (HistoryOperationRepository)repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получение списка операций.
    /// Требуется: JWT токен, полученный в методе auth.
    /// </summary>
    /// <param name="id">Идентификатор счёта.</param>
    /// <returns>Список операций.</returns>
    [HttpGet("history/{id:guid}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<OperationRead>>> GetHistoryOperations(Guid id)
    {
        if (!AuthChecker.IsAuth(User))
        {
            return Forbid();
        }

        if (!_repository.HasAccount(AuthChecker.GetId(User), id))
        {
            return NotFound(id);
        }

        if (id == Guid.Empty)
        {
            return BadRequest("Id is empty.");
        }

        return Ok((await _repository.GetAllAsync(id))
            .Select(item => _mapper.Map<OperationRead>(item)));

    }

    /// <summary>
    /// Post запрос для операции покупки.
    /// </summary>
    /// <param name="operation">Данные о операции.</param>
    /// <returns></returns>
    [HttpPost("buy")]
    public async Task<ActionResult> PostBuyOperation(BuyOperation operation)
    {
        if (!await _repository.HasCard(operation.NumberCard, operation.DataEnd, operation.Cvc))
        {
            return NotFound(operation.NumberCard);
        }

        var result = _repository.SetBuy(operation,AuthChecker.GetId(User));

        return result
            ? Ok()
            : BadRequest();
    }

    /// <summary>
    /// Перевод с карты на карту.
    /// Требуется: JWT токен, полученный в методе auth.
    /// </summary>
    /// <param name="operation">Номер карты отправителя, получателя и сумма перевода.</param>
    /// <returns></returns>
    [HttpPost("transfer")]
    [Authorize]
    public async Task<ActionResult> TransferOperation(TransferOperation operation)
    {
        if (!AuthChecker.IsAuth(User))
        {
            return Forbid();
        }

        if (!await _repository.HasCard(operation.NumberCardTo) || !await _repository.HasCard(operation.NumberCardFrom))
        {
            return NotFound(operation);
        }

        if (operation.Amount <= 0)
        {
            return BadRequest(operation);
        }

        var transferResult = _repository.SetTransfer(operation, AuthChecker.GetId(User));
        return transferResult
            ? Ok()
            : BadRequest();

    }
}
