using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Bank.Service.Api.Auth;

/// <summary>
/// Опции для генерации JWT токенов для пользователей.
/// </summary>
public class AuthOptions
{
    /// <summary>
    /// Название эмитента.
    /// </summary>
    public const string Issuer = "BankServer";

    /// <summary>
    /// Название получателя JWT токена.
    /// </summary>
    public const string Audience = "BankClient";

    /// <summary>
    /// Генерация симметричного кода безопасности для JWT токенов.
    /// </summary>
    /// <param name="key">Ключ шифрования.</param>
    /// <returns>Код безопасности.</returns>
    public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }
}