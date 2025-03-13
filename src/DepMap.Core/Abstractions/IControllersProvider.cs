using DepMap.Core.Domain;

namespace DepMap.Core.Abstractions;

public interface IControllersProvider
{
    public List<ControllerDescription> Controllers { get; }
}