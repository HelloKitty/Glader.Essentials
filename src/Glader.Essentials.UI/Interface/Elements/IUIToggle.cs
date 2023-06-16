using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	public interface IUIToggle : IUIElement<IUIToggle>, IUIInteractable
	{
		/// <summary>
		/// Indicates if the toggle is toggled on.
		/// </summary>
		bool IsToggled { get; }
	}
}
