using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unitysync.Async;
using Object = System.Object;

namespace Glader.Essentials
{
	public abstract class BaseUnityUIImageAdapter<TAdaptedToType> : BaseUnityUIAdapter<Image, TAdaptedToType>, IUIImage
		where TAdaptedToType : IUIImage, IUIElement //just make sure it's a IUIImage
	{
		private int SpriteTextureSetCounter = 1;

		private Sprite LastSetSprite = null;

		/// <inheritdoc />
		public virtual void SetSpriteTexture(Texture2D texture)
		{
			Interlocked.Increment(ref SpriteTextureSetCounter);

			//Debug.Log($"Setting Texture: {texture.name} to Image with TextureName: {RetrieveCurrentSpriteName()} AreSame: {RetrieveCurrentSpriteName() == texture.name}");
			// We compare the names of the textures to avoid expensive sprite creation
			if(!string.IsNullOrEmpty(texture.name))
			{
				// If both have non-empty/non-null then we can compare and see if we
				// can actually avoid a Sprite creation
				if (RetrieveCurrentSpriteName() == texture.name)
					return;
			}

			//Sprites complain if we don't have proper size, so we need size based on the texture2D
			UnityUIObject.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

			try
			{
				var spriteToDestroy = LastSetSprite;
				LastSetSprite = null;

				if (spriteToDestroy != null)
					UnityEngine.Object.Destroy(spriteToDestroy);
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to destroy created sprite. Reason: {e}");
			}
			finally
			{
				LastSetSprite = UnityUIObject.sprite;
			}
		}

		private string RetrieveCurrentSpriteName()
		{
			if (UnityUIObject == null)
				return String.Empty;

			if (UnityUIObject.sprite == null)
				return String.Empty;

			if (UnityUIObject.sprite.texture == null)
				return String.Empty;

			return UnityUIObject.sprite.texture.name;
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

		/// <inheritdoc />
		public override void Dispose()
		{
			try
			{
				base.Dispose();
			}
			finally
			{
				if (LastSetSprite != null)
				{
					UnityEngine.Object.Destroy(LastSetSprite);
					LastSetSprite = null;
				}
			}
		}
	}

	/// <summary>
	/// Similar to <see cref="BaseUnityUIImageAdapter{TAdaptedToType}"/> but based on <see cref="RawImage"/>.
	/// </summary>
	/// <typeparam name="TAdaptedToType"></typeparam>
	public abstract class BaseUnityUIRawImageAdapter<TAdaptedToType> : BaseUnityUIAdapter<RawImage, TAdaptedToType>, IUIImage
		where TAdaptedToType : IUIImage, IUIElement //just make sure it's a IUIImage
	{
		private int SpriteTextureSetCounter = 1;

		private Texture2D LastSetTexture = null;

		/// <inheritdoc />
		public virtual void SetSpriteTexture(Texture2D texture)
		{
			Interlocked.Increment(ref SpriteTextureSetCounter);

			//Sprites complain if we don't have proper size, so we need size based on the texture2D
			UnityUIObject.texture = texture;

			try
			{
				var textureToDestroy = LastSetTexture;
				LastSetTexture = null;

				if (textureToDestroy != null)
					UnityEngine.Object.Destroy(textureToDestroy);
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to destroy the texture. Reason: {e}");
			}
			finally
			{
				LastSetTexture = texture;
			}
		}

		/// <inheritdoc />
		public async Task SetSpriteTextureAsync(Task<Texture2D> textureAwaitable)
		{
			int currentCounter = Interlocked.Increment(ref SpriteTextureSetCounter);
			var texture = await textureAwaitable;

			// Check ensures if another texture was added or set this one won't set the texture and will basically just discard.
			if(currentCounter == SpriteTextureSetCounter)
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

		/// <inheritdoc />
		public override void Dispose()
		{
			try
			{
				base.Dispose();
			}
			finally
			{
				if (LastSetTexture != null)
				{
					UnityEngine.Object.Destroy(LastSetTexture);
					LastSetTexture = null;
				}
			}
		}
	}
}
