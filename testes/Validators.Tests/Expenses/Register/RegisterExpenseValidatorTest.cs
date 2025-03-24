using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTest
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeTrue();


        //ShouldSatisfyAllConditions(
        //    name => name.ShouldNotBeNullOrWhiteSpace(),
        //    ......);
    }

    [Theory]
    [InlineData("")]
    [InlineData("       ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = title;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe("The title is required.");

    }

    [Fact]
    public void Error_Date_Future()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe("Expenses cannot be for the future.");

    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)700;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe("Payment Type is not valid.");

    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-9)]
    [InlineData(-20)]
    public void Error_Amount_Invalid(decimal amout)
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Amount = amout;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe("Then Amount must be greater than zero.");

    }
}

