﻿using System;
using System.Collections.Generic;
using System.Text;
using Refit;

namespace Glader.Essentials
{
	/// <summary>
	/// Stub/Mock for old TypeSafe.Http.Net to help return to it
	/// in the future should a transition back occur.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class JsonBodyAttribute : BodyAttribute
	{
		public JsonBodyAttribute()
			: base(BodySerializationMethod.Serialized) //this doesn't do anything, but we'll set it anyway.
		{
			
		}
	}
}
