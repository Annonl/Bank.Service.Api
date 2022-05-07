using System.ComponentModel.DataAnnotations;
using Bank.Service.Api.Enums;

namespace Bank.Service.Api.Dto.Account;

/// <summary>
/// Данные, предназначенные для получения при Get запросах.
/// </summary>
public class AccountRead
{
    /// <summary>
    /// Идентификатор счёта.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Название счёта.
    /// </summary>
    public string NameAccount { get; set; }

    /// <summary>
    /// Номер счёта.
    /// </summary>
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
    public int Cvc { get; set; }

    /// <summary>
    /// Дата регистрации счёта.
    /// </summary>
    public DateTime DataRegistration { get; set; }

    /// <summary>
    /// Количество денег на счёте.
    /// </summary>
    public decimal Amount { get; set; } = 0;

    /// <summary>
    /// Валюта счёта (810 - Рубли, 840 - Доллары, 978 - Евро).
    /// </summary>
    public Currency CurrencyType { get; set; } = Currency.Ruble;
}