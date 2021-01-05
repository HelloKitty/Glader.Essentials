using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Glader.Essentials
{
	/// <summary>
	/// <see cref="BaseGuid"/> implementation that defines a specific <typeparamref name="TEntityType"/>.
	/// </summary>
	/// <typeparam name="TEntityType"></typeparam>
	public class ObjectGuid<TEntityType> : BaseGuid, IEquatable<ObjectGuid<TEntityType>>
		where TEntityType : Enum
	{
		/// <summary>
		/// Represents an Empty or uninitialized <see cref="ObjectGuid{TEntityType}"/>.
		/// </summary>
		[IgnoreDataMember]
		public static ObjectGuid<TEntityType> Empty { get; } = new ObjectGuid<TEntityType>(0);

		//DO NOT REMOVE!
		static ObjectGuid()
		{
			
		}

		/// <summary>
		/// Indicates the object Type that the <see cref="ObjectGuid{TEntityType}"/> is associated with.
		/// </summary>
		[IgnoreDataMember]
		public TEntityType ObjectType => CalculateObjectType();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private TEntityType CalculateObjectType()
		{
			//WoW would be 0x0000FFFF but we trim this down abit.
			ulong value = (RawValue >> 48) & 0x00000FFF;
			return Unsafe.As<ulong, TEntityType>(ref value);
		}

		/// <summary>
		/// Indicates if the Entity is of the provided object type.
		/// </summary>
		/// <param name="objectType">The object type to check.</param>
		/// <returns>True if they're matching object types.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsType(TEntityType objectType)
		{
			return EqualityComparer<TEntityType>.Default.Equals(ObjectType, objectType);
		}

		/// <summary>
		/// Creates a new value-type wrapped for the uint64 raw GUID value.
		/// </summary>
		/// <param name="guidValue">Raw GUID value.</param>
		public ObjectGuid(ulong guidValue)
		{
			RawValue = guidValue;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		public ObjectGuid()
			: this(0)
		{

		}

		/// <summary>
		/// Sets the <see cref="ObjectType"/> value.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void SetObjectType(TEntityType type)
		{
			ulong value = Unsafe.As<TEntityType, ulong>(ref type);

			//ulong value = (RawValue >> 48) & 0x00000FFF;
			RawValue |= (value & 0x00000FFF) << 48;
		}

		/// <summary>
		/// Implict cast to ulong (uint64 TC/C++)
		/// </summary>
		/// <param name="guid"></param>
		public static implicit operator ulong(ObjectGuid<TEntityType> guid)
		{
			return guid.RawValue;
		}

		/// <summary>
		/// Implict cast to ulong (uint64 TC/C++)
		/// </summary>
		/// <param name="guid"></param>
		public static implicit operator ObjectGuid<TEntityType>(ulong guid)
		{
			if (guid == 0)
				return Empty;

			return new ObjectGuid<TEntityType>(guid);
		}

		public bool Equals(ObjectGuid<TEntityType> other)
		{
			if(ReferenceEquals(null, other)) return false;
			if(ReferenceEquals(this, other)) return true;
			return base.Equals(other) && RawValue == other.RawValue;
		}

		public static bool operator ==(ObjectGuid<TEntityType> lhs, ObjectGuid<TEntityType> rhs)
		{
			// Check for null on left side.
			if(Object.ReferenceEquals(lhs, null))
			{
				if(Object.ReferenceEquals(rhs, null))
				{
					// null == null = true.
					return true;
				}

				// Only the left side is null.
				return false;
			}


			// Equals handles case of null on right side.
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ObjectGuid<TEntityType> lhs, ObjectGuid<TEntityType> rhs)
		{
			return !(lhs == rhs);
		}

		public sealed override string ToString()
		{
			return $"Type: {ObjectType} {base.ToString()}";
		}

		public sealed override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public sealed override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
