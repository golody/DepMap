namespace DepMap.Core.Domain;

public class ControllerDescription
{
    public Type Type { get; }
    public List<ActionDescription> Actions { get; }
    public List<Dependency> Dependencies { get; }

    public ControllerDescription(Type type, List<ActionDescription> actions, List<Dependency> dependencies)
    {
        Type = type;
        Actions = actions;
        Dependencies = dependencies;
    }
}