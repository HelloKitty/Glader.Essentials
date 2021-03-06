using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Glader.Essentials
{
	[Serializable]
	[DataContract]
	public class Vector3<T> : Vector2<T>
	{
		/// <summary>
		/// X value.
		/// </summary>
		[DataMember(Order = 3)]
		public T Z { get; internal set; }

		public Vector3(T x, T y, T z) 
			: base(x, y)
		{
			Z = z;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		public Vector3()
		{
			
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{base.ToString()} Z: {Z}";
		}
	}
}
