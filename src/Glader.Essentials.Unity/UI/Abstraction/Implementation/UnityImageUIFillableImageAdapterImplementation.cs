using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	public sealed class UnityImageUIFillableImageAdapterImplementation : BaseUnityUIAdapterImplementation, IUIFillableImage
	{
		/// <inheritdoc />
		protected override string LoggableComponentName => UnityImageObject.name;

		private UnityEngine.UI.Image UnityImageObject { get; }

		/// <inheritdoc />
		public UnityImageUIFillableImageAdapterImplementation([NotNull] Image unityImageObject)
		{
			UnityImageObject = unityImageObject ?? throw new ArgumentNullException(nameof(unityImageObject));
		}

		/// <inheritdoc />
		public float FillAmount
		{
			get => UnityImageObject.fillAmount;
			set => UnityImageObject.fillAmount = Mathf.Clamp(value, 0, 1.0f);
		}

		/// <inheritdoc />
		public void SetSpriteTexture(Texture2D texture)
		{
			if(UnityImageObject.sprite == null)
				UnityImageObject.sprite = Sprite.Create(texture, Rect.zero, Vector2.zero); //TODO: What should defaults be?
			else
			{
				//Sprites complain if we don't have proper size, so we need size based on the texture2D
				UnityImageObject.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			}
		}
	}
}
