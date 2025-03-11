using DepMap.Core.Abstractions;
using DepMap.Views.DepMap;
using Microsoft.AspNetCore.Mvc;

namespace DepMap.Controllers;

[Controller]
public class DepMapController : Controller
{
    private readonly IServicesProvider _servicesProvider;

    public DepMapController(IServicesProvider servicesProvider, IHost host)
    {
        _servicesProvider = servicesProvider;
    }
    public ViewResult Index()
    {
        return View();
    }
}