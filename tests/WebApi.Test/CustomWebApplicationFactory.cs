using CommonTestUtilities.Entities;
using FinCs.Domain.Security.Cryptography;
using FinCs.Domain.Security.Tokens;
using FinCs.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public UserIdentityManager User_Team_Member { get; private set; }
    public UserIdentityManager User_Admin { get; private set; }
    public ExpenseIdentityManager Expense { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(
                services =>
                {
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                    services.AddDbContext<FinCsDbContext>(
                        config =>
                        {
                            config.UseInMemoryDatabase("InMemoryDbForTesting");
                            config.UseInternalServiceProvider(provider);
                        }
                    );

                    var scope = services
                        .BuildServiceProvider()
                        .CreateScope();

                    var dbContext = scope
                        .ServiceProvider
                        .GetRequiredService<FinCsDbContext>();
                    var passwordEncripter = scope
                        .ServiceProvider
                        .GetRequiredService<IPasswordEncripter>();
                    var accessTokenGenerator = scope
                        .ServiceProvider
                        .GetRequiredService<IAccessTokenGenerator>();


                    StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);
                }
            );
    }

    private void StartDatabase(FinCsDbContext dbContext, IPasswordEncripter encripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        var password = user.Password;
        user.Password = encripter.Encript(user.Password);
        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);
        User_Team_Member = new UserIdentityManager(user, password, token);

        var expense = ExpenseBuilder.Build(user);
        dbContext.Expenses.Add(expense);
        Expense = new ExpenseIdentityManager(expense);
        dbContext.SaveChanges();
    }
}