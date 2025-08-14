using CommonTestUtilities.Entities;
using FinCs.Domain.Entities;
using FinCs.Domain.Enums;
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
    public ExpenseIdentityManager Expense_MemberTeam { get; private set; }
    public ExpenseIdentityManager Expense_Admin { get; private set; }

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

    private void StartDatabase(
        FinCsDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, accessTokenGenerator);
        var expenseTeamMember = AddExpenses(dbContext, userTeamMember, 1);
        Expense_MemberTeam = new ExpenseIdentityManager(expenseTeamMember);

        var userAdmin = AddUserAdmin(dbContext, passwordEncripter, accessTokenGenerator);
        var expenseAdmin = AddExpenses(dbContext, userAdmin, 2);
        Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(
        FinCsDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;
        user.Password = passwordEncripter.Encript(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(
        FinCsDbContext dbContext,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;
        user.Password = passwordEncripter.Encript(user.Password);

        dbContext.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expense AddExpenses(FinCsDbContext dbContext, User user, long expenseId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        dbContext.Expenses.Add(expense);

        return expense;
    }
}