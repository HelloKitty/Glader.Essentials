using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Glader.Essentials
{
	public static class GladerEnumUtils
	{
		// This is INSANE but I guess it promises randomness for the below enum.
		private static int Seed = Environment.TickCount;

		private static ThreadLocal<Random> Random { get; } = new(() => new Random(Interlocked.Increment(ref Seed)));

		// See: https://stackoverflow.com/questions/319814/generate-random-enum-in-c-sharp-2-0
		public static T SelectRandomEnumValue<T>()
			where T : Enum
		{
			T[] values = (T[])Enum.GetValues(typeof(T));
			return values[Random.Value.Next(0, values.Length)];
		}
	}
}
