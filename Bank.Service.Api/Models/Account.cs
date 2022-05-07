using System.ComponentModel.DataAnnotations;
using Bank.Service.Api.Enums;

namespace Bank.Service.Api.Models;

/// <summary>
/// Информация о счёте.
/// </summary>
public class Account
{
    /// <summary>
    /// Идентификатор счёта.
    /// </summary>
    [Required]
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Название счёта.
    /// </summary>
    [Required]
    public string NameAccount { get; set; }

    /// <summary>
    /// Номер счёта.
    /// </summary>
    [Required]
    public string NumberAccount { get; set; }

    /// <summary>
    /// Номер карты.
    /// </summary>
    public string NumberCard { get; set; }

    /// <summary>
    /// Окончание действия счёта.
    /// </summary>
    public DateTime DataEnd { get; set; }

    /// <summary>
    /// Код проверки подлинности карты.
    /// </summary>
    [Required]
    public int Cvc { get; set; }

    /// <summary>
    /// Дата регистрации счёта.
    /// </summary>
    public DateTime DataRegistration { get; set; }

    /// <summary>
    /// Количество денег на счёте.
    /// </summary>
    [Required]
    public decimal Amount { get; set; } = 0;

    /// <summary>
    /// Валюта счёта.
    /// </summary>
    [Required]
    public Currency CurrencyType { get; set; } = Currency.Ruble;

    /// <summary>
    /// Пользователь, у которого имеется данный счёт.
    /// </summary>
    [Required]
    public User User { get; set; }

    /// <summary>
    /// Список операций.
    /// </summary>
    public List<HistoryOperation> HistoryOperations { get; set; } = new List<HistoryOperation>();

}
