using System;

namespace Xannden.VSGLSL.Extensions
{
	internal static class ArrayExtensions
	{
		public static bool Contains<T>(this T[] array, T value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(value))
				{
					return true;
				}
			}

			return false;
		}

		public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] array, Converter<TInput, TOutput> converter)
		{
			return Array.ConvertAll(array, converter);
		}
	}
}
