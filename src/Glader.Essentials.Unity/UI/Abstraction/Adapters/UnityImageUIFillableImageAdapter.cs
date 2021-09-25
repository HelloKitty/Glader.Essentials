using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	//Used for revalidation
	[ExecuteInEditMode]
	public sealed class UnityImageUIFillableImageAdapter : BaseUnityUIImageAdapter<IUIFillableImage>, IUIFillableImage
	{
		/// <inheritdoc />
		protected override IUIElement Element => Adapter.Value;

		//This is sorta the new design
		//Create an adapter property that will actually handle the adaptor
		//the responsibility of this class is to expose registeration and to
		//handle the internal complicated parts of exposing it to the editor.
		private Lazy<UnityImageUIFillableImageAdapterImplementation> Adapter { get; set; }

		public UnityImageUIFillableImageAdapter()
		{
			Adapter = new Lazy<UnityImageUIFillableImageAdapterImplementation>(() => new UnityImageUIFillableImageAdapterImplementation(this.UnityUIObject));
		}

		//TODO: This won't hold up if the Type changes.
		//Override validation to check image is fillable
		/// <inheritdoc />
		protected override bool ValidateInitializedObject(Image obj)
		{
			bool result = base.ValidateInitializedObject(obj);

			if(!result)
				return false;

			//Else, if it's valid for the base we need to check if it's fillable
			//image otherwise it won't work as a fillable image
			if(obj.type != Image.Type.Filled)
			{
				UnityEngine.Debug.LogError($"Failed to initialize Image on GameObject: {obj.gameObject.name} as Fillable Image. Was ImageType: {obj.type}");
				return false;
			}

			return true;
		}

		/// <inheritdoc />
		public float FillAmount
		{
			get => Adapter.Value.FillAmount;
			set => Adapter.Value.FillAmount = value;
		}

		/// <inheritdoc />
		public override void SetSpriteTexture(Texture2D texture)
		{
			Adapter.Value.SetSpriteTexture(texture);
		}
	}
}
