namespace FinCs.Communication.Responses;

public record ResponseExpensesJson
{
    public List<ResponseShortExpenseJson> Expenses { get; set; } = [];
}