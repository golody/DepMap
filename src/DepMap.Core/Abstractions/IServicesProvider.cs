using System.Collections.Immutable;
using DepMap.Core.Domain;

namespace DepMap.Core.Abstractions;

public interface IServicesProvider
{
    List<Service> Services { get; }
    void ConsoleOutput();
}
