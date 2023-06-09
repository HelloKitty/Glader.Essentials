using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	/// <summary>
	/// Adapters for the Unity3D UI Type: <see cref="Text"/> adapted to
	/// Guardians Type: <see cref="IUIText"/>.
	/// </summary>
	public sealed class UnityTextMeshProUIDropDownAdapter : BaseUnityUIAdapter<TMPro.TMP_Dropdown, IUIDropDown>, IUIDropDown
	{
		/// <inheritdoc />
		public int SelectedIndex
		{
			get => UnityUIObject.value;
			set => UnityUIObject.value = value;
		}

		/// <inheritdoc />
		public string SelectedValue => UnityUIObject.itemText.text;

		private void Start()
		{
			// Wires up the OnSelectionChanged event
			UnityUIObject.onValueChanged.AddListener(index => Bus.Publish(this, new DropDownSelectionChangedEventArgs(SelectedIndex, SelectedValue)));
		}

		/// <inheritdoc />
		public void AddOptions(IEnumerable<string> options)
		{
			UnityUIObject.AddOptions(options.ToList());
		}

		/// <inheritdoc />
		public void Clear()
		{
			UnityUIObject.ClearOptions();
		}

		/// <inheritdoc />
		public IEnumerator<string> GetEnumerator()
		{
			foreach (var option in UnityUIObject.options)
				yield return option.text;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
