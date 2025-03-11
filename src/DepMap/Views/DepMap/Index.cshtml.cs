using DepMap.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DepMap.Views.DepMap;

public class IndexModel : PageModel
{
    public List<ServiceAbstractionModel> Services { get; set; }
    public List<DependencyModel<ServiceModel>> ServiceDependencies { get; set; }

    public IndexModel(List<DependencyModel<ServiceModel>> serviceDependencies, List<ServiceAbstractionModel> services)
    {
        ServiceDependencies = serviceDependencies;
        Services = services;
    }
}