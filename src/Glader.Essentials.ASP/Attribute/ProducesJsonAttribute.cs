using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Glader.Essentials
{
	/// <summary>
	/// Marks a controller/action indicating it should produce JSON
	/// content.
	/// </summary>
	public sealed class ProducesJsonAttribute : ProducesAttribute
	{
		/// <summary>
		/// The content-type used by the attribute.
		/// </summary>
		public static string JsonContentType { get; } = "application/json";

		/// <inheritdoc />
		public ProducesJsonAttribute() 
			: base(JsonContentType)
		{

		}
	}
}
