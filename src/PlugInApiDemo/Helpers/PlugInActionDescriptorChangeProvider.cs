﻿using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace PlugInApiDemo.Helpers
{
	public class PlugInActionDescriptorChangeProvider : IActionDescriptorChangeProvider
	{
		public static PlugInActionDescriptorChangeProvider Instance { get; } =
			new PlugInActionDescriptorChangeProvider();
		public CancellationTokenSource TokenSource { get; private set; }

		public bool HasChanged { get; set; }
		public IChangeToken GetChangeToken()
		{
			TokenSource = new CancellationTokenSource();
			return new CancellationChangeToken(TokenSource.Token);
		}
	}
}
