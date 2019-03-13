using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using SceneJect.Common;
using UnityEngine.SceneManagement;

namespace Glader.Essentials
{
	public sealed class UIDependencyRegisterationModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder register)
		{
			//Because of active load scene, we have to iterate each scene
			foreach(var registerable in new SceneUiElementEnumerable())
			{
				//Registers the adapter with the specified Key and Service Type.
				register.RegisterInstance(registerable)
					.SingleInstance()
					.Keyed(registerable.RegisterationKey, registerable.UISerivdeType);
			}
		}
	}
}
