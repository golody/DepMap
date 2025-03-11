using System.Collections.Immutable;
using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DepMap.Infrastructure.Services;

public class ControllersProvider : IControllersProvider
{
    public IReadOnlyList<ControllerDescription> Controllers { get; }

    public ControllersProvider(
        IReflectionTweaks rt,
        IDependenciesProvider dp,
        IActionDescriptorCollectionProvider endpoints,
        IServicesProvider services)
    {
        var list = new List<ControllerDescription>();
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
            ControllerDescription? controller = list.FirstOrDefault(c =>
                c.ControllerInfo == controllerType
            );
            
            if (controller == null)
            {
                var controllerDependencies = dp.GetDependencies(controllerType, services.Services);
                list.Add(new ControllerDescription(controllerType, [ad], controllerDependencies));
                
                continue;
            }

            controller.Actions.Add(ad);
        }

        Controllers = list.ToImmutableArray();
    }
}