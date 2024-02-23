using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Glader.Essentials
{
	/// <summary>
	/// Default implementation of <see cref="IFrameworkInputServices"/>
	/// </summary>
	public class DefaultFrameworkInputServices : IFrameworkInputServices
	{
		/// <summary>
		/// The current position of the cursor.
		/// Override to control this
		/// (Ex. SwanSong used to lock the position of the mouse in the center of the screen and needed to rewrite current position).
		/// </summary>
		protected virtual Vector2 CurrentCursorPosition => Input.mousePosition;

		/// <inheritdoc />
		public bool IsPositionOverUI(Vector2 screenPosition)
		{
			if (EventSystem.current.IsNull())
				return false;

			// Based on: https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/#post-7987800
			PointerEventData eventData = new PointerEventData(EventSystem.current)
			{
				position = screenPosition
			};

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, raycastResults);

			// TODO: SwanSong used to make this ignore layer configurable
			return raycastResults
				.Any(r => r.gameObject.layer != (int)Physics.IgnoreRaycastLayer);
		}

		/// <inheritdoc />
		public bool IsPositionOverUI()
		{
			// Less efficient, but it allows us to ignore the Ignore Raycast layer
			return IsPositionOverUI(CurrentCursorPosition);
		}

		/// <inheritdoc />
		public bool IsOverScrollableUI()
		{
			if(EventSystem.current.IsNull())
				return false;

			if(!EventSystem.current.IsPointerOverGameObject())
				return false;

			// Based on: https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/#post-7987800
			PointerEventData eventData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, raycastResults);

			return raycastResults
				.Any(r => r.gameObject.GetComponent<ScrollRect>() != null);
		}

		/// <inheritdoc />
		public bool IsUsingTextInput()
		{
			if(EventSystem.current.IsNull())
				return false;

			if(EventSystem.current.currentSelectedGameObject == null)
				return false;

			// If we have a input field selected then we're using a text input basically.
			return EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null;
		}

		/// <inheritdoc />
		public bool IsMouseWithinGameWindow()
		{
			// From: https://discussions.unity.com/t/how-can-i-tell-if-the-mouse-is-over-the-game-window/139428/4
			bool outsideWindowRange = (Input.mousePosition.x < 0
					 || Input.mousePosition.y < 0
					 || Input.mousePosition.x > Screen.width
					 || Input.mousePosition.y > Screen.height);

			return !outsideWindowRange;
		}
	}
}
