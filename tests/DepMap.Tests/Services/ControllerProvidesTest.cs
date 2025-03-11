using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using DepMap.Infrastructure.Services;
using DepMap.Test.Controllers;
using DepMap.Tests.Mock.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DepMap.Tests.Services;

public class ControllerProvidesTest : IClassFixture<MockApplicationFactory>
{
    private readonly IControllersProvider _controllers;

    public ControllerProvidesTest(MockApplicationFactory factory)
    {
        var provider = factory.Services.GetRequiredService(typeof(IControllersProvider)) as IControllersProvider;
        _controllers = provider
                       ?? throw new NullReferenceException("ServicesProvider not found");
    }

    [Fact]
    public void ControllerProvider_ShoudContain_MockController()
    {
        Assert.Contains(_controllers.Controllers, c =>
            c.ControllerInfo == typeof(MockController));
    }
    
    [Fact]
    public void MockController_ShoudContain_IndexAction()
    {
        ControllerDescription controller = _controllers.Controllers.First(c =>
            c.ControllerInfo == typeof(MockController));
        Assert.Contains(controller.Actions, c =>
            c.DisplayName == nameof(MockController.Index));
    }

    [Fact]
    public void IndexAction_ShoudHave_Dependency()
    {
        ControllerDescription controller = _controllers.Controllers.First(c =>
            c.ControllerInfo == typeof(MockController));
        
        var action = controller.Actions.First(c =>
            c.DisplayName == nameof(MockController.Index));
        
        Assert.Contains(action.Dependencies, d => 
            d.Type == typeof(MockService1) &&
            d.Implementation?.ImplementationType == typeof(MockService1)
        );
    }
    
    [Fact]
    public void MockController_ShoudHave_Dependecies()
    {
        ControllerDescription controller = _controllers.Controllers.First(c =>
            c.ControllerInfo == typeof(MockController));

        HasDependency(controller,
            typeof(MockService2),
            typeof(MockService2)
        );
        HasDependency(controller,
            typeof(IMockServiceUser),
            typeof(MockServiceUserConstructor)
        );
        HasDependency(controller,
            typeof(IMockServiceUser),
            typeof(MockServiceUserParams)
        );
        HasDependency(controller,
            typeof(IMockServiceUser),
            typeof(MockServiceUserIEnumerable)
        );
    }

    private void HasDependency(ControllerDescription target, Type service, Type implementation)
    {
        Assert.Contains(target.Dependencies, dep =>
            dep.Type == service &&
            dep.Implementation?.ImplementationType == implementation
        );
    }
}