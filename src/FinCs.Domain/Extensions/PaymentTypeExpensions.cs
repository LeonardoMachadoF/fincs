using FinCs.Domain.Enums;
using FinCs.Domain.Reports;

namespace FinCs.Domain.Extensions;

public static class PaymentTypeExpensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportGenerationMessages.CASH,
            PaymentType.CreditCard => ResourceReportGenerationMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportGenerationMessages.DEBIT_CARD,
            PaymentType.ElectronicTransfer => ResourceReportGenerationMessages.ELECTRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}