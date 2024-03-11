using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Glader.Essentials;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Glader.Essentials
{
	public static class UnityWebRequestHelpers
	{
		private static class XmlSerializerHelper<T>
		{
			public static XmlSerializer TypeSerializer { get; } = new XmlSerializer(typeof(T));
		}

		// TODO: Doc
		/// <summary>
		/// Requests bytes from the provided <see cref="url"/> with the specified args.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="url"></param>
		/// <param name="requestConfigAction"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Task<byte[]> RequestBytes(UnityHttpRequestType type, string url,
			Action<UnityWebRequest> requestConfigAction = null, string data = null)
		{
			var request = UnityWebRequestInitializerHelpers.CreateRequestWithCallbackToken<byte[]>(type, url, data, out var token);
			requestConfigAction?.Invoke(request);

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

		/// <summary>
		/// Requests XML deserialized result from the provided <see cref="url"/> with the specified args.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="url"></param>
		/// <param name="requestConfigAction"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Task<TResponseBodyType> RequestXml<TResponseBodyType>(UnityHttpRequestType type, string url,
			Action<UnityWebRequest> requestConfigAction = null, string data = null)
		{
			var request = UnityWebRequestInitializerHelpers.CreateRequestWithCallbackToken<TResponseBodyType>(type, url, data, out var token);
			requestConfigAction?.Invoke(request);

			request.SendWebRequest().completed += operation =>
			{
				if(request.result == UnityWebRequest.Result.Success)
				{
					using(var stream = new StringReader(request.downloadHandler.text))
						token.SetResult((TResponseBodyType)XmlSerializerHelper<TResponseBodyType>.TypeSerializer.Deserialize(stream));
				}
				else
				{
					token.SetException(new InvalidOperationException($"Failed to handle request at Endpoint: {url} Result: {request.result} Error: {request.error}"));
				}
			};

			return token.Task;
		}

		/// <summary>
		/// Requests JSON deserialized result from the provided <see cref="url"/> with the specified args.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="url"></param>
		/// <param name="requestConfigAction"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Task<TResponseBodyType> RequestJson<TResponseBodyType>(UnityHttpRequestType type, string url,
			Action<UnityWebRequest> requestConfigAction = null, string data = null)
		{
			var request = UnityWebRequestInitializerHelpers.CreateRequestWithCallbackToken<TResponseBodyType>(type, url, data, out var token);
			requestConfigAction?.Invoke(request);

			request.SendWebRequest().completed += operation =>
			{
				if(request.result == UnityWebRequest.Result.Success)
				{
					token.SetResult(JsonConvert.DeserializeObject<TResponseBodyType>(request.downloadHandler.text));
				}
				else
				{
					token.SetException(new InvalidOperationException($"Failed to handle request at Endpoint: {url} Result: {request.result} Error: {request.error}"));
				}
			};

			return token.Task;
		}

		/// <summary>
		/// Requests the string result from the provided <see cref="url"/> with the specified args.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="url"></param>
		/// <param name="requestConfigAction"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Task<string> RequestString(UnityHttpRequestType type, string url, 
			Action<UnityWebRequest> requestConfigAction = null, string data = null)
		{
			var request = UnityWebRequestInitializerHelpers.CreateRequestWithCallbackToken<string>(type, url, data, out var token);
			requestConfigAction?.Invoke(request);

			request.SendWebRequest().completed += operation =>
			{
				if(request.result == UnityWebRequest.Result.Success)
				{
					token.SetResult(request.downloadHandler.text);
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
