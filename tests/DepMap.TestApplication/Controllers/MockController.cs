using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DepMap.Tests.Mock.Services;

namespace DepMap.Test.Controllers;

public class MockController : Controller
{
    private readonly ILogger<MockController> _logger;
    public IEnumerable<IMockServiceUser> MockServiceUser { get; set; } = null!;

    public MockController(ILogger<MockController> logger, MockService2 service)
    {
        _logger = logger;
    }

    public IActionResult Index([FromServices] MockService1 ms1)
    {
        return View();
    }
}