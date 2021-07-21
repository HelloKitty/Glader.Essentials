using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Glader.Essentials
{
	//Based on: https://github.com/mxjones/RedBus/blob/master/src/Redbus/Redbus.Tests/EventBusTests.cs
	[TestClass]
	public class EventBusTests
	{
		private bool _methodHandlerHit;
		private bool _actionHandlerHit;

		[TestInitialize]
		public void Initialize()
		{
			_methodHandlerHit = false;
			_actionHandlerHit = false;
		}

		[TestMethod]
		public void SubscribeAndPublishCustomEventMethodTest()
		{
			var eventBus = new EventBus();
			eventBus.Subscribe<CustomTestEvent>(CustomTestEventMethodHandler);

			Assert.IsFalse(_methodHandlerHit);
			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Event", Identifier = 1 });
			Assert.IsTrue(_methodHandlerHit);
		}

		[TestMethod]
		public void SubscribeAndPublishCustomEventActionTest()
		{
			var eventBus = new EventBus();
			eventBus.Subscribe<CustomTestEvent>((obj, s) =>
			{
				Assert.AreEqual("Custom Event 2", s.Name);
				Assert.AreEqual(2, s.Identifier);

				_actionHandlerHit = true;
			});

			Assert.IsFalse(_actionHandlerHit);
			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Event 2", Identifier = 2 });
			Assert.IsTrue(_actionHandlerHit);
		}

		[TestMethod]
		public void SubscribeAndPublishBuiltInEventActionTest()
		{
			var eventBus = new EventBus();
			eventBus.Subscribe<BasicEventBusEventArgs<int>>((obj, s) =>
			{
				Assert.AreEqual(999, s.Payload);
				_actionHandlerHit = true;
			});

			Assert.IsFalse(_actionHandlerHit);
			eventBus.Publish(this, new BasicEventBusEventArgs<int>(999));
			Assert.IsTrue(_actionHandlerHit);
		}

		[TestMethod]
		public void PublishInCorrectOrderTest()
		{
			var eventBus = new EventBus();

			List<CustomTestEvent> customTestEventResults = new List<CustomTestEvent>();
			eventBus.Subscribe<CustomTestEvent>((obj, s) =>
			{
				customTestEventResults.Add(s);
			});

			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Test Event", Identifier = 1 });
			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Test Event", Identifier = 2 });
			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Test Event", Identifier = 3 });
			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Test Event", Identifier = 4 });
			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Test Event", Identifier = 5 });
			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Test Event", Identifier = 6 });

			Assert.AreEqual(6, customTestEventResults.Count);
			Assert.AreEqual(1, customTestEventResults[0].Identifier);
			Assert.AreEqual(2, customTestEventResults[1].Identifier);
			Assert.AreEqual(3, customTestEventResults[2].Identifier);
			Assert.AreEqual(4, customTestEventResults[3].Identifier);
			Assert.AreEqual(5, customTestEventResults[4].Identifier);
			Assert.AreEqual(6, customTestEventResults[5].Identifier);
		}

		[TestMethod]
		public void UnsubscribeTest()
		{
			var eventBus = new EventBus();
			var token = eventBus.Subscribe<CustomTestEvent>((obj, s) =>
			{
				Assert.Fail("This should not be executed due to unsubscribing.");
			});

			eventBus.Unsubscribe(token);
			eventBus.Publish(this, new CustomTestEvent { Name = "Custom Event 3", Identifier = 3 });
		}

		[TestMethod]
		public void UnsubscribeDontThrowIfDoesntExistTest()
		{
			var eventBus = new EventBus();
			var token = eventBus.Subscribe<CustomTestEvent>((obj, s) =>
			{
				Assert.Fail("This should not be executed due to unsubscribing.");
			});

			eventBus.Unsubscribe(token);
			eventBus.Unsubscribe(token);
			eventBus.Unsubscribe(token);
			eventBus.Unsubscribe(token);
		}

		[TestMethod]
		public void PublishExceptionEventOnExceptionTest()
		{
			var eventBus = new EventBus();
			bool firstSubscriberHit = false, thirdSubscriberHit = false, fourthSubscriberHit = false;
			eventBus.Subscribe<CustomTestEvent>((obj, s) => { firstSubscriberHit = true; });
			eventBus.Subscribe<CustomTestEvent>((obj, s) => throw new ApplicationException($"Subscriber error"));

			//Should be fired when above event subscription throws.
			eventBus.SubscribeException((sender, args) => { thirdSubscriberHit = true; });
			eventBus.Subscribe<CustomTestEvent>((obj, s) => { fourthSubscriberHit = true; });

			eventBus.Publish(this, new CustomTestEvent());
			Assert.IsTrue(firstSubscriberHit);
			Assert.IsTrue(thirdSubscriberHit);
			Assert.IsTrue(fourthSubscriberHit);
		}

		[TestMethod]
		public void PublishThrowsIfExceptionHandlerThrows()
		{
			var eventBus = new EventBus();
			bool firstSubscriberHit = false, thirdSubscriberHit = false;
			eventBus.Subscribe<CustomTestEvent>((obj, s) => { firstSubscriberHit = true; });
			eventBus.Subscribe<CustomTestEvent>((obj, s) => throw new ApplicationException($"Subscriber error"));
			eventBus.Subscribe<CustomTestEvent>((obj, s) => { thirdSubscriberHit = true; });

			eventBus.SubscribeException((sender, args) => throw args.ExceptionData);

			var thrownException = Assert.ThrowsException<ApplicationException>(() => eventBus.Publish(this, new CustomTestEvent())); // Subscriber exception is thrown
			Assert.AreEqual("Subscriber error", thrownException.Message); // Verify correct message from subscriber
			Assert.IsTrue(firstSubscriberHit); // The first subscriber will be hit
			Assert.IsFalse(thirdSubscriberHit); // Third subscriber will not be hit, missed due to thrown exception.
		}

		[TestMethod]
		public void PublishThrowSubscriberExceptionTest()
		{
			var eventBus = new EventBus();
			bool firstSubscriberHit = false, thirdSubscriberHit = false;
			eventBus.Subscribe<CustomTestEvent>((obj, s) => { firstSubscriberHit = true; });
			eventBus.Subscribe<CustomTestEvent>((obj, s) => throw new ApplicationException($"Subscriber error"));
			eventBus.Subscribe<CustomTestEvent>((obj, s) => { thirdSubscriberHit = true; });

			var thrownException = Assert.ThrowsException<ApplicationException>(() => eventBus.Publish(this, new CustomTestEvent())); // Subscriber exception is thrown
			Assert.AreEqual("Subscriber error", thrownException.Message); // Verify correct message from subscriber
			Assert.IsTrue(firstSubscriberHit); // The first subscriber will be hit
			Assert.IsFalse(thirdSubscriberHit); // Third subscriber will not be hit, missed due to thrown exception.
		}

		private void CustomTestEventMethodHandler(object sender, CustomTestEvent customTestEvent)
		{
			Assert.AreEqual("Custom Event", customTestEvent.Name);
			Assert.AreEqual(1, customTestEvent.Identifier);
			_methodHandlerHit = true;
		}
	}

	internal class CustomTestEvent : IEventBusEventArgs
	{
		public string Name { get; set; }
		public int Identifier { get; set; }
	}
}