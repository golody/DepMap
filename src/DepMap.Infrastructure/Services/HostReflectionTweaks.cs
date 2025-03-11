using System.Reflection;
using DepMap.Core.Abstractions;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace DepMap.Infrastructure.Services;

public class HostReflectionTweaks : IReflectionTweaks
{
    private const string ApplicationBuilderProp = "ApplicationBuilder";
    private const string ComponentsField = "_components";
    private const string MiddlewareField = "_middleware";
    private const string CallSiteFactoryProp = "CallSiteFactory";
    private const string DescriptorsProp = "Descriptors";
    private const string ControllerTypeInfoProp = "ControllerTypeInfo";

    private ApplicationBuilder GetApplicationBuilder(WebApplication app)
    {
        var applicationBuilderInfo = app.GetType()
            .GetProperty(ApplicationBuilderProp, BindingFlags.Instance | BindingFlags.NonPublic);
        if (applicationBuilderInfo == null)
        {
            throw new Exception("WebApplication does not have a field named 'ApplicationBuilder.'");
        }
        return (applicationBuilderInfo.GetValue(app) as ApplicationBuilder)!;
    }

    public List<Func<RequestDelegate, RequestDelegate>> GetMiddleware(WebApplication wapp)
    {
        var app = GetApplicationBuilder(wapp);
        var componentsInfo = app.GetType().GetField(ComponentsField, BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (componentsInfo == null)
        {
            throw new Exception("ApplicationBuilder has no field named '_components'.");
        }

        var components = componentsInfo.GetValue(app);
        return (components as List<Func<RequestDelegate, RequestDelegate>>)!;
    }

    public Type? GetMiddlewareType(Func<RequestDelegate, RequestDelegate> rd)
    {
        FieldInfo? mwField = rd.Target?.GetType().GetField(MiddlewareField, BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (mwField == null)
            return null;

        return mwField.GetValue(rd.Target) as Type;
    }

    public ServiceDescriptor[] GetServiceDescriptors(IHost app)
    {
        var serviceProvider = app.Services;
        var callSiteFactoryField = serviceProvider.GetType()
            .GetProperty(CallSiteFactoryProp, BindingFlags.Instance | BindingFlags.NonPublic);
        if (callSiteFactoryField == null)
        {
            throw new Exception("CallSiteFactory has no field named 'CallSiteFactory'.");
        }

        var callSiteFactory = callSiteFactoryField.GetValue(serviceProvider)!;
        var descriptorsField = callSiteFactory.GetType()
            .GetProperty(DescriptorsProp, BindingFlags.Instance | BindingFlags.NonPublic);
        if (descriptorsField == null)
        {
            return null!;
        }

        return (descriptorsField.GetValue(callSiteFactory)! as ServiceDescriptor[])!;
    }

    public Type GetControllerTypeInfo(ActionDescriptor action)
    {
        var ctiField = action.GetType().GetProperty(ControllerTypeInfoProp, BindingFlags.Instance | BindingFlags.Public);
        
        if (ctiField == null)
            throw new Exception("CallSiteFactory has no field named 'CallSiteFactory'.");

        return (ctiField.GetValue(action) as Type)!;
    }
}