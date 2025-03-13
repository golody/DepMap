namespace DepMap.Core.Models;

public class LinkModel
{
    public string From { get; set; }
    public string To { get; set; }

    public LinkModel(string to, string from)
    {
        To = to;
        From = from;
    }
}