using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
			: base(unityImageObject)
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

		/// <inheritdoc />
		public async Task SetSpriteTextureAsync(Task<Texture2D> textureAwaitable)
		{
			throw new NotImplementedException("Async fillable texture not implemented yet.");
		}

		/// <inheritdoc />
		public Color ElementColor
		{
			get => UnityImageObject.color;
			set => UnityImageObject.color = value;
		}
	}
}
