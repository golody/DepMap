using System.Collections.Immutable;
using DepMap.Core.Abstractions;
using DepMap.Core.Domain;

namespace DepMap.Infrastructure.Services;

public class MiddlewareProvider : IMiddlewareProvider
{
    private readonly IDependenciesProvider _dependencies;
    private readonly IServicesProvider _services;
    private readonly IReflectionTweaks _rt;
    private ImmutableList<Middleware> _middleware = null!;

    public bool Initialized { get; private set; }

    public IReadOnlyList<Middleware> Middleware {
        get {
            if (Initialized)
                return _middleware;

            throw new InvalidOperationException("MiddlewareProvider is not initialized");
        }
    }

    public MiddlewareProvider(
        IDependenciesProvider dependencies, 
        IServicesProvider services, 
        IReflectionTweaks rt)
    {
        _dependencies = dependencies;
        _services = services;
        _rt = rt;
    }
    
    // Only way I found to get the registered middleware is to get it before app.Build()
    public void Initialize(WebApplication app)
    {
        var list = new List<Middleware>();

        var middleware = _rt.GetMiddleware(app);

        foreach (var mw in middleware)
        {
            Type? type = _rt.GetMiddlewareType(mw);
            Middleware m;
            
            if (type != null)
            {
                m = new(type);
                m.Dependencies = _dependencies.GetDependencies(m.ClassType!, _services.Services);
            }
            // Anonymous middleware
            // app.Use((...) => {...})
            else
            {
                m = new();
            }
            list.Add(m);
        }

        Initialized = true;
        _middleware = list.ToImmutableList();
    }
}