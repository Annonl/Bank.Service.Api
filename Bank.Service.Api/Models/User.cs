using System.ComponentModel.DataAnnotations;

namespace Bank.Service.Api.Models;

/// <summary>
/// Пользователь.
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия.
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
    public string HashPassword { get; set; }

    public string Salt { get; set; }

    /// <summary>
    /// Email адрес.
    /// </summary>
    [Required]
    public string Email { get; set; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string Phone { get; set; } = String.Empty;

    /// <summary>
    /// Дата рождения.
    /// </summary>
    [Required]
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Возраст.
    /// </summary>
    public int Age => DateTime.Now.DayOfYear < Birthday.DayOfYear
        ? DateTime.Now.Year - Birthday.Year + 1
        : DateTime.Now.Year - Birthday.Year;

    /// <summary>
    /// Короткий код аутентификации.
    /// </summary>
    public string ShortCode { get; set; } = string.Empty;

    /// <summary>
    /// Подтверждён пользователь или нет.
    /// </summary>
    public bool IsActiveUser { get; set; }

    /// <summary>
    /// Список счетов пользователя.
    /// </summary>
    public List<Account> Accounts { get; set; } 
}
