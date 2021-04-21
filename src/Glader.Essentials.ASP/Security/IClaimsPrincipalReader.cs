using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace Glader.Essentials
{
	//From: //From HaloLive: https://github.com/HaloLive/HaloLive.Library/tree/9ca485677a8c6f85bf06de53193af704aa508dcd/src/HaloLive.Hosting.Authorization.Server/Services
	/// <summary>
	/// Claims reader based on https://github.com/aspnet/Identity/blob/f555a26b4a554f73eea70b4b34fca823fab9a643/src/Microsoft.Extensions.Identity.Core/UserManager.cs
	/// </summary>
	public interface IClaimsPrincipalReader
	{
		/// <summary>
		/// Returns the User ID claim value if present otherwise returns null.
		/// </summary>
		/// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
		/// <returns>The User ID claim value, or null if the claim is not present.</returns>
		/// <remarks>The User ID claim is identified by <see cref="ClaimTypes.NameIdentifier"/>.</remarks>
		string GetAccountId(ClaimsPrincipal principal);

		/// <summary>
		/// Returns the User's Sub-Account ID claim value if present otherwise returns null.
		/// </summary>
		/// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
		/// <returns>The User's Sub-Account ID claim value, or null if the claim is not present.</returns>
		/// <remarks>The User's Sub-Account ID claim is identified by <see cref="GladerEssentialsASPSecurityConstants.SUB_ACCOUNT_CLAIM"/>.</remarks>
		string GetSubAccountId(ClaimsPrincipal principal);

		/// <summary>
		/// Returns the Name claim value if present otherwise returns null.
		/// </summary>
		/// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
		/// <returns>The Name claim value, or null if the claim is not present.</returns>
		/// <remarks>The Name claim is identified by <see cref="ClaimsIdentity.DefaultNameClaimType"/>.</remarks>
		string GetAccountName(ClaimsPrincipal principal);

		/// <summary>
		/// Reads the UUID/GUID/JTI value from the <see cref="principal"/>.
		/// </summary>
		/// <param name="principal">The principal value.</param>
		/// <returns></returns>
		string GetGloballyUniqueUserId(ClaimsPrincipal principal);
	}
}