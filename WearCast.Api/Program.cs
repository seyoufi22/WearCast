using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Http.Connections;
using WearCast.Api;
using WearCast.Api.Common.Services.Notifications;
using WearCast.Api.Features.NotificationManagement.NotificationHub;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
    options.EnablePersistAuthorization();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

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
    DashboardTitle = "WearCast Background Jobs",
    // IsReadOnlyFunc = (DashboardContext context) => true
});
app.UseExceptionHandler();

app.MapControllers();
app.MapCarter();

app.UseWebSockets();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
