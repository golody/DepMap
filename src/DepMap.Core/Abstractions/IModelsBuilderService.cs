using DepMap.Core.Domain;
using DepMap.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DepMap.Core.Abstractions;

/// <summary>
/// Service, that converts Data to Models, that can be used in GoJS library
/// </summary>
public interface IModelsBuilderService
{
    public (List<NodeModel> nodes, List<LinkModel> links)
        GenerateNodesAndLinks(List<Service> services, List<ControllerDescription> controllers,
            List<Middleware> middleware);
}