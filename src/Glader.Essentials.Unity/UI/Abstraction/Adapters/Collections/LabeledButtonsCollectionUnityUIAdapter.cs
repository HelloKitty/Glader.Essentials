using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// Exposed designers the ability to register collections of <see cref="IUILabeledButton"/>s.
	/// </summary>
	public sealed class LabeledButtonsCollectionUnityUIAdapter : BaseCollectionUnityUIAdapter<IUILabeledButton>
	{
		[SerializeField]
		[Tooltip("The collection of labeled buttons to aggregate.")]
		private IUILabeledButton[] _elements;

		/// <inheritdoc />
		protected override IUILabeledButton[] Elements
		{
			get => _elements;
			set => _elements = value;
		}
	}
}
