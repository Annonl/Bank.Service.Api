using Bank.Service.Api.Enums;

namespace Bank.Service.Api.Models;

/// <summary>
/// Операция.
/// </summary>
public class HistoryOperation
{
    /// <summary>
    /// Идентификатор операции.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор счёта.
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Дата операции.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Количество денег.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Название операции.
    /// </summary>
    public string NameOperation { get; set; }

    /// <summary>
    /// Тип операции.
    /// </summary>
    public TypeOperation Operation { get; set; }

    /// <summary>
    /// Результат операции.
    /// </summary>
    public ResultOperation ResultOperation { get; set; }

    /// <summary>
    /// Счёт, в котором проводилась операция.
    /// </summary>
    public Account Account { get; set; }
}
