using DepMap.Core.Domain;

namespace DepMap.Core.Abstractions;

public interface IControllersProvider
{
    public IReadOnlyList<ControllerDescription> Controllers { get; }
}