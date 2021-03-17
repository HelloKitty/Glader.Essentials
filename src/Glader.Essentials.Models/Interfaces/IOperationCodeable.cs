using System;
using System.Collections.Generic;
using System.Text;

namespace Glader.Essentials
{
	/// <summary>
	/// Contract for types that can be mapped to an <typeparamref name="TOperationCodeType"/> code.
	/// </summary>
	/// <typeparam name="TOperationCodeType">The operation code type.</typeparam>
	public interface IOperationCodeable<out TOperationCodeType>
		where TOperationCodeType : Enum
	{
		/// <summary>
		/// The operation code.
		/// </summary>
		TOperationCodeType OperationCode { get; }
	}
}
