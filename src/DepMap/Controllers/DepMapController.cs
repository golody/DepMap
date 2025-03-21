using DepMap.Core.Abstractions;
using DepMap.Views.DepMap;
using Microsoft.AspNetCore.Mvc;

namespace DepMap.Controllers;

[Controller]
public class DepMapController : Controller
{
	private readonly IServicesProvider _sP;
	private readonly IControllersProvider _cP;
	private readonly IMiddlewareProvider _mP;
	private readonly IModelsBuilderService _modelBuilder;

	public DepMapController(IServicesProvider sP, IControllersProvider cP, IMiddlewareProvider mP,
		IModelsBuilderService modelBuilder)
	{
		_sP = sP;
		_cP = cP;
		_mP = mP;
		_modelBuilder = modelBuilder;
	}

	public ViewResult Index()
	{
		var data = _modelBuilder.GenerateNodesAndLinks(_sP.Services, _cP.Controllers, _mP.Middleware);

		return View(new IndexModel(data.nodes, data.links));
	}
}