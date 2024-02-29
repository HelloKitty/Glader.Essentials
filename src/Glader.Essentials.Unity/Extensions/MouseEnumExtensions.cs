using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Glader.Essentials
{
	public static class MouseEnumExtensions
	{
		/// <summary>
		/// Converts the <see cref="PointerEventData"/> mouse button type to <see cref="MouseButton"/> enum.
		/// </summary>
		/// <param name="inputButton">The input type.</param>
		/// <returns>The mouse button type.</returns>
		public static MouseButton ToMouseButtonType(this PointerEventData.InputButton inputButton)
		{
			switch (inputButton)
			{
				case PointerEventData.InputButton.Left:
					return MouseButton.LeftMouse;
				case PointerEventData.InputButton.Right:
					return MouseButton.RightMouse;
				case PointerEventData.InputButton.Middle:
					return MouseButton.MiddleMouse;
				default:
					throw new ArgumentOutOfRangeException(nameof(inputButton), inputButton, null);
			}
		}
	}
}
