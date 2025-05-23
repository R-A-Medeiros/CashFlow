﻿
using CashFlow.Communication.Enums;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using ClosedXML.Excel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;
public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public GenerateExpensesReportExcelUseCase(IExpensesRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();

        var expenses = await _repository.FilterByMonth(loggedUser, month);

        if (expenses.Count == 0)
        {
            return [];
        }

        using var workBook = new XLWorkbook();

        workBook.Author = "Rinaldo Alves";
        workBook.Style.Font.FontSize = 12;
        workBook.Style.Font.FontName = "Roboto";

        var worksheet = workBook.Worksheets.Add(month.ToString("Y"));

        InsertHeader(worksheet);

        var raw = 2;
        foreach (var expense in expenses)
        {
            worksheet.Cell($"A{raw}").Value = expense.Title;
            worksheet.Cell($"B{raw}").Value = expense.Date;
            worksheet.Cell($"C{raw}").Value = expense.PaymentType.ToString();//ConvertPaymentType(expense.PaymentType);

            worksheet.Cell($"D{raw}").Value = expense.Amount;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";

            worksheet.Cell($"E{raw}").Value = expense.Description;

            raw++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();

        workBook.SaveAs(file);

        return file.ToArray();
    }
    private void InsertHeader(IXLWorksheet worksheet)
    {

        worksheet.Cell("A1").Value = "Titulo";
        worksheet.Cell("B1").Value = "Data";
        worksheet.Cell("C1").Value = "Forma de Pagamento";
        worksheet.Cell("D1").Value = "Valor";
        worksheet.Cell("E1").Value = "Descrição";

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }
}

