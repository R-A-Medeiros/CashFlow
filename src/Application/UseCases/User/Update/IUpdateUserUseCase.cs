using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
