namespace DepMap.Core.Models;

public class ServiceAbstractionModel
{
    public string Name { get; set; }
    public string Namespace { get; set; }
    public string Path { get; set; }
    public List<ServiceModel> Implementations { get; set; }

    public ServiceAbstractionModel(string name, string ns, string path, List<ServiceModel> implementations)
    {
        Name = name;
        Namespace = ns;
        Path = path;
        Implementations = implementations;
    }
}