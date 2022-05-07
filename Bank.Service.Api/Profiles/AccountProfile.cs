using AutoMapper;
using Bank.Service.Api.Dto.Account;
using Bank.Service.Api.Models;

namespace Bank.Service.Api.Profiles;

/// <summary>
/// Профилировщик класса Account.
/// </summary>
public class AccountProfile : Profile
{
    /// <summary>
    /// Создание связи между моделями Dto класса Account.
    /// </summary>
    public AccountProfile()
    {
        CreateMap<Account, AccountRead>();
    }
}
