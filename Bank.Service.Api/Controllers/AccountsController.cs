#nullable disable
using AutoMapper;
using Bank.Service.Api.Auth;
using Bank.Service.Api.Data;
using Bank.Service.Api.Dto.Account;
using Bank.Service.Api.Models;
using BankLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Service.Api.Controllers;

/// <summary>
/// Операции над платёжным счётом пользователя.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    /// <summary>
    /// Операции, доступные для счёта пользователя.
    /// </summary>
    private readonly AccountRepository _repository;

    /// <summary>
    /// Перевод классов из одного состояния в другое. Поддержка Dto.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализация полей.
    /// </summary>
    public AccountsController(IRepository<Account> repository, IMapper mapper)
    {
        _repository = (AccountRepository) repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получение информации о первом счете пользователя.
    /// Требуется: JWT токен, полученный при методе auth.
    /// </summary>
    /// <returns>Список всех счетов пользователя.</returns>
    [HttpGet("info")]
    [Authorize]
    public async Task<ActionResult<AccountRead>> GetAccounts()
    {
        if (!AuthChecker.IsAuth(User))
        {
            return Forbid();
        }

        return Ok((await _repository.GetAllAsync(AuthChecker.GetId(User)))
            .Select(item => _mapper.Map<AccountRead>(item)).First());
    }

    /// <summary>
    /// Получение общей суммы со всех счетов клиентов.
    /// Требуется: JWT токен, полученный при методе auth.
    /// </summary>
    /// <returns>Общая сумма на всех счетах.</returns>
    [HttpGet("allSum")]
    [Authorize]
    public async Task<ActionResult<decimal>> GetSumFromAllAccounts()
    {
        if (!AuthChecker.IsAuth(User))
        {
            return Forbid();
        }

        return Ok(new JsonResult(_repository.GetAllSum(AuthChecker.GetId(User))));
    }
}