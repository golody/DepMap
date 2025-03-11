namespace DepMap.Core.Domain;

public class Service
{
    public Type AbstractionType { get; }
    public Type ImplementationType { get; }
    public List<Dependency> Dependencies = [];
    public Service(Type abstractionType, Type implementationType)
    {
        AbstractionType = abstractionType;
        ImplementationType = implementationType;
    }

    public override bool Equals(object? obj)
    {
        return GetHashCode() == obj?.GetHashCode();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ImplementationType, AbstractionType);
    }
}