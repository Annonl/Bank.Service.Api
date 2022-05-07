namespace Bank.Service.Api.Enums;

/// <summary>
/// Результат выполнения операции.
/// </summary>
public enum ResultOperation
{
    /// <summary>
    /// Успешно.
    /// </summary>
    Success,

    /// <summary>
    /// Неудачно.
    /// </summary>
    Failure,

    /// <summary>
    /// Ожидание.
    /// </summary>
    Expectation
}