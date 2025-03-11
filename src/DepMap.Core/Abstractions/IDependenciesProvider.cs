using DepMap.Core.Domain;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace DepMap.Core.Abstractions;

public interface IDependenciesProvider
{
    List<Dependency> GetDependencies(Type type, IList<Service> services);
    List<Dependency> GetActionDependencies(ActionDescriptor action, IList<Service> services);
}