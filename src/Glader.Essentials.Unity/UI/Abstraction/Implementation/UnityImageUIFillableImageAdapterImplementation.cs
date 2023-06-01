using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	public sealed class UnityImageUIFillableImageAdapterImplementation : BaseUnityUIAdapterImplementation, IUIFillableImage
	{
		private int SpriteTextureSetCounter = 1;

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
			Interlocked.Increment(ref SpriteTextureSetCounter);

			//Debug.Log($"Setting Texture: {texture.name} to Image with TextureName: {RetrieveCurrentSpriteName()} AreSame: {RetrieveCurrentSpriteName() == texture.name}");
			// We compare the names of the textures to avoid expensive sprite creation
			if(!string.IsNullOrEmpty(texture.name))
			{
				// If both have non-empty/non-null then we can compare and see if we
				// can actually avoid a Sprite creation
				if(RetrieveCurrentSpriteName() == texture.name)
					return;
			}

			if(UnityImageObject.sprite == null)
				UnityImageObject.sprite = Sprite.Create(texture, Rect.zero, Vector2.zero); //TODO: What should defaults be?
			else
			{
				//Sprites complain if we don't have proper size, so we need size based on the texture2D
				UnityImageObject.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			}
		}

		private string RetrieveCurrentSpriteName()
		{
			if(UnityImageObject == null)
				return String.Empty;

			if(UnityImageObject.sprite == null)
				return String.Empty;

			if(UnityImageObject.sprite.texture == null)
				return String.Empty;

			return UnityImageObject.sprite.texture.name;
		}

		/// <inheritdoc />
		public async Task SetSpriteTextureAsync(Task<Texture2D> textureAwaitable)
		{
			int counter = Interlocked.Increment(ref SpriteTextureSetCounter);
			var texture = await textureAwaitable;

			if (counter == SpriteTextureSetCounter)
				SetSpriteTexture(texture);
		}

		/// <inheritdoc />
		public Color ElementColor
		{
			get => UnityImageObject.color;
			set => UnityImageObject.color = value;
		}

		/// <inheritdoc />
		public void SetColor(byte r, byte g, byte b, byte a)
		{
			ElementColor = new Color32(r, g, b, a);
		}
	}
}
