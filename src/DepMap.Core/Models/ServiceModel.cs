namespace DepMap.Core.Models;

public class ServiceModel : IDepMapModel
{
    public string Name { get; set; }
    public string Namespace { get; set; }
    public string Path { get; set; }
    
    public ServiceModel(string serviceName, string serviceNamespace, string servicePath)
    {
        Name = serviceName;
        Namespace = serviceNamespace;
        Path = servicePath;
    }
}