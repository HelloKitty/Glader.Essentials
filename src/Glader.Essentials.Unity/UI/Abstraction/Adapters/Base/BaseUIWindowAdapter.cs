using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Glader.Essentials
{
	/// <summary>
	/// Base-type for <see cref="IUIWindow"/> adapters.
	/// </summary>
	/// <typeparam name="TWindowType">The window type.</typeparam>
	public abstract class BaseUIWindowAdapter<TWindowType> : BaseRegisterableUnityUI<TWindowType>, IUIWindow
		where TWindowType : IUIWindow
	{
		/// <inheritdoc />
		public override bool AsKeyed { get; } = false;

		[Button]
		public void TryLocateFrames()
		{
			foreach (FieldInfo fi in GetType()
				.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (fi.GetCustomAttribute<SerializeField>() == null)
					continue;

				if (!typeof(IUIFrame).IsAssignableFrom(fi.FieldType))
					continue;

				// Don't set fields that aren't null.
				if (fi.GetValue(this) != null)
					continue;

				//Because of active load scene, we have to iterate each scene
				foreach(var go in SceneManager.GetSceneByBuildIndex(gameObject.scene.buildIndex).GetRootGameObjects())
				{
					foreach(var registerable in go.GetComponentsInChildren<GladerBehaviour>(true)
						.Where(m => fi.FieldType.IsAssignableFrom(m.GetType())))
					{
						fi.SetValue(this, registerable);
						break;
					}
				}
			}
		}
	}
}
