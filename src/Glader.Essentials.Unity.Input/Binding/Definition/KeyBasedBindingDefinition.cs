using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	/// <inheritdoc />
	public sealed record KeyBasedBindingDefinition(KeyCode Code, params KeyCode[] Modifiers) 
		: BindingDefinition(Modifiers)
	{
		/// <inheritdoc />
		public override bool IsPressed()
		{
			return Input.GetKeyDown(Code);
		}

		/// <inheritdoc />
		public override bool IsReleased()
		{
			return Input.GetKeyUp(Code);
		}

		/// <inheritdoc />
		public override bool IsHeld()
		{
			return Input.GetKey(Code);
		}
	}
}
