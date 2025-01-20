using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;
public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private readonly IExpensesRepository _repository;
    public GenerateExpensesReportPdfUseCase(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expense = await _repository.FilterByMonth(month);
        if(expense.Count == 0)
        {
            return [];
        }

        return [];
    }
}
