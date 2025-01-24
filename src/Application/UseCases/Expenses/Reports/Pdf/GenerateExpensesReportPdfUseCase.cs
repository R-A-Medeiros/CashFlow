using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;
public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;

    private readonly IExpensesRepository _repository;
    public GenerateExpensesReportPdfUseCase(IExpensesRepository repository)
    {
        _repository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);
        if (expenses.Count == 0)
        {
            return [];
        }
        var document = CreateDocument(month);
        var page = Createpage(document);

        CreatedheaderWithProfilePhotoAndName(page);

        var totalExpenses = expenses.Sum(e => e.Amount);

        CreateTotalSpentSection(page, month, totalExpenses);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            AddExpenseTitle(row.Cells[0], expense.Title);

            AddHeaderForAmount(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.ToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            if(string.IsNullOrWhiteSpace(expense.Description) == false)
            {
                var description = table.AddRow();
                description.Height = HEIGHT_ROW_EXPENSE_TABLE;

                description.Cells[0].AddParagraph(expense.Description);
                description.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
                description.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                description.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                description.Cells[0].MergeRight = 2;
                description.Cells[0].Format.LeftIndent = 20;

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }


        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"Despesa de {month:Y}"; //interpolação simplificada, só funciona em interpolações
        document.Info.Author = "Rinaldo Alves";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.ROBOTO_REGULAR;

        return document;
    }

    private Section Createpage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 40;

        return section;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }

    private void CreatedheaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();
        row.Cells[0].AddImage(""); // caminho da imagem

        row.Cells[1].AddParagraph("Hey, Rinaldo Alves");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.ROBOTO_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;

    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format("Total spent in {0}", month.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 15 });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{totalExpenses} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;
        return table;
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph("Amout");
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"{amount} {CURRENCY_SYMBOL}");
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }
}
