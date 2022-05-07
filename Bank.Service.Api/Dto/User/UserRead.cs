namespace Bank.Service.Api.Dto.User;

public class UserRead
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
    public string Patronymic { get; set; }

    /// <summary>
    /// Email адрес.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Телефон.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Дата рождения.
    /// </summary>
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Возраст.
    /// </summary>
    public int Age => DateTime.Now.DayOfYear < Birthday.DayOfYear
        ? DateTime.Now.Year - Birthday.Year + 1
        : DateTime.Now.Year - Birthday.Year;
}