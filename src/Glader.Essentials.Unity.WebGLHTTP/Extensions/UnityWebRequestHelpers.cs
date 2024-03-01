using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Glader.Essentials;
using UnityEngine.Networking;

namespace Glader.Essentials
{
	public static class UnityWebRequestHelpers
	{
		// TODO: Doc
		/// <summary>
		/// Requests bytes from the provided <see cref="url"/> with the specified args.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="url"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Task<byte[]> RequestBytes(UnityHttpRequestType type, string url, string data = null)
		{
			var request = UnityWebRequestInitializerHelpers.CreateRequestWithCallbackToken<byte[]>(type, url, data, out var token);

			request.SendWebRequest().completed += operation =>
			{
				if (request.result == UnityWebRequest.Result.Success)
				{
					token.SetResult(request.downloadHandler.data);
				}
				else
				{
					token.SetException(new InvalidOperationException($"Failed to handle request at Endpoint: {url} Result: {request.result} Error: {request.error}"));
				}
			};

			return token.Task;
		}
	}
}
