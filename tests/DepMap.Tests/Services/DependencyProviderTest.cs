using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using DepMap.Test.Mock.Middleware;
using DepMap.Tests.Mock.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DepMap.Tests.Services;

public class DependencyProviderTest : IClassFixture<MockApplicationFactory>
{
    private readonly IServicesProvider _services;

    public DependencyProviderTest(MockApplicationFactory factory)
    {
        var sp = factory.Services.GetRequiredService(typeof(IServicesProvider)) as IServicesProvider;
        _services = sp 
                    ?? throw new NullReferenceException("ServicesProvider not found");
    }

    [Fact]
    public void DependencyProvider_TypesMatch()
    {
        Assert.All(_services.Services.SelectMany(s => s.Dependencies), TypesMatch);
    }
    
    private void TypesMatch(Dependency dependency)
    {
        Assert.Equal(dependency.Type, dependency.Implementation?.AbstractionType);
    }
}