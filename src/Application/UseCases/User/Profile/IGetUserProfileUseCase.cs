using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}
