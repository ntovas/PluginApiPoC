using System.Runtime.Loader;

namespace PlugInApiDemo.Helpers
{
	public class PluginLoadContext : AssemblyLoadContext
	{
		public PluginLoadContext() : base(true) { }
	}
}
