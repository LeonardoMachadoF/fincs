using System.Text;
using FinCs.Api.Extensions;
using FinCs.Api.Filters;
using FinCs.Api.Middlewares;
using FinCs.Application;
using FinCs.Infrastructure;
using FinCs.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(
    options => { options.LowercaseUrls = true; }
);
builder.Services.AddOpenApi();
builder.Services.AddDependencyInjectionServices();

builder.Services.AddMvc(options =>
    options.Filters.Add<ExceptionFilter>()
);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = new TimeSpan(0, 0, 0),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder
                        .Configuration
                        .GetValue<string>("Settings:Jwt:SigningKey")!)
            )
    };
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opts =>
        opts.SwaggerEndpoint(
            "/openapi/v1.json",
            "v1.0.0")
    );
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await MigrateDatabaseAsync();
app.Run();

async Task MigrateDatabaseAsync()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}