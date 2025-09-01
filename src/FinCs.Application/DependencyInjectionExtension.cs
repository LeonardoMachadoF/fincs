using FinCs.Application.AutoMapper;
using FinCs.Application.UseCases.Expenses.Delete;
using FinCs.Application.UseCases.Expenses.GetAll;
using FinCs.Application.UseCases.Expenses.GetById;
using FinCs.Application.UseCases.Expenses.Register;
using FinCs.Application.UseCases.Expenses.Reports.Excel;
using FinCs.Application.UseCases.Expenses.Reports.Pdf;
using FinCs.Application.UseCases.Expenses.Update;
using FinCs.Application.UseCases.Report.Excel;
using FinCs.Application.UseCases.Users.Login;
using FinCs.Application.UseCases.Users.Profile;
using FinCs.Application.UseCases.Users.Register;
using FinCs.Application.UseCases.Users.Update;
using Microsoft.Extensions.DependencyInjection;

namespace FinCs.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddAutoMapper(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetAllExpenseUseCase, GetAllExpenseUseCase>();
        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
        services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
        services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
        services.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
        services.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
    }
}