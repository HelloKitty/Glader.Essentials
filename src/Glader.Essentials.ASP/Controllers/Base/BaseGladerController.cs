using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Glader.Essentials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Glader.Essentials
{
	/// <summary>
	/// The base controller Type for Glader ASP MVC controllers.
	/// </summary>
	public abstract class BaseGladerController : Controller
	{
		/// <summary>
		/// The logging service for the controller.
		/// </summary>
		protected ILogger<BaseGladerController> Logger { get; }

		/// <inheritdoc />
		protected BaseGladerController([FromServices] ILogger<BaseGladerController> logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		//TODO: Fix doc
		/// <summary>
		/// Builds a successful JsonResult of ResponseModel
		/// with a successful result code an the value of result.
		/// </summary>
		/// <typeparam name="TModelType">The model type.</typeparam>
		/// <param name="result">The result.</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(JsonSuccess) + " instead.")]
		protected JsonResult BuildSuccessfulResponseModel<TModelType>(TModelType result)
			where TModelType : class
		{
			if (result == null) throw new ArgumentNullException(nameof(result));
			return Json(new ResponseModel<TModelType, StubbedResponseCode>(result)); //We use a stubbed response code since we don't actually known the result.
		}

		/// <summary>
		/// TODO: Fix doc, intellisense not working as I'm writing this.
		/// </summary>
		/// <typeparam name="TResponseCodeType">The response code type.</typeparam>
		/// <param name="failedResponseCode">The failure code.</param>
		/// <returns></returns>
		[Obsolete("Use " + nameof(JsonFailure) + " instead.")]
		protected JsonResult BuildFailedResponseModel<TResponseCodeType>(TResponseCodeType failedResponseCode)
			where TResponseCodeType : Enum
		{
			//TODO: add enum check.
			return Json(new ResponseModel<object, TResponseCodeType>(failedResponseCode)); //We use a stubbed response code since we don't actually known the result.
		}

		//TODO: Fix doc
		/// <summary>
		/// Builds a successful JsonResult of ResponseModel
		/// with a successful result code an the value of result.
		/// </summary>
		/// <typeparam name="TModelType">The model type.</typeparam>
		/// <param name="result">The result.</param>
		/// <returns></returns>
		protected JsonResult JsonSuccess<TModelType>(TModelType result)
			where TModelType : class
		{
			if(result == null) throw new ArgumentNullException(nameof(result));
			return Json(new ResponseModel<TModelType, StubbedResponseCode>(result)); //We use a stubbed response code since we don't actually known the result.
		}

		/// <summary>
		/// TODO: Fix doc, intellisense not working as I'm writing this.
		/// </summary>
		/// <typeparam name="TResponseCodeType">The response code type.</typeparam>
		/// <param name="failedResponseCode">The failure code.</param>
		/// <returns></returns>
		protected JsonResult JsonFailure<TResponseCodeType>(TResponseCodeType failedResponseCode)
			where TResponseCodeType : Enum
		{
			//TODO: add enum check.
			return Json(new ResponseModel<object, TResponseCodeType>(failedResponseCode)); //We use a stubbed response code since we don't actually known the result.
		}

		//TODO: Fix doc
		/// <summary>
		/// Builds a successful JsonResult of ResponseModel
		/// with a successful result code an the value of result.
		/// </summary>
		/// <typeparam name="TModelType">The model type.</typeparam>
		/// <typeparam name="TResponseCodeType">The response code type</typeparam>
		/// <param name="result">The result.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected ResponseModel<TModelType, TResponseCodeType> Success<TModelType, TResponseCodeType>(TModelType result)
			where TModelType : class
			where TResponseCodeType : Enum
		{
			if(result == null) throw new ArgumentNullException(nameof(result));
			return new ResponseModel<TModelType, TResponseCodeType>(result); //We use a stubbed response code since we don't actually known the result.
		}

		/// <summary>
		/// TODO: Fix doc, intellisense not working as I'm writing this.
		/// </summary>
		/// <typeparam name="TResponseCodeType">The response code type.</typeparam>
		/// <typeparam name="TModelType">The response model.</typeparam>
		/// <param name="failedResponseCode">The failure code.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected ResponseModel<TModelType, TResponseCodeType> Failure<TModelType, TResponseCodeType>(TResponseCodeType failedResponseCode)
			where TResponseCodeType : Enum
			where TModelType : class
		{
			//TODO: add enum check.
			return new ResponseModel<TModelType, TResponseCodeType>(failedResponseCode); //We use a stubbed response code since we don't actually known the result.
		}
	}
}
