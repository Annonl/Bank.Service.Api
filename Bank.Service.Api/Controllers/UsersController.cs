using AutoMapper;
using Bank.Service.Api.Auth;
using Bank.Service.Api.Data;
using Bank.Service.Api.Dto.User;
using Bank.Service.Api.Models;
using BankLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace Bank.Service.Api.Controllers;

/// <summary>
/// Контроллер для пользователей.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    /// <summary>
    /// Операции над пользователем.
    /// </summary>
    private readonly UserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Инициализация полей.
    /// </summary>
    public UsersController(IRepository<User> repository, IMapper mapper, IConfiguration configuration)
    {
        _repository = (UserRepository)repository;
        _mapper = mapper;
        _configuration = configuration;
    }

    /// <summary>
    /// Получение информации о пользователе.
    /// Требуется: JWT токен, полученный в методе auth.
    /// </summary>
    /// <returns>Информация о найденном пользователе.</returns>
    [Authorize]
    [HttpGet("info")]
    public async Task<ActionResult<UserRead>> GetUser()
    {
        if (!AuthChecker.IsAuth(User))
        {
            return Forbid();
        }

        var id = AuthChecker.GetId(User);

        try
        {
            var user = await _repository.GetByIdAsync(id);
            return await Task.FromResult(Ok(_mapper.Map<UserRead>(user)));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return Problem();
        }
    }

    /// <summary>
    /// Получение информации о пользоватле по номеру карты.
    /// Требуется: JWT токен, полученный в методе auth.
    /// </summary>
    /// <param name="numberCard">Номер карты.</param>
    /// <returns>Информация о пользователе.</returns>
    [Authorize]
    [HttpGet("info/{numberCard}")]
    public async Task<ActionResult<UserForTransferInfo>> GetUser(string numberCard)
    {
        if (!AuthChecker.IsAuth(User))
        {
            return Forbid();
        }

        try
        {
            var user = await _repository.GetByNumberCard(numberCard);
            return await Task.FromResult(Ok(_mapper.Map<UserForTransferInfo>(user)));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return Problem();
        }
    }

    /// <summary>
    /// Регистрация пользователя.
    /// </summary>
    /// <param name="userCreate">Данные о пользователе.</param>
    /// <returns>Данные о созданном пользователе.</returns>
    [HttpPost("registration")]
    public async Task<ActionResult<UserRead>> Registration(UserRegistration userCreate)
    {
        if (await _repository.IsExistEmail(userCreate.Email))
        {
            return BadRequest("Email exist");
        }

        var user = _mapper.Map<User>(userCreate);
        user.IsActiveUser = false;

        await _repository.SetUserPasswordAsync(user, userCreate.Password);
        await _repository.CreateAsync(user);

        _repository.SaveChangesAsync();
        var readUser = _mapper.Map<UserRead>(user);

        await _repository.AddNewDefaultAccountAsync(user);

        Email.Send(user.Email,
            $"<p>Для подтверждения учётной записи перейдите по этой <a href=\"{_configuration["BaseUrl"]}api/Users/confirm/{user.Id}\">ссылке.</a></p>",
            "Подтверждение регистрации в lunapay.");

        return CreatedAtAction(nameof(GetUser), new { id = readUser.Id }, readUser);
    }

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="user">Информация о входе пользователя.</param>
    /// <returns>JWT токен, требующийся для входа по короткому коду пользователя.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLogin user)
    {
        if (!await _repository.IsExistUser(user))
        {
            return BadRequest("Wrong email or password.");
        }

        if (!_repository.IsActivateUser(user.Email))
        {
            return BadRequest("Not confirm email.");
        }

        Email.Send(user.Email, "Кто то зашёл в ваш аккаунт. Если это были не вы, то это плохо.", "Вход в аккаунт lunapay.");
        
        return Ok(new JsonResult(_repository.CreateLoginToken(user, _configuration))); //Ok("{ \"token\":" + _repository.CreateLoginToken(user, _configuration).ToJson() + " }");
    }

    /// <summary>
    /// Аутентификация пользователя по короткому коду.
    /// Требует авторизации: JWT токен, полученный при методе login.
    /// </summary>
    /// <returns>JWT токен, требующийся для совершения остальных операций.</returns>
    [HttpPost("auth")]
    [Authorize]
    public async Task<ActionResult<string>> LogAuth(int shortCode)
    {
        var idUser = AuthChecker.GetId(User);

        return Ok(new JsonResult(_repository.CreateAuthToken(idUser, _configuration)));
    }

    /// <summary>
    /// Установка короткого кода для пользователя.
    /// Требуется: JWT токен, полученный при методе login.
    /// </summary>
    /// <param name="shortCode">Короткий код.</param>
    /// <returns></returns>
    [HttpPost("setshortcode")]
    [Authorize]
    public async Task<ActionResult> SetShortCode(string shortCode)
    {
        var userId = AuthChecker.GetId(User);

        _repository.SetShortCode(userId, shortCode);
        return Ok();
    }

    /// <summary>
    /// Подтверждение почты.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns></returns>
    [HttpGet("confirm/{id:guid}")]
    public async Task<ActionResult<string>> ConfirmUser(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest();
        }

        _repository.SetActivateUser(id);

        _repository.SaveChangesAsync();

        return Ok("Почта подтверждена.");
    }
}
