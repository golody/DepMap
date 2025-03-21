using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using ServiceProviderOptions = DepMap.Core.Options.ServiceProviderOptions;

namespace DepMap.Infrastructure.Services;

public class ServicesProvider : IServicesProvider
{
    private readonly IDependenciesProvider _dependenciesProvider;
    private readonly ServiceProviderOptions _options;
    public List<Service> Services { get; } = [];
    public ServicesProvider(IDependenciesProvider dependenciesProvider, IHost host, IReflectionTweaks rt, ServiceProviderOptions so, ServiceProviderOptions options)
    {
        _dependenciesProvider = dependenciesProvider;
        _options = options;
        var serviceDescriptors = rt.GetServiceDescriptors(host);
        
        AddServices(serviceDescriptors);
        MapDependencies();
    }

    private void AddServices(ServiceDescriptor[] services)
    {
        foreach (ServiceDescriptor sd in services)
        {
            bool skip = _options.ExcludeServices.Any(rule => rule.Invoke(sd.ServiceType.ToString()));

            if (skip)
            {
                continue;
            }
            
            if (sd.IsKeyedService || sd.ImplementationType == null)
                continue; // not implemented
            
            Services.Add(new Service(sd.ServiceType, sd.ImplementationType));
        }
    }

    private void MapDependencies()
    {
        foreach (Service service in Services) {
            Type t = service.ImplementationType;
            service.Dependencies.AddRange(_dependenciesProvider.GetDependencies(t, Services));
        }
    }
}