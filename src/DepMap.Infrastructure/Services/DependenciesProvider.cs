using System.Collections;using System.Reflection;
using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace DepMap.Infrastructure.Services;

public class DependenciesProvider : IDependenciesProvider
{
    private List<Service> _services = [];
    private List<Dependency> _dependencies = [];

    public List<Dependency> GetDependencies(Type type, IList<Service> services)
    {
        _dependencies = new List<Dependency>();
        _services = services.ToList();
        
        var constructors = type.GetConstructors();
        if (constructors.Length > 0)
        {
            AddConstructorDependencies(constructors[0]);
        }

        foreach (PropertyInfo property in type.GetProperties())
        {
            AddPropertyDependency(property);
        }

        return _dependencies;
    }

    public List<Dependency> GetActionDependencies(ActionDescriptor action, IList<Service> services)
    {
        _dependencies = new List<Dependency>();
        _services = services.ToList();
        
        var list = new List<Dependency>();
        
        foreach (var parameterDescriptor in action.Parameters)
        {
            var parameter = (ControllerParameterDescriptor)parameterDescriptor;
            // Search for parameters with [FromServices] attribute
            if (parameter.ParameterInfo.CustomAttributes.Any(
                    attr => attr.AttributeType == typeof(FromServicesAttribute)))
            {
                var dependencies = FindDependenciesForType(parameter.ParameterType);
                list.AddRange(dependencies);
            }
        }

        return list;
    }

    private void AddConstructorDependencies(ConstructorInfo constructor)
    {
        foreach (var parameter in constructor.GetParameters())
        {
            var dependencies = FindDependenciesForType(parameter.ParameterType);
            foreach (Dependency dependency in dependencies)
            {
                if (!_dependencies.Contains(dependency))
                {
                    _dependencies.Add(dependency);
                }
            }
        }
    }

    private void AddPropertyDependency(PropertyInfo property)
    {
        var dependencies = FindDependenciesForType(property.PropertyType);
        foreach (Dependency? dependency in dependencies)
        {
            if (!_dependencies.Contains(dependency))
            {
                _dependencies.Add(dependency);
            }
        }
    }

    // Search the type in registered services
    private Dependency[] FindDependenciesForType(Type type)
    {
        // Look for IEnumerable<ISomeService>, List<IService> etc.
        if (type.IsGenericType &&
            type.GenericTypeArguments.Length == 1 &&
            typeof(IEnumerable).IsAssignableFrom(type)
           )
        {
            return _services
                .Where(s => s.AbstractionType == type.GenericTypeArguments[0])
                .Select(s => new Dependency(s.AbstractionType, s))
                .ToArray();
        }

        for (int i = _services.Count - 1; i >= 0; i--)
        {
            Service service = _services[i];
            if (type == service.ImplementationType && type == service.AbstractionType)
            {
                return [new Dependency(service.ImplementationType, service)];
            }

            if (type == service.AbstractionType)
            {
                return [new Dependency(service.AbstractionType, service)];
            }
        }

        return [];
    }
}