using System.Reflection;
using System.Text.Json.Serialization;
using DepMap.Core.Abstractions;
using DepMap.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace DepMap.Extensions;

public static class DepMapExtensions
{
    public static IServiceCollection AddDepMap(this IServiceCollection services)
    {
        services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        services.AddTransient<IDependenciesProvider, DependenciesProvider>();
        services.AddTransient<IServicesProvider, ServicesProvider>();
        services.AddSingleton<IMiddlewareProvider, MiddlewareProvider>();
        services.AddTransient<IControllersProvider, ControllersProvider>();
        services.AddTransient<IReflectionTweaks, HostReflectionTweaks>();
        services.AddTransient<IReflectionTweaks, HostReflectionTweaks>();
        services.AddTransient<IModelsBuilderService, ModelsBuilderService>();
        return services;
    }

    public static WebApplication UseDepMap(this WebApplication app)
    {
        app.UseStaticFiles();
        app.MapControllerRoute(
                name: "depmapui",
                pattern: "depmapui/{action=Index}",
                defaults: new { controller = "DepMap", action = "Index" });
        app.MapControllers();
        
        using IServiceScope scope = app.Services.CreateScope();
        var mp = scope.ServiceProvider.GetRequiredService<IMiddlewareProvider>();
        mp.Initialize(app);
        
        return app;
    }
}