using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UnityEngine.Networking;

namespace Glader.Essentials
{
	public static class UnityWebRequestInitializerHelpers
	{
		internal sealed class DisabledCertificateHandler : CertificateHandler
		{
			/// <summary>
			/// Shared <see cref="DisabledCertificateHandler"/> instance.
			/// </summary>
			public static CertificateHandler Instance { get; } = new DisabledCertificateHandler();

			/// <inheritdoc />
			protected override bool ValidateCertificate(byte[] certificateData)
			{
				return true;
			}
		}

		/// <summary>
		/// Disables the certificate checks for the provided <see cref="request"/>.
		/// </summary>
		/// <param name="request">The request to disable the cert checks for.</param>
		public static void DisableCertCheck([NotNull] UnityWebRequest request)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));
			request.certificateHandler = DisabledCertificateHandler.Instance;
		}

		/// <summary>
		/// Creates a <see cref="UnityWebRequest"/> for the provided HTTP <see cref="type"/> with the provided <see cref="url"/> and <see cref="data"/>.
		/// </summary>
		/// <param name="type">The HTTP type. (Ex. GET)</param>
		/// <param name="url">The url/uri</param>
		/// <param name="data">The request data. Used for Post/Put.</param>
		/// <returns>A unsent <see cref="UnityWebRequest"/>. Call must send.</returns>
		public static UnityWebRequest CreateRequest(UnityHttpRequestType type, string url, string data = null)
		{
			switch (type)
			{
				case UnityHttpRequestType.Delete:
					return UnityWebRequest.Delete(url);
				case UnityHttpRequestType.Get:
					return UnityWebRequest.Get(url);
				case UnityHttpRequestType.Head:
					return UnityWebRequest.Head(url);
				case UnityHttpRequestType.Post:
					return UnityWebRequest.Post(url, data);
				case UnityHttpRequestType.Put:
					return UnityWebRequest.Put(url, data);
				case UnityHttpRequestType.Option:
				case UnityHttpRequestType.Patch:
				case UnityHttpRequestType.Trace:
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}
