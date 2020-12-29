using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Glader.Essentials
{
	public static class GladerASPIMvcBuilderExtensions
	{
		/// <summary>
		/// Registers the general <see cref="HealthCheckController"/> with the MVC
		/// controllers. See controller documentation for what it does and how it works.
		/// </summary>
		/// <param name="builder">The MVC builder.</param>
		/// <returns>The MVC builder.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IMvcBuilder RegisterHealthCheckController(this IMvcBuilder builder)
		{
			return RegisterController<HealthCheckController>(builder);
		}

		/// <summary>
		/// Registers the general <see cref="VersionController"/> with the MVC
		/// controllers. See controller documentation for what it does and how it works.
		/// </summary>
		/// <param name="builder">The MVC builder.</param>
		/// <returns>The MVC builder.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IMvcBuilder RegisterVersionController(this IMvcBuilder builder)
		{
			return RegisterController<VersionController>(builder);
		}

		/// <summary>
		/// Registers a controller of the specified type <typeparamref name="TControllerType"/>.
		/// </summary>
		/// <typeparam name="TControllerType">The controller type to register.</typeparam>
		/// <param name="builder">The MVC builder.</param>
		/// <returns>The MVC builder.</returns>
		public static IMvcBuilder RegisterController<TControllerType>(this IMvcBuilder builder)
			where TControllerType : Controller
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			//See AddControllersAsServices: https://github.com/aspnet/Mvc/blob/747420e5aa7cc2c7834cfb9731510286ded6fc03/src/Microsoft.AspNetCore.Mvc.Core/DependencyInjection/MvcCoreMvcCoreBuilderExtensions.cs#L107
			return builder.ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new GenericControllerFeatureProvider<TControllerType>()));
		}

		/// <summary>
		/// ASP Core feature provider that registers a controller.
		/// </summary>
		private class GenericControllerFeatureProvider<TControllerType> : IApplicationFeatureProvider<ControllerFeature>
			where TControllerType : Controller
		{
			/// <inheritdoc />
			public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
			{
				//We need to check this because, for some reason, this could be called twice? Was registered twice better for some reason
				//So don't remove this check
				if(!feature.Controllers.Contains(typeof(TControllerType).GetTypeInfo()))
					feature.Controllers.Add(typeof(TControllerType).GetTypeInfo());
			}
		}
	}
}
