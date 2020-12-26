using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for DB models that are simple keyable types.
	/// </summary>
	public interface IDatabaseModelKeyable<out TKeyType>
	{
		/// <summary>
		/// The key to the database model entry.
		/// </summary>
		[NotMapped]
		TKeyType EntryKey { get; }
	}

	/// <summary>
	/// Contract for DB models that are simple keyable types.
	/// </summary>
	public interface IDatabaseModelKeyable : IDatabaseModelKeyable<int>
	{

	}
}
