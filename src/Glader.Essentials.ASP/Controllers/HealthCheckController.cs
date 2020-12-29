using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Glader.Essentials
{
	/// <summary>
	/// General health check controller that exposes a GET action at
	/// /api/healthcheck that returns a status code of 200 if everything is
	/// healthy.
	/// </summary>
	[Route("api/[controller]")]
	public sealed class HealthCheckController : Controller
	{
		private ILogger<HealthCheckController> Logger { get; }

		/// <inheritdoc />
		public HealthCheckController(ILogger<HealthCheckController> logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Returns an empty response with status code Ok (200).
		/// </summary>
		/// <returns>Returns </returns>
		[NoResponseCache] //disable caching
		[HttpGet]
		public IActionResult Check()
		{
			if (Logger.IsEnabled(LogLevel.Debug))
				Logger.LogDebug($"Health check from: {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}");

			//TODO: Should we make some determinations other than this? Maybe if we have too many ongoing requests?
			return Ok();
		}
	}
}
