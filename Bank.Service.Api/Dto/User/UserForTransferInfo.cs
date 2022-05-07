namespace Bank.Service.Api.Dto.User;

/// <summary>
/// Данные для получения информации о пользователе по карте.
/// </summary>
public class UserForTransferInfo
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Отчество.
    /// </summary>
    public string Patronymic { get; set; } = string.Empty;

}