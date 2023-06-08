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
		[SerializeField]
		private TMPro.TMP_Dropdown DropDownComponent;

		/// <inheritdoc />
		public int SelectedIndex => DropDownComponent.value;

		/// <inheritdoc />
		public string SelectedValue => DropDownComponent.itemText.text;

		private void Start()
		{
			// Wires up the OnSelectionChanged event
			DropDownComponent.onValueChanged.AddListener(index => Bus.Publish(this, new DropDownSelectionChangedEventArgs(SelectedIndex, SelectedValue)));
		}

		/// <inheritdoc />
		public void AddOptions(IEnumerable<string> options)
		{
			DropDownComponent.AddOptions(options.ToList());
		}

		/// <inheritdoc />
		public void Clear()
		{
			DropDownComponent.ClearOptions();
		}

		/// <inheritdoc />
		public IEnumerator<string> GetEnumerator()
		{
			foreach (var option in DropDownComponent.options)
				yield return option.text;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
