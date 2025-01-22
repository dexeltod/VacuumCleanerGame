using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.LinkerGenerator
{
	internal static class EnumerableExtensions
	{
		public static IEnumerable<TItem> Concat<TItem>(this IEnumerable<TItem> enumerable, TItem item) => enumerable.Concat(
			new[]
			{
				item
			}
		);

		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (T item in enumerable)
				action(
					item
				);
		}
	}
}