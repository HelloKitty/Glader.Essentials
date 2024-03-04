using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace Glader.Essentials
{
	/// <summary>
	/// Base type for managed response models.
	/// </summary>
	/// <typeparam name="TModelType">The response model type.</typeparam>
	/// <typeparam name="TResponseCodeType">The response code type.</typeparam>
	[JsonObject]
	public class ResponseModel<TModelType, TResponseCodeType> : IResponseModel<TResponseCodeType>, ISucceedable
		where TResponseCodeType : Enum
	{
		/// <inheritdoc />
		[JsonProperty]
		public TResponseCodeType ResultCode { get; private set; }

		//This assumes that success is ALWAYS equal to 1.
		/// <inheritdoc />
		[JsonIgnore]
		public bool isSuccessful => ConvertResponseCodeToInt() == GladerEssentialsModelConstants.RESPONSE_CODE_SUCCESS_VALUE;

		/// <summary>
		/// The result object if <see cref="isSuccessful"/> is true.
		/// Can be null/default.
		/// </summary>
		[JsonProperty]
		public TModelType Result { get; private set; }

		/// <summary>
		/// Creates a failed <see cref="ResponseModel{TModelType,TResponseCodeType}"/> with the specified
		/// response code <see cref="ResultCode"/>
		/// </summary>
		/// <param name="resultCode">The non-Success response code.</param>
		public ResponseModel(TResponseCodeType resultCode)
		{
			//order is this way for conversion purposes
			ResultCode = resultCode;
			if(!Enum.IsDefined(typeof(TResponseCodeType), resultCode)) throw new InvalidEnumArgumentException(nameof(resultCode), ConvertResponseCodeToInt(), typeof(TResponseCodeType));

			//This is the failure ctor so we also check if it's successful
			if(isSuccessful)
				throw new ArgumentException($"Cannot initialize {nameof(resultCode)} with {resultCode}/Success when creating a failure response model", nameof(resultCode));
		}

		/// <summary>
		/// Creates a successful <see cref="ResponseModel{TModelType,TResponseCodeType}"/> with the specified <see cref="Result"/>.
		/// <see cref="isSuccessful"/> will be true and the <see cref="ResultCode"/> will match success.
		/// </summary>
		/// <param name="result"></param>
		public ResponseModel(TModelType result)
		{
			Result = result;

			//TODO: This won't work on AOT platforms.
			ResultCode = Generic.Math.GenericMath.Convert<int, TResponseCodeType>(GladerEssentialsModelConstants.RESPONSE_CODE_SUCCESS_VALUE);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int ConvertResponseCodeToInt()
		{
			//TODO: This won't work on AOT platforms.
			return Generic.Math.GenericMath.Convert<TResponseCodeType, int>(ResultCode);
		}

		/// <summary>
		/// Internal serializable ctor.
		/// </summary>
		[JsonConstructor]
		public ResponseModel()
		{
			//Validates assumptions about the state of a response model.
			if(isSuccessful && Result == null)
				throw new InvalidOperationException($"Received successful {typeof(TModelType).Name} Response Model BUT the {nameof(Result)} field was null.");
		}
	}
}