namespace DepMap.Core.Models;

public class DependencyModel<T> where T : IDepMapModel
{
    public T Model { get; set; }
    public ServiceModel Dependency { get; set; }
    
    public DependencyModel(T model, ServiceModel dependency)
    {
        Model = model;
        Dependency = dependency;
    }
}