using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for a binding definition.
	/// </summary>
	public abstract record BindingDefinition(params KeyCode[] Modifiers)
	{
		/// <summary>
		/// Indicates if the binding is pressed.
		/// </summary>
		/// <returns>True if it's just been pressed.</returns>
		public abstract bool IsPressed();

		/// <summary>
		/// Indicates if the binding has been released.
		/// </summary>
		/// <returns>True if it's just been released.</returns>
		public abstract bool IsReleased();

		/// <summary>
		/// Indicates if the binding has been held.
		/// </summary>
		/// <returns></returns>
		public abstract bool IsHeld();
	}
}
