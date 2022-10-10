using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unitysync.Async;

namespace Glader.Essentials
{
	public abstract class BaseUnityUIImageAdapter<TAdaptedToType> : BaseUnityUIAdapter<Image, TAdaptedToType>, IUIImage
		where TAdaptedToType : IUIImage, IUIElement //just make sure it's a IUIImage
	{
		private int SpriteTextureSetCounter = 1;

		/// <inheritdoc />
		public virtual void SetSpriteTexture(Texture2D texture)
		{
			Interlocked.Increment(ref SpriteTextureSetCounter);
			//Sprites complain if we don't have proper size, so we need size based on the texture2D
			UnityUIObject.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		}

		/// <inheritdoc />
		public async Task SetSpriteTextureAsync(Task<Texture2D> textureAwaitable)
		{
			int currentCounter = Interlocked.Increment(ref SpriteTextureSetCounter);
			var texture = await textureAwaitable;

			// Check ensures if another texture was added or set this one won't set the texture and will basically just discard.
			if (currentCounter == SpriteTextureSetCounter)
				SetSpriteTexture(texture);
		}

		/// <inheritdoc />
		public Color ElementColor
		{
			get => UnityUIObject.color;
			set => UnityUIObject.color = value;
		}

		/// <inheritdoc />
		public void SetColor(byte r, byte g, byte b, byte a)
		{
			ElementColor = new Color32(r, g, b, a);
		}
	}
}
