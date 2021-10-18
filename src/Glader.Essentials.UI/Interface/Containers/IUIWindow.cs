using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// The container for all frames.
	/// </summary>
	public interface IUIWindow
	{

	}

	/// <summary>
	/// The container for all frames.
	/// </summary>
	public interface IUIWindow<out TRootFrameType> : IUIWindow
		where TRootFrameType : IUIFrame
	{
		/// <summary>
		/// The root/default frame that contains all free-floating elements.
		/// </summary>
		TRootFrameType Root { get; }
	}
}
