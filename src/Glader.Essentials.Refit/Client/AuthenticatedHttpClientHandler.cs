using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Glader.Essentials
{
	/// <summary>
	/// <see cref="HttpClientHandler"/> that will replace Authentication headers
	/// with the contents of the provided <see cref="IReadonlyAuthTokenRepository"/>.
	/// </summary>
	public sealed class AuthenticatedHttpClientHandler : DelegatingHandler
	{
		/// <summary>
		/// Token provider.
		/// </summary>
		private IReadonlyAuthTokenRepository AuthTokenProvider { get; }

		/// <inheritdoc />
		public AuthenticatedHttpClientHandler(IReadonlyAuthTokenRepository authTokenProvider)
			: base(new HttpClientHandler())
		{
			AuthTokenProvider = authTokenProvider ?? throw new ArgumentNullException(nameof(authTokenProvider));
		}

		/// <inheritdoc />
		public AuthenticatedHttpClientHandler(IReadonlyAuthTokenRepository authTokenProvider, HttpClientHandler innerHandler)
			: base(innerHandler)
		{
			if (innerHandler == null) throw new ArgumentNullException(nameof(innerHandler));
			AuthTokenProvider = authTokenProvider ?? throw new ArgumentNullException(nameof(authTokenProvider));
		}

		/// <inheritdoc />
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			//This will set the authentication token if it's required.
			AuthenticationHeaderValue auth = request.Headers.Authorization;
			if(auth != null)
				request.Headers.Authorization = new AuthenticationHeaderValue(AuthTokenProvider.RetrieveType(), AuthTokenProvider.Retrieve());

			return base.SendAsync(request, cancellationToken);
		}
	}
}
