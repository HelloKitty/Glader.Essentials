using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials.Unity;

namespace Glader.Essentials
{
	/// <summary>
	/// Event fired by the <see cref="DefaultGameFrameworkManager"/> after the game framework has been initialized.
	/// </summary>
	public sealed record GameFrameworkInitializationFinishedEventArgs()
		: IEventBusEventArgs
	{
		/// <summary>
		/// Shared instance for the init event.
		/// </summary>
		public static GameFrameworkInitializationFinishedEventArgs Instance { get; } = new();
	}
}
