using DepMap.Tests.Mock.Services;

namespace DepMap.Test.Mock.Middleware;

public class MockMiddleware
{
    public MockService2 Mock2 = null!;
    private readonly RequestDelegate _next;

    public MockMiddleware(RequestDelegate next, MockService1 service)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
    }
}