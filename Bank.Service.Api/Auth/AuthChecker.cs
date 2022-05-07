using System.Security.Claims;

namespace Bank.Service.Api.Auth;

/// <summary>
/// Проверки авторизации.
/// </summary>
public static class AuthChecker
{
    /// <summary>
    /// Получение идентификатора пользователя по текущему jwt токену.
    /// </summary>
    /// <returns>Идентификатор пользователя.</returns>
    public static Guid GetId(ClaimsPrincipal userPrincipal)
    {
        if (userPrincipal == null)
        {
            throw new ArgumentNullException(nameof(userPrincipal), "Claims must be not null.");
        }

        foreach (var userClaim in userPrincipal.Claims)
        {
            if (userClaim.Type != ClaimTypes.SerialNumber) continue;
            return Guid.Parse(userClaim.Value);
        }

        return Guid.Empty;
    }

    /// <summary>
    /// Проверка авторизованности пользователя в систему.
    /// </summary>
    /// <returns>Вошёл ли пользователь или нет.</returns>
    public static bool IsAuth(ClaimsPrincipal userClaimsPrincipal)
    {
        if (userClaimsPrincipal == null)
        {
            throw new ArgumentNullException(nameof(userClaimsPrincipal), "Claims must be not null.");
        }

        return (from claim in userClaimsPrincipal.Claims
            where claim.Type == "IsAuthentication"
            select bool.Parse(claim.Value)).FirstOrDefault();
    }
}