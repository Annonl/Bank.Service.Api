using System.ComponentModel.DataAnnotations;

namespace Bank.Service.Api.Dto.User;

/// <summary>
/// Предназначен для данных при авторизации пользователя.
/// </summary>
public class UserLogin
{
    /// <summary>
    /// Email адрес.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}