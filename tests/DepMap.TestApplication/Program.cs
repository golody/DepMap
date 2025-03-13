using DepMap.Extensions;
using DepMap.Test.Mock.Middleware;
using DepMap.Tests.Mock.Services;

namespace DepMap.Test;

public class MockApplication
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<IMockService, MockService1>();
        builder.Services.AddTransient<IMockService, MockService2>();
        builder.Services.AddTransient<MockService1>();
        builder.Services.AddTransient<MockService2>();
        builder.Services.AddTransient<IMockServiceUser, MockServiceUserConstructor>();
        builder.Services.AddTransient<IMockServiceUser, MockServiceUserParams>();
        builder.Services.AddTransient<IMockServiceUser, MockServiceUserIEnumerable>();
        builder.Services.AddDepMap();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseDirectoryBrowser("/db");
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Mock}/{action=Index}/{id?}");
        
        app.Use(async (context, next) =>
        {
            await next(context);
        });
        
        app.UseMiddleware<MockMiddleware>();
        app.UseDepMap();
        app.Run();
    }
}
