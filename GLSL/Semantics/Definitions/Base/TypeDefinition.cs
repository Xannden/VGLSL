using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public sealed class TypeDefinition
	{
		public TypeDefinition(ReturnTypeSyntax type)
		{
			if (type.VoidKeyword != null)
			{
				this.Type = type.VoidKeyword.ToColoredString();
			}
			else
			{
				this.Type = type.TypeSyntax.TypeNonArray.ToColoredString();

				List<ColoredString> arraySpecifiers = new List<ColoredString>();

				for (int i = 0; i < type.TypeSyntax.ArraySpecifiers.Count; i++)
				{
					arraySpecifiers.AddRange(type.TypeSyntax.ArraySpecifiers[i].ToColoredString());
				}

				this.ArraySpecifiers = arraySpecifiers;
			}
		}

		public TypeDefinition(TypeSyntax type)
		{
			this.Type = type.TypeNonArray.ToColoredString();

			List<ColoredString> arraySpecifiers = new List<ColoredString>();

			for (int i = 0; i < type.ArraySpecifiers.Count; i++)
			{
				arraySpecifiers.AddRange(type.ArraySpecifiers[i].ToColoredString());
			}

			this.ArraySpecifiers = arraySpecifiers;
		}

		internal TypeDefinition(ColoredString type)
		{
			this.Type = new List<ColoredString> { type, ColoredString.Create(" ", ColorType.WhiteSpace) };
			this.ArraySpecifiers = new List<ColoredString>();
		}

		internal TypeDefinition(string type)
		{
			this.Type = new List<ColoredString> { ColoredString.Create(type, ColorType.Keyword), ColoredString.Create(" ", ColorType.WhiteSpace) };
			this.ArraySpecifiers = new List<ColoredString>();
		}

		public IReadOnlyList<ColoredString> Type { get; }

		public IReadOnlyList<ColoredString> ArraySpecifiers { get; }

		public IReadOnlyList<ColoredString> GetColoredText()
		{
			List<ColoredString> list = new List<ColoredString>();

			list.AddRange(this.Type);
			list.AddRange(this.ArraySpecifiers);

			return list;
		}

		internal bool Equals(TypeDefinition other)
		{
			if (other == null)
			{
				return false;
			}

			if (this.Type != other.Type || this.ArraySpecifiers.Count != other.ArraySpecifiers.Count)
			{
				return false;
			}

			for (int i = 0; i < this.ArraySpecifiers.Count; i++)
			{
				if (this.ArraySpecifiers[i] != other.ArraySpecifiers[i])
				{
					return false;
				}
			}

			return true;
		}
	}
}