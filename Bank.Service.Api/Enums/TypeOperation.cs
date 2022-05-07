namespace Bank.Service.Api.Enums;

/// <summary>
/// Тип операции.
/// </summary>
public enum TypeOperation
{
    /// <summary>
    /// Пополнение.
    /// </summary>
    Replenishment,

    /// <summary>
    /// Снятие.
    /// </summary>
    Withdrawal,

    /// <summary>
    /// Перевод.
    /// </summary>
    Transfer,

    /// <summary>
    /// Покупка.
    /// </summary>
    Purchase
}