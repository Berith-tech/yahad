using back_yahad.Infrastructure.DependencyInjection;
using back_yahad.Modules.Auth.Endpoints;
using back_yahad.Modules.Auth.Extensions;
using back_yahad.Modules.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "appsettings.Local.json",
    optional: true,
    reloadOnChange: true
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthModule(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthenticationModule(builder.Configuration);
builder.Services.AddUsersModule();

builder.Services.AddSwaggerModule();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("FrontendPolicy");
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