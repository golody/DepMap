namespace DepMap.Core.Options;

public class ServiceProviderOptions
{
	public required Func<string, bool>[] ExcludeServices { get; set; }

	public static ServiceProviderOptions Default = new() {
		ExcludeServices = [
			nmspc => nmspc.Contains("Microsoft"),
			nmspc => nmspc.Contains("System"),
			nmspc => nmspc.Contains("Newtownsoft"),
			nmspc => nmspc.Contains("Schwashbuckle"),
			nmspc => nmspc.Contains("Serilog"),
		]
	};
}