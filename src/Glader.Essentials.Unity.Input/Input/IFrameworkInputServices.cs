using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Glader.Essentials
{
	/// <summary>
	/// Based on SwanSong's IEngineInputServices
	/// </summary>
	public interface IFrameworkInputServices
	{
		/// <summary>
		/// Determines if the provided <see cref="screenPosition"/> is over a raycastable UI element.
		/// </summary>
		/// <param name="screenPosition">The screen position to check.</param>
		/// <returns>True if this position is over a UI element.</returns>
		bool IsPositionOverUI(Vector2 screenPosition);

		/// <summary>
		/// Determines if the current input position is over a raycastable UI element.
		/// </summary>
		/// <returns>True if this current input position is over a UI element.</returns>
		bool IsPositionOverUI();

		/// <summary>
		/// Determines if the current input position is over a <see cref="ScrollRect"/>-like element.
		/// </summary>
		/// <returns>True if the input position is over a <see cref="ScrollRect"/>-like element.</returns>
		bool IsOverScrollableUI();

		/// <summary>
		/// Indicates if the client is currently using a text input.
		/// </summary>
		/// <returns></returns>
		bool IsUsingTextInput();

		/// <summary>
		/// Indicates if the mouse is within the game screen/window
		/// </summary>
		/// <returns></returns>
		bool IsMouseWithinGameWindow();
	}
}
