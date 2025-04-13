using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.Delete;
public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesRepository _expensesRepository;
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteExpenseUseCase(
        IExpensesRepository repository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser,
        IExpensesRepository expensesRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _expensesRepository = expensesRepository;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var expense = await _expensesRepository.GetById(loggedUser, id);

        if (expense is null)
        {
            throw new DirectoryNotFoundException("Expense Not Found");
        }


        await _repository.Delete(id);

        await _unitOfWork.Commit();
    }
}
