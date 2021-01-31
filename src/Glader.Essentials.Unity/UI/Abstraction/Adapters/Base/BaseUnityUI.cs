using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unitysync.Async;

namespace Glader.Essentials
{
	/// <summary>
	/// <see cref="MonoBehaviour"/> implementation of <see cref="IUIAdapterRegisterable"/>
	/// </summary>
	public abstract class BaseUnityUI : MonoBehaviour, IUIAdapterRegisterable
	{
		//Assigned by Unity3D.
		/// <summary>
		/// Internally Unity3D serialized key.
		/// </summary>
		[Tooltip("Used to determine wiring for UI dependencies.")]
		[SerializeField]
#pragma warning disable 649
		private string _RegistrationKey;
#pragma warning restore 649

		/// <inheritdoc />
		public string RegistrationKey => _RegistrationKey;

		/// <inheritdoc />
		public abstract Type UIServiceType { get; }
	}

	/// <summary>
	/// <see cref="BaseUnityUI"/> that adapts the specified type <typeparamref name="TAdaptedToType"/>.
	/// </summary>
	/// <typeparam name="TAdaptedToType"></typeparam>
	public abstract class BaseUnityUI<TAdaptedToType> : BaseUnityUI
	{
		/// <inheritdoc />
		public override Type UIServiceType => typeof(TAdaptedToType);

		//TODO: Eventually we need to refactor this away.
		/// <summary>
		/// Can be called as a <see cref="StartCoroutine"/>
		/// to track the result, and dispatch the exception/logging for, async tasks.
		/// </summary>
		/// <param name="task">The task to await.</param>
		/// <returns></returns>
		protected IEnumerator AsyncCallbackHandler(Task task)
		{
			if(task == null) throw new ArgumentNullException(nameof(task));

			//This will wait until the task is complete
			yield return new WaitForFuture(task);

			if(task.IsFaulted)
			{
				StringBuilder builder = new StringBuilder(200);

				if(task.Exception != null && task.Exception.InnerExceptions != null)
					foreach(Exception inner in task.Exception?.InnerExceptions)
						builder.Append($"\nMessage: {inner.Message}\nStack: {inner.StackTrace}");

				UnityEngine.Debug.LogError($"Encounter exception from Button: {name} OnClickAsync: {builder}");
			}

			//We don't need to do anything, task succeeded and is finished.
		}
	}
}
