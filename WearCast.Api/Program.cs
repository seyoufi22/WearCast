using Hangfire;
using HangfireBasicAuthenticationFilter;
using WearCast.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
    options.EnablePersistAuthorization();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

// 3. إعدادات لوحة تحكم Hangfire
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

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();