// noinspection JSUnresolvedReference

const $ = go.GraphObject.make;

function makeDiagram(id, nodeDataArray, linkDataArray) {
	const diagram = $(go.Diagram, id, {
		layout: $(go.LayeredDigraphLayout, {
			direction: 0, // Left to right layout
		})
	});
	diagram.model = new go.GraphLinksModel(nodeDataArray, linkDataArray);
	return diagram;
}
function getColor(type) {
	if (type === "controller")
		return "#4a77cf";
	if (type === "service")
		return "#f1f144";
	if (type === "middleware")
		return "#e45353";
	return "white";
}
function makeGroupTemplate(diagram) {
	diagram.groupTemplate =
		$(go.Group, "Auto",
			{
				layout: $(go.GridLayout,
					{
						wrappingColumn: 1,
						cellSize: new go.Size(1, 1),
						spacing: new go.Size(50, 20),
						alignment: go.GridLayout.Position
					}
				),
				selectable: true,
				computesBoundsAfterDrag: true,
				computesBoundsIncludingLocation: true,
				resizable: true,
				isSubGraphExpanded: true
			},
			$(go.Shape,
				"RoundedRectangle",
				{
					fill: "lightgray",
					stroke: null,
				},
				new go.Binding("fill", "type", getColor)
			),
			$(go.Panel, "Vertical",
				{margin: 5},
				$(go.TextBlock, {font: "Bold 12pt Sans-Serif", margin: 5},
					new go.Binding("text", "key")),
				$(go.Placeholder, {padding: 5})
			)
		);
}

function makeNodeTemplate(diagram) {
	diagram.nodeTemplate =
		$(go.Node, "Auto",
			$(go.Shape, "RoundedRectangle",
				{
					fill: "white",  // Background color
					stroke: null,
					parameter1: 10  // Corner radius for rounded edges
				}
			),
			$(go.Panel, "Vertical",
				{padding: 2},  // Adds 5px padding around the content
				$(go.TextBlock,
					{
						margin: 2,
						wrap: go.TextBlock.WrapFit,
						textAlign: "center"
					},
					new go.Binding("text", "key")  // Binds text to node data
				)
			)
		);
}

function makeLinkTemplate(diagram) {
	diagram.linkTemplate = new go.Link(
		{
			curve: go.Curve.Bezier,
			fill: "white"
		}
	).add(
		$(go.Shape, { stroke: "white", strokeWidth: 2 }),
	);
}