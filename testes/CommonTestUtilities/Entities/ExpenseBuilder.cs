using Bogus;
using CashFlow.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class ExpenseBuilder
{
   //public static Expense Build(User user)
   // {
   //     return new Faker<Expense>()
   //         .RuleFor(u => u.Id, _ => 1)
   //         .RuleFor(u => u.Title, faker => faker.Commerce.ProductName())
   //         .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
   //         .RuleFor(r => r.Date, faker => faker.Date.Past())
   //         .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
   //         .RuleFor(r => r.PaymentType, faker => faker.PickRandom<CashFlow.Communication.Enums.PaymentType>())
   //         .RuleFor(r => r.UserId, _ => user.Id)
   //         .RuleFor(r => r.Tags, faker => faker.Make(1, () => new CashFlow.Domain.Entities.Tag
   //         {

   //         }));
   // }
}
