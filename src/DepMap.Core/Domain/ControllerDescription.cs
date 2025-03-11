namespace DepMap.Core.Domain;

public class ControllerDescription
{
    public Type ControllerInfo { get; }
    public List<ActionDescription> Actions { get; }
    public List<Dependency> Dependencies { get; }

    public ControllerDescription(Type controllerInfo, List<ActionDescription> actions, List<Dependency> dependencies)
    {
        ControllerInfo = controllerInfo;
        Actions = actions;
        Dependencies = dependencies;
    }
}