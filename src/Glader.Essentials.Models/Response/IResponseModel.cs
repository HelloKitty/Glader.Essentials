using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	//From: https://github.com/HaloLive/HaloLive.Library/tree/master/src/HaloLive.Models.Common
	//The concept here is that response models should be related to a request.
	//The request should have an enumerable state of results that the response can be in.
	/// <summary>
	/// Contract for models that are responses to a request.
	/// </summary>
	public interface IResponseModel<out TResultType>
		where TResultType : Enum
	{
		/// <summary>
		/// Indicates the result of the response.
		/// </summary>
		TResultType ResultCode { get; }
	}
}