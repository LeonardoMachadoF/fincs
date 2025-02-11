using Bogus;
using FinCs.Communication.Enums;
using FinCs.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterExpenseJsonBuilder
{
    public static RequestRegisterExpenseJson BuildRequestRegisterExpenseJson()
    {
        return new Faker<RequestRegisterExpenseJson>()
            .RuleFor(r => r.Title, f => f.Commerce.Product())
            .RuleFor(r => r.Description, f => f.Commerce.ProductDescription())
            .RuleFor(r => r.Date, f => f.Date.Past())
            .RuleFor(r => r.PaymentType, f => f.PickRandom<PaymentType>())
            .RuleFor(r => r.Amount, f => f.Random.Decimal(1, 100));
    }

    public static RequestRegisterExpenseJson BuildInvalidRequestRegisterExpenseJson(string invalidProperty)
    {
        return new Faker<RequestRegisterExpenseJson>()
            .RuleFor(r => r.Title, f => invalidProperty == "Title" ? "" : f.Commerce.Product())
            .RuleFor(r => r.Description, f => f.Commerce.ProductDescription())
            .RuleFor(r => r.Date, f => invalidProperty == "Date" ? f.Date.Future() : f.Date.Past())
            .RuleFor(r => r.PaymentType,
                f => invalidProperty == "PaymentType" ? (PaymentType)99 : f.PickRandom<PaymentType>())
            .RuleFor(r => r.Amount,
                f => invalidProperty == "Amount" ? -10 : f.Random.Decimal(1, 100));
    }
}