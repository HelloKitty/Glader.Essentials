using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Glader.Essentials
{
	public static class GladerASPIServiceCollectionExtensions
	{
		/// <summary>
		/// Registers all the required/related Glader ASP services.
		/// </summary>
		/// <param name="serviceCollection"></param>
		/// <returns></returns>
		public static IServiceCollection RegisterGladerASP(this IServiceCollection serviceCollection)
		{
			if(serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

			serviceCollection.AddSingleton<IClaimsPrincipalReader, ClaimsPrincipalReader>();

			return serviceCollection;
		}
	}
}
