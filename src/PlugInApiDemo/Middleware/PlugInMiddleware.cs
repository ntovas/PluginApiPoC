using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using PlugInApiDemo.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlugInApiDemo.Middleware
{
	public class PlugInMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ApplicationPartManager _partManager;

		private readonly Dictionary<string, PluginLoadContext> _dllLoaders;

		private readonly Dictionary<string, List<string>> _parts;

		public PlugInMiddleware(RequestDelegate next, ApplicationPartManager partManager)
		{
			_next = next;
			_partManager = partManager;

			_dllLoaders = new Dictionary<string, PluginLoadContext>();
			_parts = new Dictionary<string, List<string>>();
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var request = context.Request.Path.ToString();
			var name = context.Request.Query["name"];
			var directory = context.Request.Query["dir"];

			if (request == "/")
			{
				context.Response.Clear();
				context.Response.StatusCode = 200;
				await context.Response.WriteAsync("Loaded");
			}
			else if (string.IsNullOrEmpty(request) || string.IsNullOrEmpty(name))
			{
				await _next(context);
			}
			else
			{
				if (request.StartsWith("/load"))
				{
					try
					{

						var files = Directory.GetFiles(directory, "*.dll");
						var loader = new PluginLoadContext();

						var asmList = new List<string>();
						foreach (var file in files)
						{
							var path = Path.Combine(directory, file);

							var assembly = loader.LoadFromAssemblyPath(path);
							asmList.Add(assembly.GetName().Name);
							_partManager.ApplicationParts.Add(new AssemblyPart(assembly));
						}

						PlugInActionDescriptorChangeProvider.Instance.HasChanged = true;
						PlugInActionDescriptorChangeProvider.Instance.TokenSource.Cancel();

						_dllLoaders.Add(name, loader);
						_parts.Add(name, asmList);

						context.Response.Clear();
						context.Response.StatusCode = 200;
						await context.Response.WriteAsync($"Api {name} loaded.");
					}
					catch (Exception e)
					{
						context.Response.Clear();
						context.Response.StatusCode = 200;
						await context.Response.WriteAsync(e.Message);
					}

				}
				else if (request.StartsWith("/remove"))
				{
					try
					{
						var part = _parts[name];

						foreach (var partName in part)
						{
							var appPart = _partManager.ApplicationParts
								.FirstOrDefault(c => c.Name == partName);

							_partManager.ApplicationParts.Remove(appPart);
						}

						PlugInActionDescriptorChangeProvider.Instance.HasChanged = true;
						PlugInActionDescriptorChangeProvider.Instance.TokenSource.Cancel();

						var loader = _dllLoaders[name];

						loader.Unload();

						_dllLoaders.Remove(name);
						_parts.Remove(name);

						context.Response.Clear();
						context.Response.StatusCode = 200;
						await context.Response.WriteAsync($"Api {name} removed.");
					}
					catch (Exception e)
					{
						context.Response.Clear();
						context.Response.StatusCode = 200;
						await context.Response.WriteAsync(e.Message);
					}
				}
			}
		}
	}
}
