using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public sealed class UnitFrameElementsAdapter: MonoBehaviour, IUIAdapterRegisterable, IUIUnitFrame
	{
		[Tooltip("Used to determine wiring for UI dependencies.")]
		[SerializeField]
		private int _RegisterationKey;

		/// <summary>
		/// The registeration key for the adapted UI element.
		/// </summary>
		public int RegisterationKey => _RegisterationKey;

		/// <inheritdoc />
		public Type UIServiceType => typeof(IUIUnitFrame);

		/// <inheritdoc />
		public UILabeledBar HealthBar { get; private set; }

		/// <inheritdoc />
		public UILabeledBar TechniquePointsBar { get; private set; }

		/// <inheritdoc />
		public IUIText UnitName { get; private set; }

		/// <inheritdoc />
		public IUIText UnitLevel { get; private set; }

		//These are the actual serialzied fields.

		[SerializeField]
		private UnityEngine.UI.Text HealthText;

		[SerializeField]
		private UnityEngine.UI.Image HealthBarImage;

		[SerializeField]
		private UnityEngine.UI.Text TechniquePointsText;

		[SerializeField]
		private UnityEngine.UI.Image TechniquePointsBarImage;

		[SerializeField]
		private UnityEngine.UI.Text PlayerNameText;

		[SerializeField]
		private UnityEngine.UI.Text UnitLevelText;

		void Awake()
		{
			HealthBar = new UILabeledBar(new UnityTextUITextAdapterImplementation(HealthText), new UnityImageUIFillableImageAdapterImplementation(HealthBarImage));
			TechniquePointsBar = new UILabeledBar(new UnityTextUITextAdapterImplementation(TechniquePointsText), new UnityImageUIFillableImageAdapterImplementation(TechniquePointsBarImage));

			UnitName = new UnityTextUITextAdapterImplementation(PlayerNameText);
			UnitLevel = new UnityTextUITextAdapterImplementation(UnitLevelText);
		}

		/// <inheritdoc />
		public void SetElementActive(bool state)
		{
			gameObject.SetActive(state);
		}
	}
}
