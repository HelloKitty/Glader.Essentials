using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Booma
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
