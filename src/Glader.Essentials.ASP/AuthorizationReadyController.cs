using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Glader.Essentials
{
	/// <summary>
	/// Base Controller that supports Authorization with <see cref="ClaimsReader"/>
	/// and derives from <see cref="BaseGladerController"/> with JSON response mode support.
	/// </summary>
	public abstract class AuthorizationReadyController : BaseGladerController
	{
		/// <summary>
		/// The reader for the claims.
		/// </summary>
		protected IClaimsPrincipalReader ClaimsReader { get; }

		/// <inheritdoc />
		protected AuthorizationReadyController([FromServices] IClaimsPrincipalReader claimsReader, [FromServices] ILogger<AuthorizationReadyController> logger)
			: base(logger)
		{
			ClaimsReader = claimsReader ?? throw new ArgumentNullException(nameof(claimsReader));
		}
	}
}
