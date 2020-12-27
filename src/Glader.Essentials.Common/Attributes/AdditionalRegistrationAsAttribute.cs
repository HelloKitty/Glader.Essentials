using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	/// <summary>
	/// Indicates to a reflection API that is interested in dependency injection/IoC that a annotated
	/// type implements a specified service interface/type and can be registered as this
	/// under cases determined by the observer of this annotation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class AdditionalRegistrationAsAttribute : Attribute
	{
		/// <summary>
		/// The service type to register the marked type as.
		/// </summary>
		public Type ServiceType { get; }

		/// <summary>
		/// The service type that the targeted class implements and can be registered in an
		/// IoC container as.
		/// </summary>
		/// <param name="serviceType">The service type.</param>
		public AdditionalRegistrationAsAttribute(Type serviceType)
		{
			ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
		}
	}
}