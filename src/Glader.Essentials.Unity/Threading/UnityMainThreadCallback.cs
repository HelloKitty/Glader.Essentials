using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace Glader.Essentials
{
	/// <summary>
	/// Type for continue callbacks on the main Unity3D thread.
	/// </summary>
	public static class UnityMainThreadCallback
	{
		/// <summary>
		/// Creates a new <see cref="UnityMainThreadCallback{T}"/> wrapping the provided <see cref="action"/>.
		/// </summary>
		/// <typeparam name="T">The parameter type.</typeparam>
		/// <param name="action">The action type.</param>
		/// <returns>Wrapped action that can convert to an <see cref="Action{T}"/>.</returns>
		public static UnityMainThreadCallback<T> Create<T>([NotNull] Action<T> action)
		{
			if(action == null) throw new ArgumentNullException(nameof(action));
			return new UnityMainThreadCallback<T>(action);
		}
	}

	/// <summary>
	/// Type for continue callbacks on the main Unity3D thread.
	/// </summary>
	public sealed class UnityMainThreadCallback<T>
	{
		private Action<T> WrappedAction { get; }

		/// <summary>
		/// Creates a new <see cref="UnityMainThreadCallback{T}"/> that will continue on the Unity3D main thread when invoked.
		/// </summary>
		/// <param name="wrappedAction"></param>
		public UnityMainThreadCallback([NotNull] Action<T> wrappedAction)
		{
			WrappedAction = wrappedAction ?? throw new ArgumentNullException(nameof(wrappedAction));
		}

		/// <summary>
		/// Casts the provided <see cref="UnityMainThreadCallback{T}"/> to an <see cref="Action{T}"/>.
		/// </summary>
		/// <param name="c">The callback object.</param>
		/// <returns>Wrapped callback that will invoke on the main thread.</returns>
		public static implicit operator Action<T>(UnityMainThreadCallback<T> c)
		{
			return obj =>
			{
				UnityAsyncHelper.UnityMainThreadContext.Post(state =>
				{
					c.WrappedAction(obj);
				}, null);
			};
		}
	}
}
