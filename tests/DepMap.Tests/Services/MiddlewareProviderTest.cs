using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using DepMap.Test.Mock.Middleware;
using DepMap.Tests.Mock.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DepMap.Tests.Services;

public class MiddlewareProviderTest : IClassFixture<MockApplicationFactory>
{
    private readonly IMiddlewareProvider _middleware;

    public MiddlewareProviderTest(MockApplicationFactory factory)
    {
        var provider = factory.Services.GetRequiredService(typeof(IMiddlewareProvider)) as IMiddlewareProvider;
        _middleware = provider
                      ?? throw new NullReferenceException("IMiddlewareProvider not found");
    }

    [Fact]
    public void ContainsMockMiddleware()
    {
        Assert.Contains(_middleware.Middleware, m => m.ClassType == typeof(MockMiddleware));
    }

    [Fact]
    public void MockMiddleware_ShoudDependFrom_MockServices()
    {
        var middleware = _middleware.Middleware.First(m => m.ClassType == typeof(MockMiddleware));
        HasDependency(
            middleware,
            typeof(MockService1),
            typeof(MockService1)
        );
    }

    private void HasDependency(Middleware target, Type service, Type implementation)
    {
        Assert.Contains(target.Dependencies, dep =>
            dep.Type == service &&
            dep.Implementation?.ImplementationType == implementation
        );
    }
}