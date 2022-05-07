using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Bank.Service.Api.Auth;
using Bank.Service.Api.Dto.User;
using Bank.Service.Api.Enums;
using Bank.Service.Api.Models;
using BankLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Bank.Service.Api.Data;

/// <summary>
/// Методы для работы с пользователями.
/// </summary>
public class UserRepository : IRepository<User>
{
    /// <summary>
    /// Связь с базой данных.
    /// </summary>
    private readonly DataBaseContext _context;

    public UserRepository(DataBaseContext context)
    {
        _context = context;
    }

    public bool SaveChangesAsync()
    {
        return _context.SaveChangesAsync().Result >= 0;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        return user ?? throw new InvalidOperationException("User was not found.");
    }

    public async Task CreateAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "Account must not be null.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Проверка существования пользователя.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<bool> IsExistUser(UserLogin user)
    {
        if (!_context.Users.Any(item => item.Email == user.Email))
        {
            return false;
        }

        var findUser = await _context.Users.FirstAsync(item => item.Email == user.Email);

        return VerifyPasswordHash(findUser, user.Password);
    }

    /// <summary>
    /// Хеширование пароля для пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="password">Пароль.</param>
    /// <returns></returns>
    public async Task SetUserPasswordAsync(User user, string password)
    {
        using var hmac = new HMACSHA512();

        user.Salt = Convert.ToBase64String(hmac.Key);

        user.HashPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    /// <summary>
    /// Проверка правильности введённого пароля для пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="password">Пароль.</param>
    /// <returns>Соответсвие введённого пароля с паролем пользователя.</returns>
    private static bool VerifyPasswordHash(User user, string password)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(user.Salt));

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(Convert.FromBase64String(user.HashPassword));
    }

    /// <summary>
    /// Создание JWT токена для пользователя при единичных входах.
    /// </summary>
    /// <param name="user">Информация о пользователе.</param>
    /// <param name="configuration">Конфигурация</param>
    /// <returns></returns>
    public string CreateLoginToken(UserLogin user, IConfiguration configuration)
    {
        var findUser = _context.Users.FirstAsync(item => item.Email == user.Email).Result;

        return CreateToken(bool.FalseString, DateTime.UtcNow.AddDays(30), findUser, configuration);
    }

    /// <summary>
    /// Создание JWT токена для пользователя при введении короткого кода.
    /// </summary>
    /// <param name="idUser">Идентификатор пользователя.</param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public string CreateAuthToken(Guid idUser, IConfiguration configuration)
    {
        var findUser = GetByIdAsync(idUser).Result;

        return CreateToken(bool.TrueString, DateTime.UtcNow.AddMinutes(30), findUser, configuration);
    }

    /// <summary>
    /// Создание JWT токена.
    /// </summary>
    /// <param name="isAuth">Авторизация произошла по короткому коду или нет.</param>
    /// <param name="toTime">Время действия токена.</param>
    /// <param name="findUser">Информация о пользователе.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>JWT токен.</returns>
    private string CreateToken(string isAuth, DateTime toTime, User findUser, IConfiguration configuration)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.SerialNumber, findUser.Id.ToString()),
            new(ClaimTypes.Email, findUser.Email),
            new(ClaimTypes.Name, findUser.FirstName),
            new(ClaimTypes.Surname, findUser.LastName),
            new("IsAuthentication",isAuth)
        };

        var cred = new SigningCredentials(
            AuthOptions.GetSymmetricSecurityKey(configuration.GetSection("AppSettings:JWTToken").Value),
            SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            notBefore: DateTime.UtcNow,
            claims: claims,
            expires: toTime,
            signingCredentials: cred
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Добавление нового аккаунта для нового пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <returns>Создание счёта пользователя.</returns>
    public async Task AddNewDefaultAccountAsync(User user)
    {
        var account = new Account()
        {
            Amount = 50000,
            CurrencyType = Currency.Ruble,
            User = user,
            UserId = user.Id,
        };
        account.NameAccount = $"Счёт {account.NumberAccount}";

        await new AccountRepository(_context).CreateAsync(account);
    }

    /// <summary>
    /// Установка нового короткого кода для пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="shortCode">Короткий код.</param>
    public void SetShortCode(Guid userId, string shortCode)
    {
        var user = GetByIdAsync(userId).Result;

        user.ShortCode = shortCode;

        _context.Users.Update(user);

        SaveChangesAsync();
    }

    /// <summary>
    /// Подтверждение электронной почты пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    public void SetActivateUser(Guid id)
    {
        var user = GetByIdAsync(id).Result;

        user.IsActiveUser = true;

        _context.Users.Update(user);
    }

    /// <summary>
    /// Проверка, активирована почта у пользователя или нет.
    /// </summary>
    /// <param name="userEmail">Электронная почта пользователя.</param>
    /// <returns>Активирована почта или нет.</returns>
    public bool IsActivateUser(string userEmail)
    {
        return _context.Users
            .First(item => item.Email == userEmail)
            .IsActiveUser;

    }

    /// <summary>
    /// Получение пользователя по номеру карты.
    /// </summary>
    /// <param name="numberCard">Номер карты.</param>
    /// <returns>Информация о пользователе.</returns>
    /// <exception cref="InvalidOperationException">При отсутствии пользователя.</exception>
    public async Task<User> GetByNumberCard(string numberCard)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(item => item.NumberCard == numberCard);

        if (account == null)
        {
            throw new InvalidOperationException("User was not found.");
        }

        return await _context.Users.FindAsync(account.UserId) ?? throw new InvalidOperationException("User was not found."); ;
    }

    public async Task<bool> IsExistEmail(string userEmail)
    {
        return await _context.Users.AnyAsync(item => item.Email == userEmail);
    }
}
