using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Glader.Essentials
{
	//From HaloLive: https://github.com/HaloLive/HaloLive.Library/tree/9ca485677a8c6f85bf06de53193af704aa508dcd/src/HaloLive.Hosting.Authorization.Server/Services
	public sealed class ClaimsPrincipalReader : IClaimsPrincipalReader
	{
		/// <summary>
		/// The <see cref="IdentityOptions"/> used to configure Identity.
		/// </summary>
		private IdentityOptions Options { get; }

		public ClaimsPrincipalReader(IOptions<IdentityOptions> options)
		{
			//We don't allow defaults like ASP implementation does
			if(options == null) throw new ArgumentNullException(nameof(options));

			Options = options.Value;
		}

		/// <inheritdoc />
		public string GetAccountId(ClaimsPrincipal principal)
		{
			if(principal == null)
				throw new ArgumentNullException(nameof(principal));

			return principal.FindFirstValue(Options.ClaimsIdentity.UserIdClaimType);
		}

		/// <inheritdoc />
		public string GetSubAccountId(ClaimsPrincipal principal)
		{
			if (principal == null) throw new ArgumentNullException(nameof(principal));

			return principal.FindFirstValue(GladerEssentialsASPSecurityConstants.SUB_ACCOUNT_CLAIM);
		}

		/// <inheritdoc />
		public string GetAccountName(ClaimsPrincipal principal)
		{
			if(principal == null)
				throw new ArgumentNullException(nameof(principal));

			return principal.FindFirstValue(Options.ClaimsIdentity.UserNameClaimType);
		}

		/// <inheritdoc />
		public string GetGloballyUniqueUserId(ClaimsPrincipal principal)
		{
			if(principal == null) throw new ArgumentNullException(nameof(principal));

			Claim claim = principal.FindFirst("jti");

			if(claim == null)
				throw new InvalidOperationException($"The principal does not contain a claim for GUID/UUID (JTI).");

			return claim.Value;
		}
	}
}