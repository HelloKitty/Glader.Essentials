using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Glader.Essentials
{
	/// <summary>
	/// Helper class for handling text links easier.
	/// </summary>
	public static class UnityTextLinkHelper
	{
		/// <summary>
		/// Subscribers will handler <see cref="TextLinkClickedEventArgs"/> events globally.
		/// Important to unsubscribe.
		/// </summary>
		public static EventHandler<TextLinkClickedEventArgs> OnLinkClicked;

		/// <summary>
		/// Internal used. Used for publishing <see cref="TextLinkClickedEventArgs"/> through <see cref="OnLinkClicked"/>.
		/// </summary>
		/// <param name="sender">The link sender.</param>
		/// <param name="linkArgs">The link args data.</param>
		internal static void Publish(object sender, [NotNull] TextLinkClickedEventArgs linkArgs)
		{
			if (linkArgs == null) throw new ArgumentNullException(nameof(linkArgs));

			OnLinkClicked?.Invoke(sender, linkArgs);
		}
	}
}
