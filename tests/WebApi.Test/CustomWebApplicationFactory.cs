using CommonTestUtilities.Entities;
using FinCs.Domain.Entities;
using FinCs.Domain.Security.Cryptography;
using FinCs.Domain.Security.Tokens;
using FinCs.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private string _password;
    private User _user;
    private string _token;

    public string GetEmail()
    {
        return _user.Email;
    }

    public string GetName()
    {
        return _user.Name;
    }

    public string GetPassword()
    {
        return _password;
    }  public string GetToken()
    {
        return _token;
    }

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

                    var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    StartDatabase(dbContext, passwordEncripter);

                    _token = tokenGenerator.Generate(_user);
                }
            );
    }

    private void StartDatabase(FinCsDbContext dbContext, IPasswordEncripter encripter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        _user.Password = encripter.Encript(_user.Password);
        dbContext.Users.Add(_user);
        dbContext.SaveChanges();
    }
}