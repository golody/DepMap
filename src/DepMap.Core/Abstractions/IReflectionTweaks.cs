using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DepMap.Core.Abstractions;

public interface IReflectionTweaks
{
    List<Func<RequestDelegate, RequestDelegate>> GetMiddleware(WebApplication wapp);
    Type? GetMiddlewareType(Func<RequestDelegate, RequestDelegate> rd);
    ServiceDescriptor[] GetServiceDescriptors(IHost app);
    Type GetControllerTypeInfo(ActionDescriptor action);
}