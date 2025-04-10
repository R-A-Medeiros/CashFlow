using CashFlow.Application.UseCases.User;
using CashFlow.Exception.ExceptionBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUserCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Token.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUserCase();

        var act = async () => await useCase.Execute(request);

        var result =  await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Count.ShouldBe(1);

        result.GetErrors().ShouldContain("Name cannot be empty.");
    }

    //[Fact]
    //public async Task Error_Email_Already_Exist()
    //{
    //    var request = RequestRegisterUserJsonBuilder.Build();
    //    //request.Name = string.Empty;

    //    var useCase = CreateUserCase();

    //    var act = async () => await useCase.Execute(request);

    //    var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

    //    result.GetErrors().Count.ShouldBe(1);

    //    result.GetErrors().ShouldContain("Name cannot be empty.");
    //}

    private RegisterUserUseCase CreateUserCase()
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var tokenGenerator = JwtTokenGeneratorBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder().Build();

        return new RegisterUserUseCase(mapper, passwordEncripter, readRepository, writeRepository, unitOfWork, tokenGenerator);
    }
}
