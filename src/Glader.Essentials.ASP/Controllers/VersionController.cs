using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Glader.Essentials
{
	/// <summary>
	/// Controller that exposes a gettable version identifier over GET.
	/// </summary>
	[Route("api/[controller]")]
	public sealed class VersionController : Controller
	{
		/// <summary>
		/// The cached version string based on the executing assembly version.
		/// </summary>
		private static string VersionString { get; } = BuildVersionString();

		//Do not remove!
		static VersionController()
		{

		}

		private static string BuildVersionString()
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;

			if (version == null)
				return "unknown";
			else
				return $"{version}";
		}

		/// <summary>
		/// Returns an empty response with status code Ok (200).
		/// </summary>
		/// <returns>Returns </returns>
		[NoResponseCacheAttribute] //disable caching
		[HttpGet]
		public IActionResult GetVersion()
		{
			return Ok(VersionString);
		}
	}
}
