using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Glader.Essentials
{
	public static class IClaimsPrincipalReaderExtensions
	{
		/// <summary>
		/// Returns the User ID claim value if present otherwise will throw.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
		/// <returns>The User ID claim value. Will throw if the principal doesn't contain the id.</returns>
		/// <exception cref="ArgumentException">Throws if the provided principal doesn't contain an id.</exception>
		/// <remarks>The User ID claim is identified by <see cref="ClaimTypes.NameIdentifier"/>.</remarks>
		public static int GetAccountIdInt(this IClaimsPrincipalReader reader, ClaimsPrincipal principal)
		{
			if(!int.TryParse(reader.GetAccountId(principal), out var accountId))
				throw new ArgumentException($"Provided {nameof(ClaimsPrincipal)} does not contain a user id.", nameof(principal));

			return accountId;
		}

		//TODO: Doc.
		public static int GetPlayerAccountId(this IClaimsPrincipalReader reader, ClaimsPrincipal principal)
		{
			string accountIdString = principal.FindFirstValue("sub");

			if(int.TryParse(accountIdString, out int resultValue))
				return resultValue;
			else
				throw new InvalidOperationException($"Failed to read Player AccountId Claim: {"sub"}");
		}
	}
}