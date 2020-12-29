using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public static int GetAccountId(this IClaimsPrincipalReader reader, ClaimsPrincipal principal)
		{
			if(!int.TryParse(reader.GetAccountId(principal), out var accountId))
				throw new ArgumentException($"Provided {nameof(ClaimsPrincipal)} does not contain a user id.", nameof(principal));

			return accountId;
		}

		/// <summary>
		/// Returns the User ID claim value if present otherwise will throw.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
		/// <returns>The User ID claim value. Will throw if the principal doesn't contain the id.</returns>
		/// <exception cref="ArgumentException">Throws if the provided principal doesn't contain an id.</exception>
		/// <remarks>The User ID claim is identified by <see cref="ClaimTypes.NameIdentifier"/>.</remarks>
		public static TIdentifierType GetAccountId<TIdentifierType>(this IClaimsPrincipalReader reader, ClaimsPrincipal principal)
		{
			try
			{
				string id = reader.GetAccountId(principal);

				if(typeof(TIdentifierType) == typeof(string))
					return (TIdentifierType)(object)id;

				if(id == default)
					return default;

				return (TIdentifierType)Convert.ChangeType(id, typeof(TIdentifierType));
			}
			catch (Exception e)
			{
				throw new ArgumentException($"Provided {nameof(ClaimsPrincipal)} does not contain a user id.", nameof(principal), e);
			}
		}
	}
}