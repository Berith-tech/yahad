using back_yahad.Infrastructure.DependencyInjection;
using back_yahad.Modules.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddUsersModule();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new { status = "ok", servico = "yahad-api" }));
app.MapUsersModule();

app.Run();
