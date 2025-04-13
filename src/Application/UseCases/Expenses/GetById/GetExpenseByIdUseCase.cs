using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;
public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IMapper _mapper;
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public GetExpenseByIdUseCase(
        IExpensesRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetById(loggedUser, id);

        if (result is null)
        {
            throw new NotFoundException("Expense not Found");
        }

        return _mapper.Map<ResponseExpenseJson>(result);
    }

}
