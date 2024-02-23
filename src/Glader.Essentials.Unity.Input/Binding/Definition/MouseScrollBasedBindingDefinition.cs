using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using UnityEngine;
using UnityEngine.UIElements;

namespace Glader.Essentials
{
	/// <summary>
	/// Mouse scroll wheel-based implementation of <see cref="BindingDefinition"/>.
	/// </summary>
	public sealed record MouseScrollBasedBindingDefinition(bool Up, params KeyCode[] Modifiers) 
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
			if (Up)
				return Input.mouseScrollDelta.y > 0.0f;
			else
				return Input.mouseScrollDelta.y < 0.0f;
		}

		/// <inheritdoc />
		public override bool IsReleased()
		{
			// This is a ONE TIME sort of input, it's pressed by instantly considered released and never held.
			return true;
		}

		/// <inheritdoc />
		public override bool IsHeld()
		{
			// This is a ONE TIME sort of input, it's pressed by instantly considered released and never held.
			return false;
		}
	}
}
