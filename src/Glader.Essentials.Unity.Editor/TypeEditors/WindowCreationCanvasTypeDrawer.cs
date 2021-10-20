using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Glader.Essentials
{
	[CustomEditor(typeof(Canvas), true)]
	public class WindowCreationCanvasTypeDrawer : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if(GUILayout.Button("Create UIWindow"))
			{
				CreateWindowSource();
			}
		}

		private void CreateWindowSource()
		{
			throw new NotImplementedException();
			Canvas canvas = (Canvas) target;
			var scene = SceneManager.GetSceneByBuildIndex(canvas.gameObject.scene.buildIndex);

			foreach(var go in scene.GetRootGameObjects())
			{

			}
		}
	}
}
