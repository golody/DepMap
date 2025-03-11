using System.Collections.Immutable;
using DepMap.Core.Domain;

namespace DepMap.Core.Abstractions;

public interface IServicesProvider
{
    ImmutableList<Service> Services { get; }
    void ConsoleOutput();
}
