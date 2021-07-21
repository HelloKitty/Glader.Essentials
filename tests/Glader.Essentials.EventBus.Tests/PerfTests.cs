using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Glader.Essentials;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Redbus.Tests
{
	[TestClass]
	public class PerfTests
	{
		// Naive baseline to see changes for now
		[TestMethod]
		public void NaivePublishPerformanceTest()
		{
			var eventBus = new EventBus();
			eventBus.Subscribe<CustomTestEvent>(CustomTestEventHandler);

			var sw = Stopwatch.StartNew();
			for(int i = 0; i < 10000000; i++)
				eventBus.Publish(this, new CustomTestEvent { Name = "Custom Event", Identifier = 1 });
			sw.Stop();

			Debug.WriteLine($"Finished in {sw.ElapsedMilliseconds}ms");
			Assert.IsTrue(sw.Elapsed < TimeSpan.FromSeconds(1), $"NaivePublishPerformanceTest took {sw.ElapsedMilliseconds}ms");
			Console.WriteLine($"[DEBUG] NaivePublishPerformanceTest took {sw.ElapsedMilliseconds}ms");
		}

		[TestMethod]
		public void NaiveConcurrentPublishPerformanceTest()
		{
			var eventBus = new EventBus();
			eventBus.Subscribe<CustomTestEvent>(CustomTestEventHandler);

			var mainSw = Stopwatch.StartNew();
			var threads = new List<Thread>();
			for(int i = 0; i < 64; i++)
			{
				var thread = new Thread(() =>
				{
					var sw = Stopwatch.StartNew();
					for(int j = 0; j < 1000000; j++)
					{
						eventBus.Publish(this, new CustomTestEvent { Name = "Custom Event", Identifier = 1 });
					}
					sw.Stop();
					Assert.IsTrue(sw.Elapsed < TimeSpan.FromSeconds(1), $"NaivePublishPerformanceTest took {sw.ElapsedMilliseconds}ms");
				});

				threads.Add(thread);
			}

			foreach(var thread in threads)
			{
				thread.Start();
			}

			foreach(var thread in threads)
			{
				thread.Join();
			}
			mainSw.Stop();

			Debug.WriteLine($"Finished in {mainSw.ElapsedMilliseconds}ms");
			Assert.IsTrue(mainSw.Elapsed < TimeSpan.FromSeconds(2), $"NaivePublishPerformanceTest took {mainSw.ElapsedMilliseconds}ms");
			Console.WriteLine($"[DEBUG] NaivePublishPerformanceTest took {mainSw.ElapsedMilliseconds}ms");
		}

		[TestMethod]
		public void NaiveConcurrentSubUnsubPerformanceTest()
		{
			var eventBus = new EventBus();
			eventBus.Subscribe<CustomTestEvent>(CustomTestEventHandler);

			var mainSw = Stopwatch.StartNew();
			var threads = new List<Thread>();
			for(int i = 0; i < 64; i++)
			{
				var thread = new Thread(() =>
				{
					var sw = Stopwatch.StartNew();
					for(int j = 0; j < 10000; j++)
					{
						var token = eventBus.Subscribe<CustomTestEvent>(CustomTestEventHandler);
						eventBus.Publish(this, new CustomTestEvent { Name = "Custom Event", Identifier = 1 });
						eventBus.Unsubscribe(token);
					}
					sw.Stop();
					Assert.IsTrue(sw.Elapsed < TimeSpan.FromSeconds(1), $"NaivePublishPerformanceTest took {sw.ElapsedMilliseconds}ms");
				});

				threads.Add(thread);
			}

			foreach(var thread in threads)
			{
				thread.Start();
			}

			foreach(var thread in threads)
			{
				thread.Join();
			}
			mainSw.Stop();

			Debug.WriteLine($"Finished in {mainSw.ElapsedMilliseconds}ms");
			Assert.IsTrue(mainSw.Elapsed < TimeSpan.FromSeconds(2), $"NaivePublishPerformanceTest took {mainSw.ElapsedMilliseconds}ms");
			Console.WriteLine($"[DEBUG] NaivePublishPerformanceTest took {mainSw.ElapsedMilliseconds}ms");
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		private void CustomTestEventHandler(object sender, CustomTestEvent customTestEvent)
		{

		}
	}
}