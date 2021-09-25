using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	public interface IUIInteractable
	{
		/// <summary>
		/// Indicates if the element is interactable.
		/// </summary>
		bool IsInteractable { get; set; }
	}
}
