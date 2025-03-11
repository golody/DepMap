using System.Collections.Immutable;
using DepMap.Core.Abstractions;
using DepMap.Core.Domain;

namespace DepMap.Infrastructure.Services;

public class ServicesProvider : IServicesProvider
{
    private readonly IDependenciesProvider _dependenciesProvider;
    public ImmutableList<Service> Services { get; private set; } = [];

    public ServicesProvider(IDependenciesProvider dependenciesProvider, IHost host, IReflectionTweaks rt)
    {
        _dependenciesProvider = dependenciesProvider;
        var serviceDescriptors = rt.GetServiceDescriptors(host);
        
        AddServices(serviceDescriptors);
        MapDependencies();
    }

    private void AddServices(ServiceDescriptor[] services)
    {
        var list = new List<Service>();
        foreach (ServiceDescriptor sd in services) {
            if (sd.IsKeyedService || sd.ImplementationType == null)
                continue; // not implemented
            
            list.Add(new Service(sd.ServiceType, sd.ImplementationType));
        }
        Services = list.ToImmutableList();
    }

    private void MapDependencies()
    {
        foreach (var service in Services) {
            Type t = service.ImplementationType;
            service.Dependencies.AddRange(_dependenciesProvider.GetDependencies(t, Services));
        }
    }

    public void ConsoleOutput()
    {
        foreach (Service s in Services) {
            Console.WriteLine("Inteface: " + s.AbstractionType);
            Console.WriteLine("|-Implementation: " + s.AbstractionType);
            Console.WriteLine("|-Dependencies: ");
            foreach (Dependency d in s.Dependencies) {
                Console.WriteLine("|---Inteface: " + d.Type);
                Console.WriteLine("|---Implementation: " + d.Implementation?.ImplementationType);
            }
        }
    }
}