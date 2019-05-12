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
		private UnityLabeledButtonAdapter[] _elements;

		/// <inheritdoc />
		protected override IEnumerable<IUILabeledButton> Elements => _elements;
	}
}
