using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using UnityEngine;
using UnityEngine.UIElements;

namespace Glader.Essentials
{
	/// <summary>
	/// Mouse-based implementation of <see cref="BindingDefinition"/>.
	/// </summary>
	public sealed record MouseBasedBindingDefinition(MouseButton Button, params KeyCode[] Modifiers) 
		: BindingDefinition(Modifiers)
	{
		/*if (Input.GetMouseButton(0))
		    Debug.Log("The left mouse button is being held down.");
		if (Input.GetMouseButton(1))
		    Debug.Log("The right mouse button is being held down.");
		if (Input.GetMouseButton(2))
		    Debug.Log("The middle mouse button is being held down.");
		*/

		/// <inheritdoc />
		public override bool IsPressed()
		{
			switch(Button)
			{
				case MouseButton.LeftMouse:
					return Input.GetMouseButtonDown(0); // left
				case MouseButton.RightMouse:
					return Input.GetMouseButtonDown(1); // right
				case MouseButton.MiddleMouse:
					return Input.GetMouseButtonDown(2); // middle
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <inheritdoc />
		public override bool IsReleased()
		{
			switch(Button)
			{
				case MouseButton.LeftMouse:
					return Input.GetMouseButtonUp(0); // left
				case MouseButton.RightMouse:
					return Input.GetMouseButtonUp(1); // right
				case MouseButton.MiddleMouse:
					return Input.GetMouseButtonUp(2); // middle
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <inheritdoc />
		public override bool IsHeld()
		{
			switch(Button)
			{
				case MouseButton.LeftMouse:
					return Input.GetMouseButton(0); // left
				case MouseButton.RightMouse:
					return Input.GetMouseButton(1); // right
				case MouseButton.MiddleMouse:
					return Input.GetMouseButton(2); // middle
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
