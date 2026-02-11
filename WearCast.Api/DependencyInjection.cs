using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Errors;
using System.Reflection;

namespace WearCast.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllers();

            services
                .AddSwaggerServices()
                .AddFluentValidationConfig();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();
            return services;
        }
        private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
            services
               .AddFluentValidationAutoValidation()
               .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
