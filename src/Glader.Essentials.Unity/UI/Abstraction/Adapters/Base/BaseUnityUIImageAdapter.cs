using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	public abstract class BaseUnityUIImageAdapter<TAdaptedToType> : BaseUnityUIAdapter<Image, TAdaptedToType>, IUIImage
		where TAdaptedToType : IUIImage //just make sure it's a IUIImage
	{
		/// <inheritdoc />
		public virtual void SetSpriteTexture(Texture2D texture)
		{
			//Sprites complain if we don't have proper size, so we need size based on the texture2D
			UnityUIObject.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		}
	}
}
