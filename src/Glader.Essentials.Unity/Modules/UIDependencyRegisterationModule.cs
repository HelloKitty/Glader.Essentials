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
	public class UIDependencyRegisterationModule<TKeyType> : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder register)
		{
			if(typeof(TKeyType) == typeof(int))
			{
				//Because of active load scene, we have to iterate each scene
				foreach(var registerable in new SceneUiElementEnumerable())
				{
					//Registers the adapter with the specified Key and Service Type.
					register.RegisterInstance(registerable)
						.SingleInstance()
						.Keyed(registerable.RegisterationKey, registerable.UIServiceType);
				}
			}
			else
			{
				//Because of active load scene, we have to iterate each scene
				foreach(var registerable in new SceneUiElementEnumerable())
				{
					//Registers the adapter with the specified Key and Service Type.
					register.RegisterInstance(registerable)
						.SingleInstance()
						.Keyed(GenericMath.Convert<int, TKeyType>(registerable.RegisterationKey), registerable.UIServiceType);
				}
			}
		}
	}
	
	/// <summary>
	/// Default int-keyed implementation of <see cref="UIDependencyRegisterationModule{TKeyType}"/>.
	/// </summary>
	public sealed class UIDependencyRegisterationModule : UIDependencyRegisterationModule<int>
	{
		
	}
}
