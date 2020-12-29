using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// <see cref="HttpClientHandler"/> that will bypass cert checks.
	/// </summary>
	public sealed class BypassHttpsValidationHandler : HttpClientHandler
	{
		public BypassHttpsValidationHandler()
		{
			this.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
		}
	}
}
