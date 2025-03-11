using System.Collections.Immutable;
using DepMap.Core.Domain;
using Microsoft.AspNetCore.Builder;

namespace DepMap.Core.Abstractions;

public interface IMiddlewareProvider
{
    bool Initialized { get; }
    IReadOnlyList<Middleware> Middleware { get; }
    void Initialize(WebApplication app);
}
