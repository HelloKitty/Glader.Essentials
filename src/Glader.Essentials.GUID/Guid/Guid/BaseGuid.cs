using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Glader.Essentials
{
	/// <summary>
	/// Base non-generic guid type.
	/// </summary>
	public abstract class BaseGuid : IEquatable<BaseGuid>
	{
		/// <summary>
		/// Internal GUID value.
		/// </summary>
		[DataMember(Order = 1, IsRequired = true)]
		[IgnoreDataMember]
		public ulong RawGuidValue { get; internal set; } //setter only for serialization

		/// <summary>
		/// Indicates the current GUID of the object. This is the last chunk represents the id that the world server assigned to the object. (The rest is just maskable flags about the object)
		/// </summary>
		[IgnoreDataMember]
		public int CurrentObjectGuid => (int)(RawGuidValue & 0x0000000000FFFFFF);

		/// <summary>
		/// Indicates the templated reference identifier (entry)
		/// for the Entity.
		/// </summary>
		[IgnoreDataMember]
		public int Entry => (int) (RawGuidValue >> 24) & 0x0000000000FFFFFF;

		//TODO: This only support 15 shards.
		/// <summary>
		/// Represents the unique bits related to shard/server identifier.
		/// Meaning an object attached to one shard may be globally unique against one, similar or identical,
		/// to another except for these top-most bits.
		/// </summary>
		[IgnoreDataMember]
		public byte ShardIdentifier => (byte) ((RawGuidValue >> 63) & 0xF);

		/// <summary>
		/// Indicates if the entity has an entry.
		/// </summary>
		[IgnoreDataMember]
		public bool HasEntry => Entry != 0;

		/// <summary>
		/// Indicates if the GUID is an empty or uninitialized GUID.
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return RawGuidValue == 0;
		}

		/// <inheritdoc />
		public bool Equals(BaseGuid other)
		{
			if (other == null)
				return false;
			else
				return other.RawGuidValue == this.RawGuidValue;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((BaseGuid) obj);
		}

		public static bool operator ==(BaseGuid lhs, BaseGuid rhs)
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

		public static bool operator !=(BaseGuid lhs, BaseGuid rhs)
		{
			return !(lhs == rhs);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			//We actually ONLY mutate it during deserialization.
			// ReSharper disable once NonReadonlyMemberInGetHashCode
			return RawGuidValue.GetHashCode();
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{RawGuidValue} 0x{RawGuidValue:X} Entry: {Entry} Id: {CurrentObjectGuid}";
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		protected BaseGuid()
		{

		}
	}
}
