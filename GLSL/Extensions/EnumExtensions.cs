using System;

namespace Xannden.GLSL.Extensions
{
	public static class EnumExtensions
	{
		public static bool HasFlag<T>(this T enumeration, T flag) where T : struct, IConvertible
		{
			return (enumeration.ToInt64(null) & flag.ToInt64(null)) != 0;
		}

		public static bool HasFlags<T>(this T enumeration, params T[] flags) where T : struct, IConvertible
		{
			long combinedFlags = 0;

			for (int i = 0; i < flags.Length; i++)
			{
				combinedFlags |= flags[i].ToInt64(null);
			}

			return (enumeration.ToInt64(null) & combinedFlags) != 0;
		}

		public static T SetFlag<T>(this T enumeration, T flag) where T : struct, IConvertible
		{
			long value = enumeration.ToInt64(null) | flag.ToInt64(null);

			return (T)(object)value;
		}

		public static T UnsetFlag<T>(this T enumeration, T flag) where T : struct, IConvertible
		{
			long value = enumeration.ToInt64(null) & (~flag.ToInt64(null));

			return (T)(object)value;
		}
	}
}
