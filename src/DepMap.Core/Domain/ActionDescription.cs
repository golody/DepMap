namespace DepMap.Core.Domain;

public class ActionDescription
{
    public string DisplayName { get; }
    public IDictionary<string, string?> Routes { get; }
    public IReadOnlyList<Dependency> Dependencies { get; }
    
    public ActionDescription(string displayName, IReadOnlyList<Dependency> dependencies, IDictionary<string, string?> routes)
    {
        DisplayName = displayName;
        Dependencies = dependencies;
        Routes = routes;
    }

}