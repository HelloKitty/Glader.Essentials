using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Glader.Essentials
{
	/// <summary>
	/// Based on the SwanSong implementation of forcing the focus of a input.
	/// </summary>
	public sealed class ForceFocusedUnityTextMeshProUGUIUIInputFieldAdapter : UnityTextMeshProUGUIUIInputFieldAdapter
	{
		/// <summary>
		/// Sets the forcing of focus on the input.
		/// </summary>
		public bool ForcedFocus { get; private set; } = false;

		private Coroutine InputEndCoroutine = null;

		/// <summary>
		/// Call to setup the functionality for forcing focus.
		/// </summary>
		public void SetupForcedFocusHandling()
		{
			// These force it to stay in focus/selected
			UnityUIObject.onEndEdit.AddListener(arg =>
			{
				if(!ForcedFocus)
					return;

				HandleAttemptedEnd();
			});

			UnityUIObject.onDeselect.AddListener(arg =>
			{
				if(!ForcedFocus)
					return;

				HandleAttemptedEnd();
			});

			RegisterSelectCallback(arg =>
			{
				if (!ForcedFocus)
					StartInput();
			});

			UnityUIObject.onFocusSelectAll = false;
			UnityUIObject.resetOnDeActivation = false;
			UnityUIObject.restoreOriginalTextOnEscape = false;
		}

		private void HandleAttemptedEnd()
		{
			if(IsOverOtherTextInput())
				EndInput();
			else if(UnityUIObject.interactable)
				UnityUIObject.ActivateInputField();
		}

		/// <summary>
		/// Starts the input.
		/// </summary>
		public void StartInput()
		{
			// Cannot start an input right now.
			if(!UnityUIObject.interactable)
				return;

			// If an end is in progress we should stop it
			StopInputEndCoroutine();

			// If we're starting input then deactivate temporarily
			// So events can fire for reactivation
			ForcedFocus = true;
			Bus.Publish(this, new TextInputFieldActivatedStateChangedEventArgs(ForcedFocus));

			UnityUIObject.caretPosition = UnityUIObject.text.Length;
			UnityUIObject.ActivateInputField();
			UnityUIObject.Select();
			UnityUIObject.caretPosition = UnityUIObject.text.Length;
			// We set the caret position basically because of the SlashOpen binding.
		}

		/// <summary>
		/// Ends the input.
		/// </summary>
		public void EndInput()
		{
			UnityUIObject.interactable = false;
			DisableInput();

			StopInputEndCoroutine();
			InputEndCoroutine = StartCoroutine(EndInputCoroutine());
		}

		private void StopInputEndCoroutine()
		{
			if (InputEndCoroutine != null)
				StopCoroutine(InputEndCoroutine);
			InputEndCoroutine = null;
		}

		private void DisableInput(bool includeEventSystemDeselect = true)
		{
			ForcedFocus = false;
			Bus.Publish(this, new TextInputFieldActivatedStateChangedEventArgs(false));
			UnityUIObject.ReleaseSelection();
			UnityUIObject.DeactivateInputField(true);
			UnityUIObject.ReleaseSelection();
			UnityUIObject.SetTextWithoutNotify(String.Empty);

			// For whatever reason, all the above isn't enough to release the input
			if(includeEventSystemDeselect &&
				!EventSystem.current.alreadySelecting // Will throw otherwise
				&& EventSystem.current.currentSelectedGameObject == UnityUIObject.gameObject)
				EventSystem.current.SetSelectedGameObject(null);
		}

		private IEnumerator EndInputCoroutine()
		{
			yield return new WaitForEndOfFrame();
			DisableInput();
			UnityUIObject.interactable = true;
		}

		private bool IsOverOtherTextInput()
		{
			if(!Cursor.visible)
				return false;

			if(EventSystem.current.IsNull())
				return false;

			// Based on: https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/#post-7987800
			PointerEventData eventData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, raycastResults);

			return raycastResults
				.Where(r => r.gameObject.layer != (int)Physics.IgnoreRaycastLayer)
				.Any(g => g.gameObject.GetComponent<TMP_InputField>() && gameObject != g.gameObject);
		}
	}
}
