using System.Net.NetworkInformation;
using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, u) => faker.Internet.Email(u.Name))
            .RuleFor(u => u.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }

}

