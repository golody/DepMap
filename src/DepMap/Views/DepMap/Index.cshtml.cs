using DepMap.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DepMap.Views.DepMap;

public class IndexModel : PageModel
{
    public List<NodeModel> Services { get; set; }
    public List<LinkModel> ServiceDependencies { get; set; }

    public IndexModel(List<NodeModel> services, List<LinkModel> serviceDependencies)
    {
        Services = services;
        ServiceDependencies = serviceDependencies;
    }
}