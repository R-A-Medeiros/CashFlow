using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpensesRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll(Entities.User user);
    Task<Expense?> GetById(Entities.User user, long id);
    Task Delete(long id);
    Task<List<Expense>> FilterByMonth(Entities.User user, DateOnly date);
    
}
