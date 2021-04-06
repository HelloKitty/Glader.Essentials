using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Default simple implementation of <see cref="IServiceBaseUrlFactory"/>.
	/// </summary>
	public sealed class DefaultServiceBaseUrlFactory : IServiceBaseUrlFactory
	{
		/// <inheritdoc />
		public string Create(Uri context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			return context.GetLeftPart(UriPartial.Authority);
		}
	}
}
