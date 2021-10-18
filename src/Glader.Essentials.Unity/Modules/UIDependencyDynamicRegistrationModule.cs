using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Generic.Math;
using SceneJect.Common;
using UnityEngine.SceneManagement;

namespace Glader.Essentials
{
	/// <summary>
	/// AutoFac service <see cref="Module"/> that registers all UI adapters located in the scene.
	/// </summary>
	public class UIDependencyDynamicRegistrationModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder register)
		{
			//Because of active load scene, we have to iterate each scene
			foreach(var registerable in new SceneUiElementEnumerable())
			{
				//Registers the adapter with the specified Key and Service Type.
				if (registerable.AsKeyed)
				{
					register.RegisterInstance(registerable)
						.SingleInstance()
						.Keyed(registerable.RegistrationKey, registerable.UIServiceType);
				}
				else
				{
					register.RegisterInstance(registerable)
						.SingleInstance()
						.As(registerable.UIServiceType);
				}
			}
		}
	}
}
