using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that implement UI image.
	/// </summary>
	public interface IUIImage : IUIElement, IUIColorable
	{
		/// <summary>
		/// Sets the texture for the current sprite.
		/// </summary>
		/// <param name="texture"></param>
		void SetSpriteTexture(Texture2D texture);
	}
}
