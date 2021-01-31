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
	/// <see cref="HttpClientHandler"/> that will replace the base-URL with an async future URL.
	/// </summary>
	public sealed class AsyncEndpointHttpClientHandler : DelegatingHandler
	{
		/// <summary>
		/// Token provider.
		/// </summary>
		private Task<string> EndpointFuture { get; }

		/// <inheritdoc />
		public AsyncEndpointHttpClientHandler(Task<string> endpointFuture)
			: this(endpointFuture, new HttpClientHandler())
		{

		}

		/// <inheritdoc />
		public AsyncEndpointHttpClientHandler(Task<string> endpointFuture, HttpClientHandler innerHandler)
			: base(innerHandler)
		{
			if (innerHandler == null) throw new ArgumentNullException(nameof(innerHandler));
			EndpointFuture = endpointFuture;
		}

		/// <inheritdoc />
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			//We build {base}{relative} async (completes immediately if already complete.
			string endpoint = await EndpointFuture;
			request.RequestUri = new Uri(new Uri(endpoint), request.RequestUri);

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
