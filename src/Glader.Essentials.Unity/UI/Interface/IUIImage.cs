﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that implement UI image.
	/// </summary>
	public interface IUIImage : IUIElement, IUIUnityColorable
	{
		/// <summary>
		/// Sets the texture for the current sprite.
		/// </summary>
		/// <param name="texture"></param>
		void SetSpriteTexture(Texture2D texture);

		/// <summary>
		/// Sets the texture for the current sprite async.
		/// </summary>
		/// <param name="textureAwaitable">Awaitable that can produce a <see cref="Texture2D"/>.</param>
		Task SetSpriteTextureAsync(Task<Texture2D> textureAwaitable);
	}
}
