namespace DepMap.Core.Domain;

public class Middleware
{
    public Type? ClassType { get; }
    public List<Dependency> Dependencies { get; set; } = [];

    public Middleware() 
    { }
    public Middleware(Type classType)
    {
        ClassType = classType;
    }
}