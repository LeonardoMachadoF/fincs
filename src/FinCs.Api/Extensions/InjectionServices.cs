using FinCs.Application.UseCases.Expenses.Register;

namespace FinCs.Api.Extensions;

public static class InjectionServices
{
    public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        return services;
    }
    
}