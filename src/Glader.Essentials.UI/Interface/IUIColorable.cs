using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IUIColorable
	{
		/// <summary>
		/// Sets the element's color to the specific RGBA color values.
		/// </summary>
		/// <param name="r">Red.</param>
		/// <param name="g">Green.</param>
		/// <param name="b">Blue.</param>
		/// <param name="a">Alpha.</param>
		void SetColor(byte r, byte g, byte b, byte a);
	}
}
