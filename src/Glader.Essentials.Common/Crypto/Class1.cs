using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Glader.Essentials
{
	public static class CertificateHelpers
	{
		/// <summary>
		/// Derives an ECDSA P-256 self-signed X509Certificate2 from a shared seed.
		/// </summary>
		public static X509Certificate2 CreateSelfSignedCertFromSeed(
			string seed,
			string subjectName,
			byte[]? salt = null,
			int iterations = 100_000,
			ECCurve? curve = null,
			DateTimeOffset? notBefore = null,
			DateTimeOffset? notAfter = null)
		{
			salt ??= new byte[] { 0x00 };  // fixed salt
			curve ??= ECCurve.NamedCurves.nistP256;

			// 1) Derive private-key bytes
			var seedBytes = Encoding.UTF8.GetBytes(seed);
			using var kdf = new Rfc2898DeriveBytes(seedBytes, salt, iterations, HashAlgorithmName.SHA256);
			// P-256 needs 32-byte D
			byte[] d = kdf.GetBytes(32);

			// 2) Build ECDsa from parameters
			var ecParams = new ECParameters { Curve = curve.Value, D = d };
			using var ecdsa = ECDsa.Create(ecParams)!;

			// 3) Create the cert request
			var req = new CertificateRequest(
				new X500DistinguishedName(subjectName),
				ecdsa,
				HashAlgorithmName.SHA256);

			// Optional: add basic constraints (no CA)
			req.CertificateExtensions.Add(
				new X509BasicConstraintsExtension(
					certificateAuthority: false,
					hasPathLengthConstraint: false,
					pathLengthConstraint: 0,
					critical: false));

			// 4) Self-sign
			var nb = notBefore ?? DateTimeOffset.UtcNow;
			var na = notAfter ?? nb.AddYears(1);
			using var cert = req.CreateSelfSigned(nb, na);

			// 5) Export + re-import as X509Certificate2 with private key
			var pfx = cert.Export(X509ContentType.Pfx, password: "");
			return new X509Certificate2(
				pfx,
				"",
				X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
		}
	}
}
