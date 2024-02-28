using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Glader.Essentials
{
	public static class BaseGuidMutator
	{
		/// <summary>
		/// Mutates/initializes the internal value of the provided <see cref="guid"/>.
		/// </summary>
		/// <param name="guid">The guid to initialize.</param>
		/// <param name="value">The value.</param>
		public static void SetValue(BaseGuid guid, ulong value)
		{
			if (guid == null)
				return;

			guid.RawValue = value;
		}
	}

	/// <summary>
	/// Base non-generic guid type.
	/// </summary>
	public abstract class BaseGuid : IEquatable<BaseGuid>
	{
		// Made protected internal so that you can set it for serialization purposes in derived types.
		/// <summary>
		/// Internal GUID value.
		/// </summary>
		[DataMember(Order = 1, IsRequired = true)]
		public ulong RawValue { get; protected internal set; } //setter only for serialization

		/// <summary>
		/// Indicates the current GUID of the object. This is the last chunk represents the id that the world server assigned to the object. (The rest is just maskable flags about the object)
		/// </summary>
		[IgnoreDataMember]
		public int Identifier => CanHaveEntry() ? (int)(RawValue & 0x0000000000FFFFFF) : (int)(RawValue & 0x00000000FFFFFFFF); // Use an extra byte if it can't have an entry

		/// <summary>
		/// Indicates the templated reference identifier (entry)
		/// for the Entity.
		/// </summary>
		[IgnoreDataMember]
		public int Entry => CanHaveEntry() ? (int) (RawValue >> 24) & 0x0000000000FFFFFF : 0;

		//TODO: This only support 15 shards.
		/// <summary>
		/// Represents the unique bits related to shard/server identifier.
		/// Meaning an object attached to one shard may be globally unique against one, similar or identical,
		/// to another except for these top-most bits.
		/// </summary>
		[IgnoreDataMember]
		public byte ShardIdentifier => (byte) ((RawValue >> 60) & 0xF);

		/// <summary>
		/// Indicates if the entity has an entry.
		/// </summary>
		[IgnoreDataMember]
		public bool HasEntry => CanHaveEntry() && Entry != 0;

		/// <summary>
		/// Indicates if the guid CAN have an entry.
		/// If it cannot have an entry the bytes/bits are reclaimed for the <see cref="Identifier"/>.
		/// </summary>
		/// <returns></returns>
		protected virtual bool CanHaveEntry()
		{
			return true;
		}

		/// <summary>
		/// Indicates if the GUID is an empty or uninitialized GUID.
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return RawValue == 0;
		}

		/// <inheritdoc />
		public bool Equals(BaseGuid other)
		{
			if (other == null)
				return false;
			else
				return other.RawValue == this.RawValue;
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
			return RawValue.GetHashCode();
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{RawValue} 0x{RawValue:X} Entry: {Entry} Id: {Identifier}";
		}

		/// <summary>
		/// Sets the <see cref="Entry"/> value.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected internal void SetEntry(int entry)
		{
			if (!CanHaveEntry())
				return;

			//(RawGuidValue >> 24) & 0x0000000000FFFFFF
			RawValue |= ((ulong)(entry & 0x0000000000FFFFFF) << 24);
		}

		/// <summary>
		/// Sets the <see cref="Identifier"/> value.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected internal void SetIdentifier(int id)
		{
			if (CanHaveEntry())
				RawValue |= (ulong)id & 0x0000000000FFFFFF;
			else
				RawValue |= (ulong)id & 0x00000000FFFFFFFF;
		}

		/// <summary>
		/// Sets the <see cref="ShardIdentifier"/> value.
		/// </summary>
		protected internal void SetShard(int shardId)
		{
			//ShardIdentifier => (byte) ((RawValue >> 63) & 0xF);
			RawValue |= (ulong)(shardId & 0xF) << 60;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		protected BaseGuid()
		{

		}
	}
}
