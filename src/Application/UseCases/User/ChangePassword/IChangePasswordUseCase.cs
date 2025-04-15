using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
