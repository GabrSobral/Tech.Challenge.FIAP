using System.Net;
using Tech.Challenge.Application;
using Tech.Challenge.Configuration;
using Tech.Challenge.Infra.Database;
using Tech.Challenge.Infra.MailService;
using Tech.Challenge.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddApplication()
    .AddInfraDatabase(builder.Configuration)
    .AddInfraMailService()
    .AddPresentation()
    .ConfigureCors()
    .ConfigureAuthentication();

builder.Services.AddOpenApi();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var remoteIp = context.Connection.RemoteIpAddress;

    // Log only if NOT local
    if (remoteIp != null && !IPAddress.IsLoopback(remoteIp))
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("External Request: {Method} {Path} from {IP}", 
            context.Request.Method, 
            context.Request.Path, 
            remoteIp);
    }

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ConfigureMiddlewares();
app.ConfigureEndPoints();
app.ConfigureMigrations();

app.Run();
