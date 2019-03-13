using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Glader.Essentials
{
	/// <summary>
	/// Enumerator for all <see cref="IUIAdapterRegisterable"/> in a scene.
	/// </summary>
	public class SceneUiElementEnumerable : IEnumerable<IUIAdapterRegisterable>
	{
		public IEnumerator<IUIAdapterRegisterable> GetEnumerator()
		{
			//Because of active load scene, we have to iterate each scene
			for(int i = 0; i < SceneManager.sceneCount; i++)
				foreach(var go in SceneManager.GetSceneAt(i).GetRootGameObjects())
				{
					foreach(var registerable in go.GetComponentsInChildren<MonoBehaviour>(true)
						.Select(m => m as IUIAdapterRegisterable)
						.Where(m => m != null))
					{
						yield return registerable;
					}
				}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
