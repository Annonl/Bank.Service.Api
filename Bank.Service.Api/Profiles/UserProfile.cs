using AutoMapper;
using Bank.Service.Api.Dto.User;
using Bank.Service.Api.Models;

namespace Bank.Service.Api.Profiles;

/// <summary>
/// Профилировщик класса User.
/// </summary>
public class UserProfile : Profile
{
    /// <summary>
    /// Создание связи между моделями Dto класса User.
    /// </summary>
    public UserProfile()
    {
        CreateMap<User, UserRead>();
        CreateMap<UserRegistration, User>();
        CreateMap<User, UserForTransferInfo>();
    }
}

