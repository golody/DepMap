namespace DepMap.Core.Domain;

public class Dependency
{
    public Type Type { get; }
    public Service? Implementation { get; }

    public Dependency(Type type, Service? implementation)
    {
        Type = type;
        Implementation = implementation;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Implementation);
    }
    
    public override bool Equals(object? o)
    {
        return GetHashCode() == o?.GetHashCode();
    }
}