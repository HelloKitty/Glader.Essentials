using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	/// <summary>
	/// Adapters for the Unity3D UI Type: <see cref="Text"/> adapted to
	/// Guardians Type: <see cref="IUIText"/>.
	/// </summary>
	public sealed class UnityTextUITextAdapter : BaseUnityUIAdapter<Text, IUIText>, IUIText
	{
		//This is sorta the new design
		//Create an adapter property that will actually handle the adaptor
		//the responsibility of this class is to expose registeration and to
		//handle the internal complicated parts of exposing it to the editor.
		private Lazy<UnityTextUITextAdapterImplementation> Adapter { get; set; }

		public UnityTextUITextAdapter()
		{
			Adapter = new Lazy<UnityTextUITextAdapterImplementation>(() => new UnityTextUITextAdapterImplementation(UnityUIObject));
		}

		/// <inheritdoc />
		public string Text
		{
			get => Adapter.Value.Text;
			set => Adapter.Value.Text = value;
		}

		/// <inheritdoc />
		public void SetColor(byte r, byte g, byte b, byte a)
		{
			Adapter.Value.SetColor(r, g, b, a);
		}
	}
}
