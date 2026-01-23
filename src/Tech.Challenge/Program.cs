using System.Net;
using NewRelic.LogEnrichers.Serilog;
using Serilog;
using Serilog.Events;
using Tech.Challenge.Application;
using Tech.Challenge.Configuration;
using Tech.Challenge.Infra.Database;
using Tech.Challenge.Infra.MailService;
using Tech.Challenge.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Logo no início do Builder
var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithNewRelicLogsInContext() // <--- O PULO DO GATO: Vincula Log ao Trace
    .WriteTo.Console(new NewRelic.LogEnrichers.Serilog.NewRelicFormatter());

Log.Logger = loggerConfig.CreateLogger();
builder.Host.UseSerilog();

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddApplication()
    .AddInfraDatabase(builder.Configuration)
    .AddInfraMailService()
    .AddPresentation()
    .ConfigureCors()
    .ConfigureAuthentication()
    .AddHealthChecks();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapHealthChecks("/health");

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

app.UseSerilogRequestLogging();

app.ConfigureMiddlewares();
app.ConfigureEndPoints();
app.ConfigureMigrations();

app.Run();
