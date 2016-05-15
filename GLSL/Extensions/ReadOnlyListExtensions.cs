using System;
using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;

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

		public static IReadOnlyList<T> FindAll<T>(this IReadOnlyList<T> list, Predicate<T> predicate)
		{
			if (list == null)
			{
				throw new ArgumentNullException(nameof(list));
			}

			List<T> result = new List<T>();

			for (int i = 0; i < (list?.Count ?? 0); i++)
			{
				if (predicate?.Invoke(list[i]) ?? false)
				{
					result.Add(list[i]);
				}
			}

			return result;
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

		public static bool Contains<T>(this IReadOnlyList<T> list, T value)
		{
			if (list == null)
			{
				throw new ArgumentNullException(nameof(list));
			}

			for (int i = 0; i < (list?.Count ?? 0); i++)
			{
				if (list[i].Equals(value))
				{
					return true;
				}
			}

			return false;
		}

		public static T Last<T>(this IReadOnlyList<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException(nameof(list));
			}

			if (list.Count <= 0)
			{
				return default(T);
			}

			return list[list.Count - 1];
		}

		public static IReadOnlyList<SyntaxToken> GetSyntaxTokens<T>(this IReadOnlyList<T> list) where T : SyntaxNode
		{
			List<SyntaxToken> tokens = new List<SyntaxToken>();

			for (int i = 0; i < list?.Count; i++)
			{
				tokens.AddRange(list[i].GetSyntaxTokens());
			}

			return tokens;
		}

		public static IEnumerable<TResult> ConvertList<TInput, TResult>(this IReadOnlyList<TInput> list, Func<TInput, TResult> converter)
		{
			for (int i = 0; i < list.Count; i++)
			{
				yield return converter(list[i]);
			}
		}

		public static IEnumerable<TResult> ConvertList<TInput, TResult>(this IReadOnlyList<TInput> list, Func<TInput, TResult> converter, TResult seperator, bool endWithSeperator = false)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (i != 0)
				{
					yield return seperator;
				}

				yield return converter(list[i]);
			}

			if (endWithSeperator && list.Count > 0)
			{
				yield return seperator;
			}
		}
	}
}
