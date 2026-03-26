using FluentValidation.AspNetCore;
using PAS.API.Extensions;
using PAS.API.Hubs;
using PAS.API.Middleware;
using PAS.Application;
using PAS.Infrastructure;
using PAS.Persistence;
using Persistence.Context;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/pas-api-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.ApplicationInsights("YOUR-APP-INSIGHTS-KEY", TelemetryConverter.Traces)
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .CreateLogger();

try
{
    Log.Information("Starting up the Property Automation System API");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services
    builder.Services.AddApplicationServices();
    builder.Services.AddPersistenceServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApiServices(builder.Configuration);
    builder.Services.AddControllers()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
    builder.Services.AddSignalR();
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<ApplicationDbContext>()
        .AddUrlGroup(new Uri("https://www.ecx.com.et"), "ECX Website");
    builder.Services.AddSwaggerDocumentation();
    builder.Services.AddCorsPolicy(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerDocumentation();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowSpecificOrigin");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();
    app.UseMiddleware<PerformanceMiddleware>();
    app.UseMiddleware<JwtMiddleware>();
    app.MapControllers();
    app.MapHub<NotificationHub>("/notificationHub");
    app.MapHealthChecks("/health");

    // Seed database
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        await dbInitializer.InitializeAsync();
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}