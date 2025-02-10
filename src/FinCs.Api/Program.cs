using FinCs.Api.Extensions;
using FinCs.Api.Filters;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
builder.Services.AddOpenApi();
builder.Services.AddDependencyInjectionServices();

builder.Services.AddMvc(options =>
    options.Filters.Add<ExceptionFilter>()
);


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();