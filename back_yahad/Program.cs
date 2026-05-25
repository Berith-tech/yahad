using back_yahad.Infrastructure.DependencyInjection;
using back_yahad.Modules.Users;
using back_yahad.Modules.Auth.Extensions;
using back_yahad.Modules.Auth.Endpoints;
using back_yahad.Infrastructure.DependencyInjection;
using back_yahad.Modules.Auth.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthModule();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthenticationModule(builder.Configuration);
builder.Services.AddUsersModule();

builder.Services.AddSwaggerModule();
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () =>
    Results.Ok(new
    {
        status = "ok",
        servico = "yahad-api"
    })
);

app.MapUsersModule();
app.MapAuthEndpoints();

app.Run();