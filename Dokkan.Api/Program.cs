using Dokkan.Api;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDependencies(builder.Configuration);

builder.Host.UseSerilog((context, configurations) =>
{
    configurations.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
        {
        User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
        Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }

    ],
    //IsReadOnlyFunc = (DashboardContext context) => true
});

app.Run();
