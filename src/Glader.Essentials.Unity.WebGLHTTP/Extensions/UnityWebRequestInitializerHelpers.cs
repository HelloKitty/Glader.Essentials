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
	}
}
