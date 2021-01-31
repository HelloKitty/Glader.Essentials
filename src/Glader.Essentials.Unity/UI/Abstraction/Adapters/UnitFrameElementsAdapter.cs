using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	public sealed class UnitFrameElementsAdapter : MonoBehaviour, IUIAdapterRegisterable, IUIUnitFrame
	{
		[Tooltip("Used to determine wiring for UI dependencies.")]
		[SerializeField]
		private string _RegistrationKey;

		/// <summary>
		/// The registration key for the adapted UI element.
		/// </summary>
		public string RegistrationKey => _RegistrationKey;

		/// <inheritdoc />
		public Type UIServiceType => typeof(IUIUnitFrame);

		/// <inheritdoc />
		public UILabeledBar HealthBar => _healthBar.Value;

		/// <inheritdoc />
		public UILabeledBar TechniquePointsBar => _techniquePointsBar.Value;

		/// <inheritdoc />
		public IUIText UnitName => _unitName.Value;

		/// <inheritdoc />
		public IUIText UnitLevel => _unitLevel.Value;

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

		private readonly Lazy<UILabeledBar> _healthBar;
		private readonly Lazy<UILabeledBar> _techniquePointsBar;
		private readonly Lazy<IUIText> _unitName;
		private readonly Lazy<IUIText> _unitLevel;

		//Do it in CTOR even though Unity says not to.
		//Because sometimes we can't wait for Awake to happen.
		public UnitFrameElementsAdapter()
		{
			_healthBar = new Lazy<UILabeledBar>(() => new UILabeledBar(new UnityTextUITextAdapterImplementation(HealthText), new UnityImageUIFillableImageAdapterImplementation(HealthBarImage)));
			_techniquePointsBar = new Lazy<UILabeledBar>(() => new UILabeledBar(new UnityTextUITextAdapterImplementation(TechniquePointsText), new UnityImageUIFillableImageAdapterImplementation(TechniquePointsBarImage)));

			_unitName = new Lazy<IUIText>(() => new UnityTextUITextAdapterImplementation(PlayerNameText));
			_unitLevel = new Lazy<IUIText>(() => new UnityTextUITextAdapterImplementation(UnitLevelText));
		}

		/// <inheritdoc />
		public void SetElementActive(bool state)
		{
			gameObject.SetActive(state);
		}

		public bool isActive => gameObject.activeSelf;
	}
}
