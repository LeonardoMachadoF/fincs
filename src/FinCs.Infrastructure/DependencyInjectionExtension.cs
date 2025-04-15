using FinCs.Domain.Repositories;
using FinCs.Domain.Repositories.Expenses;
using FinCs.Domain.Repositories.User;
using FinCs.Domain.Security.Cryptography;
using FinCs.Domain.Security.Tokens;
using FinCs.Infrastructure.DataAccess;
using FinCs.Infrastructure.DataAccess.Repositories;
using FinCs.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinCs.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddDbContext(services, configuration);
        AddToken(services, configuration);

        services.AddScoped<IPasswordEncripter, Security.BCrypt>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTime = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeInMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(opt => new JwtTokenGenerator(expirationTime, signingKey!));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
        services.AddScoped<IExpensesUpdateOnlyRepository, ExpensesRepository>();

        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var serverVersion = new MySqlServerVersion(new Version(9, 2, 0));
        services.AddDbContext<FinCsDbContext>(config =>
            config.UseMySql(connectionString, serverVersion)
        );
    }
}