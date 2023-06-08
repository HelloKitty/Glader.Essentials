using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Glader.Essentials
{
	public interface IUIDropDown : IUIElement<IUIDropDown>, IEnumerable<string>
	{
		/// <summary>
		/// Adds the specified options to the dropdown.
		/// </summary>
		/// <param name="options">The options.</param>
		void AddOptions(IEnumerable<string> options);

		/// <summary>
		/// Clears the options from the dropdown.
		/// </summary>
		void Clear();

		/// <summary>
		/// The zero-based index into the options selected.
		/// </summary>
		int SelectedIndex { get; }

		/// <summary>
		/// The selected string option text at <see cref="SelectedIndex"/>
		/// </summary>
		string SelectedValue { get; }
	}

	public static class IUIDropDownExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static EventBusSubscriptionToken OnSelectionChanged(this IUIDropDown dropdown, EventHandler<DropDownSelectionChangedEventArgs> callback)
		{
			return dropdown.Bus
				.Subscribe<DropDownSelectionChangedEventArgs>(callback);
		}
	}
}
