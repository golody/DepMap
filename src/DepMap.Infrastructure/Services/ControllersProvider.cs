using System.Collections.Immutable;
using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DepMap.Infrastructure.Services;

public class ControllersProvider : IControllersProvider
{
    public List<ControllerDescription> Controllers { get; } = [];

    public ControllersProvider(
        IReflectionTweaks rt,
        IDependenciesProvider dp,
        IActionDescriptorCollectionProvider endpoints,
        IServicesProvider services)
    {
        foreach (var actionDescriptor in endpoints.ActionDescriptors.Items)
        {
            var action = (ControllerActionDescriptor)actionDescriptor;
            var actionDependencies = dp.GetActionDependencies(action, services.Services);
            ActionDescription ad = new(
                action.ActionName!,
                actionDependencies,
                action.RouteValues
            );

            Type controllerType = action.ControllerTypeInfo;
            ControllerDescription? controller = Controllers.FirstOrDefault(c =>
                c.Type == controllerType
            );
            
            if (controller == null)
            {
                var controllerDependencies = dp.GetDependencies(controllerType, services.Services);
                Controllers.Add(new ControllerDescription(controllerType, [ad], controllerDependencies));
                
                continue;
            }

            controller.Actions.Add(ad);
        }
    }
}