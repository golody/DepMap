using DepMap.Core.Abstractions;
using DepMap.Infrastructure.Services;

namespace DepMap.Extensions;

public static class DepMapExtensions
{
    public static IServiceCollection AddDepMap(this IServiceCollection services)
    {
        services.AddTransient<IDependenciesProvider, DependenciesProvider>();
        services.AddTransient<IServicesProvider, ServicesProvider>();
        services.AddSingleton<IMiddlewareProvider, MiddlewareProvider>();
        services.AddTransient<IControllersProvider, ControllersProvider>();
        services.AddTransient<IReflectionTweaks, HostReflectionTweaks>();
        
        return services;
    }

    public static WebApplication UseDepMap(this WebApplication app)
    {
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