using NUnit.Framework;

namespace Glader.Essentials
{
	public enum TestObjectType
	{

	}

	public class ObjectGuidTests
	{
		[SetUp]
		public void Setup()
		{

		}

		[Test]
		public void Test_Empty_Guid_Value_0()
		{
			Assert.AreEqual(0, ObjectGuid<TestObjectType>.Empty.RawValue);
		}

		[Test]
		[TestCase((ulong)0)]
		[TestCase(ulong.MaxValue)]
		[TestCase(ulong.MinValue)]
		public void Test_Raw_Value_Matches_Initialization(ulong value)
		{
			Assert.AreEqual(value, new ObjectGuid<TestObjectType>(value).RawValue);
		}

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(int.MaxValue & 0xFFFFFF)]
		public void Test_Identifier_Value_Matches(int value)
		{
			var guid = new ObjectGuid<TestObjectType>(0);
			guid.SetIdentifier(value);

			Assert.AreEqual(value, guid.Identifier);
			Assert.AreEqual(0, guid.ShardIdentifier);
			Assert.AreEqual(0, guid.Entry);
			Assert.AreEqual(0, (int)guid.ObjectType);
		}

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(4)]
		[TestCase(15)] //max shard id
		public void Test_ShardId_Value_Matches(int value)
		{
			var guid = new ObjectGuid<TestObjectType>(0);
			guid.SetShard(value);

			Assert.AreEqual(value, guid.ShardIdentifier);
			Assert.AreEqual(0, guid.Identifier);
			Assert.AreEqual(0, guid.Entry);
			Assert.AreEqual(0, (int)guid.ObjectType);
		}

		[Test]
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(int.MaxValue & 0xFFFFFF)]
		public void Test_Entry_Value_Matches(int value)
		{
			var guid = new ObjectGuid<TestObjectType>(0);
			guid.SetEntry(value);

			Assert.AreEqual(value, guid.Entry);
			Assert.AreEqual(0, guid.Identifier);
			Assert.AreEqual(0, guid.ShardIdentifier);
			Assert.AreEqual(0, (int)guid.ObjectType);
		}

		[Test]
		[TestCase(int.MaxValue & 0xFFFFFF, 15, 4000, ushort.MaxValue)]
		public void Test_All_Value_Matches(int id, int shardid, int objecttype, int entry)
		{
			var guid = new ObjectGuid<TestObjectType>(0);
			guid.SetEntry(entry);
			guid.SetObjectType((TestObjectType) objecttype);
			guid.SetIdentifier(id);
			guid.SetShard(shardid);

			Assert.AreEqual(id, guid.Identifier);
			Assert.AreEqual(shardid, guid.ShardIdentifier);
			Assert.AreEqual(objecttype, (int)guid.ObjectType);
			Assert.AreEqual(entry, guid.Entry);
		}
	}
}