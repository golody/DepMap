using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using DepMap.Tests.Mock.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DepMap.Tests.Services;

public class ServicesProviderTest : IClassFixture<MockApplicationFactory>
{
    private readonly IServicesProvider _services;

    public ServicesProviderTest(MockApplicationFactory factory)
    {
        var provider = factory.Services.GetRequiredService(typeof(IServicesProvider)) as IServicesProvider;
        _services = provider
                    ?? throw new NullReferenceException("IServicesProvider not found");
    }

    [Fact]
    public void Services_MustContain_MockServices()
    {
        Assert.Contains(_services.Services, service =>
            service.AbstractionType == typeof(IMockService) &&
            service.ImplementationType == typeof(MockService1)
        );

        Assert.Contains(_services.Services, service =>
            service.AbstractionType == typeof(IMockService) &&
            service.ImplementationType == typeof(MockService2)
        );

        Assert.Contains(_services.Services, service =>
            service.AbstractionType == typeof(IMockServiceUser) &&
            service.ImplementationType == typeof(MockServiceUserConstructor)
        );

        Assert.Contains(_services.Services, service =>
            service.AbstractionType == typeof(IMockServiceUser) &&
            service.ImplementationType == typeof(MockServiceUserParams)
        );

        Assert.Contains(_services.Services, service =>
            service.AbstractionType == typeof(IMockServiceUser) &&
            service.ImplementationType == typeof(MockServiceUserIEnumerable)
        );
    }

    [Fact]
    public void MockServiceUserIEnumerable_ShoudDependFrom_MockServices()
    {
        var userie = _services.Services.First(service =>
            service.AbstractionType == typeof(IMockServiceUser) &&
            service.ImplementationType == typeof(MockServiceUserIEnumerable)
        );
        HasDependency(
            userie,
            typeof(IMockService),
            typeof(MockService1));
        HasDependency(
            userie,
            typeof(IMockService),
            typeof(MockService2));
    }

    [Fact]
    public void MockServiceConstructor_ShoudDependFrom_MockServices()
    {
        var userc = _services.Services.First(service =>
            service.AbstractionType == typeof(IMockServiceUser) &&
            service.ImplementationType == typeof(MockServiceUserConstructor)
        );

        HasDependency(
            userc,
            typeof(MockService1),
            typeof(MockService1));
        HasDependency(
            userc,
            typeof(MockService2),
            typeof(MockService2));
    }

    [Fact]
    public void MockServiceUserParams_ShoudDependFrom_MockServices()
    {
        var userp = _services.Services.First(service =>
            service.AbstractionType == typeof(IMockServiceUser) &&
            service.ImplementationType == typeof(MockServiceUserParams)
        );
        HasDependency(
            userp,
            typeof(MockService1),
            typeof(MockService1));
        HasDependency(
            userp,
            typeof(IMockService),
            typeof(MockService2));
    }

    private void HasDependency(Service target, Type service, Type implementation)
    {
        Assert.Contains(target.Dependencies, dep =>
            dep.Type == service &&
            dep.Implementation?.ImplementationType == implementation
        );
    }
}