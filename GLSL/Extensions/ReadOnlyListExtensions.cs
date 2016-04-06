using System;
using System.Collections.Generic;

namespace Xannden.GLSL.Extensions
{
	public static class ReadOnlyListExtensions
	{
		public static T Find<T>(this IReadOnlyList<T> list, Predicate<T> predicate)
		{
			if (list == null)
			{
				throw new ArgumentNullException(nameof(list));
			}

			for (int i = 0; i < (list?.Count ?? 0); i++)
			{
				if (predicate?.Invoke(list[i]) ?? false)
				{
					return list[i];
				}
			}

			return default(T);
		}

		public static bool Contains<T>(this IReadOnlyList<T> list, Predicate<T> predicate)
		{
			if (list == null)
			{
				throw new ArgumentNullException(nameof(list));
			}

			for (int i = 0; i < (list?.Count ?? 0); i++)
			{
				if (predicate?.Invoke(list[i]) ?? false)
				{
					return true;
				}
			}

			return false;
		}
	}
}
