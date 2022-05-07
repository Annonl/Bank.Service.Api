using System.ComponentModel.DataAnnotations;

namespace Bank.Service.Api.Dto.User;

/// <summary>
/// Данные пользователя при регистрации.
/// </summary>
public class UserRegistration
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    [Required]
    public string LastName { get; set; }

    /// <summary>
    /// Отчество.
    /// </summary>
    public string Patronymic { get; set; } = string.Empty;

    /// <summary>
    /// Пароль.
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Email адрес.
    /// </summary>
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Дата рождения.
    /// </summary>
    [Required]
    public DateTime Birthday { get; set; }
}