namespace DepMap.Core.Models;

public class NodeModel
{
    public string Key { get; set; }
    public string? Namespace { get; set; }
    public string? Group { get; set; }
    public bool IsGroup { get; set; }
    public string Type { get; set; }
    
    public NodeModel(string serviceKey, string? serviceNamespace, string? group, bool isGroup, string type)
    {
        Key = serviceKey;
        Namespace = serviceNamespace;
        Group = group;
        IsGroup = isGroup;
        Type = type;
    }
}