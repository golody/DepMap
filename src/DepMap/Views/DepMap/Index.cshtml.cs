using DepMap.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DepMap.Views.DepMap;

public class IndexModel : PageModel
{
	public List<NodeModel> Nodes { get; set; }
	public List<LinkModel> ServiceDependencies { get; set; }

	public IndexModel(List<NodeModel> nodes, List<LinkModel> serviceDependencies)
	{
		Nodes = nodes;
		ServiceDependencies = serviceDependencies;
	}
}