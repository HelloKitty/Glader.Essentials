using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	//TODO: Doc
	public interface IUIAdapterRegisterable
	{
		int RegisterationKey { get; }

		/// <summary>
		/// The actual type to register this UI adapter as.
		/// </summary>
		Type UISerivdeType { get; }
	}
}
