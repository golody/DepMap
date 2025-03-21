using DepMap.Core.Abstractions;
using DepMap.Core.Domain;
using DepMap.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DepMap.Infrastructure.Services;

public class ModelsBuilderService : IModelsBuilderService
{
	public (List<NodeModel> nodes, List<LinkModel> links)
		GenerateNodesAndLinks(List<Service> services, List<ControllerDescription> controllers,
			List<Middleware> middleware)
	{
		List<NodeModel> nodes = [];
		List<LinkModel> links = [];

		foreach (Service service in services)
		{
			AddService(service, nodes, links);
		}

		nodes.Add(new NodeModel(
			"Middleware",
			"Middleware",
			null,
			true,
			"middleware"
		));

		foreach (Middleware mw in middleware)
		{
			AddMiddleware(mw, nodes, links);
		}

		foreach (ControllerDescription controller in controllers)
		{
			AddController(controller, nodes, links);
		}

		return (nodes, links);
	}

	private void AddService(Service service, List<NodeModel> nodes, List<LinkModel> links)
	{
		if (nodes.Any(s =>
			    s.Key == service.ImplementationType.Name &&
			    s.Namespace == service.ImplementationType.Namespace
		    ))
		{
			return;
		}

		// Create group if service has no interface or abstract class
		if (service.AbstractionType == service.ImplementationType)
		{
			nodes.Add(new NodeModel(
				service.AbstractionType.Name,
				service.AbstractionType.Namespace,
				null,
				true,
				"service"
			));
		}

		// Else create group if not exists
		NodeModel? group = nodes.FirstOrDefault(s =>
			s.Key == service.AbstractionType.Name &&
			s.IsGroup
		);

		if (group == null)
		{
			nodes.Add(new NodeModel(
				service.AbstractionType.Name,
				service.AbstractionType.Namespace,
				null,
				true,
				"service"
			));
		}

		// Add node inside AbstractionType.Name group
		nodes.Add(new NodeModel(
			service.ImplementationType.Name,
			service.ImplementationType.Namespace,
			service.AbstractionType.Name,
			false,
			"service"
		));

		// Add links
		foreach (Dependency dependency in service.Dependencies)
		{
			// Link to implementation
			links.Add(new LinkModel(
				to: service.ImplementationType.Name,
				from: dependency.Implementation?.ImplementationType.Name!
			));
		}
	}

	private void AddController(ControllerDescription controller, List<NodeModel> nodes, List<LinkModel> links)
	{
		// Add controller as a group
		nodes.Add(new NodeModel(
			controller.Type.Name,
			controller.Type.Namespace,
			null,
			true,
			"controller"
		));

		// Add links to controller
		links.AddRange(controller.Dependencies.Select(dependency => new LinkModel(
			to: controller.Type.Name,
			from: dependency.Implementation?.ImplementationType.Name!
		)));

		// Add link to middleware group 
		links.Add(new LinkModel(to: controller.Type.Name, from: "Middleware"));

		// Add actions to group
		foreach (var action in controller.Actions)
		{
			nodes.Add(new NodeModel(
				controller.Type.Name + "." + action.DisplayName,
				controller.Type.Namespace,
				controller.Type.Name,
				false,
				"action"
			));

			// Add links to action
			links.AddRange(action.Dependencies.Select(dependency => new LinkModel(
				to: action.DisplayName,
				from: dependency.Implementation?.ImplementationType.Name!
			)));
		}
	}

	private void AddMiddleware(Middleware middleware, List<NodeModel> nodes, List<LinkModel> links)
	{
		nodes.Add(new NodeModel(
			middleware.ClassType?.Name ?? "app.Use(...) => { ... }",
			middleware.ClassType?.Namespace,
			"Middleware", // Put middleware in single group
			false,
			"middleware"
		));

		if (middleware.ClassType == null)
			return;

		// Add links
		links.AddRange(middleware.Dependencies.Select(dependency => new LinkModel(
			to: middleware.ClassType.Name,
			from: dependency.Implementation?.ImplementationType.Name!
		)));
	}
}