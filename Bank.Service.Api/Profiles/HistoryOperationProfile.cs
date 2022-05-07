using AutoMapper;
using Bank.Service.Api.Dto.HistoryOperation;
using Bank.Service.Api.Models;

namespace Bank.Service.Api.Profiles;

/// <summary>
/// Профилировщик класса HistoryOperation.
/// </summary>
public class HistoryOperationProfile : Profile
{
    /// <summary>
    /// Созание связи между моделями Dto класса HistoryOperation.
    /// </summary>
    public HistoryOperationProfile()
    {
        CreateMap<HistoryOperation, OperationRead>();
    }
}