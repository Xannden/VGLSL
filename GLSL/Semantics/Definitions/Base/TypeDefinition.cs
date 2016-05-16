using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
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
				this.Type = SyntaxType.VoidKeyword;
				this.ArraySpecifiers = new List<ColoredString>();
			}
			else
			{
				this.Type = type.TypeSyntax.TypeNonArray.Children[0].SyntaxType;

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
			this.Type = type.TypeNonArray.Children[0].SyntaxType;

			List<ColoredString> arraySpecifiers = new List<ColoredString>();

			for (int i = 0; i < type.ArraySpecifiers.Count; i++)
			{
				arraySpecifiers.AddRange(type.ArraySpecifiers[i].ToColoredString());
			}

			this.ArraySpecifiers = arraySpecifiers;
		}

		internal TypeDefinition(SyntaxType type)
		{
			this.Type = type;
			this.ArraySpecifiers = new List<ColoredString>();
		}

		internal TypeDefinition(Definition definition)
		{
			this.Type = SyntaxType.TypeName;
			this.Definition = definition;
			this.ArraySpecifiers = new List<ColoredString>();
		}

		public SyntaxType Type { get; }

		public IReadOnlyList<ColoredString> ArraySpecifiers { get; }

		public Definition Definition { get; }

		public IReadOnlyList<ColoredString> GetColoredText()
		{
			List<ColoredString> list = new List<ColoredString>();

			if (this.Type == SyntaxType.TypeName)
			{
				list.AddRange(this.Definition.GetColoredText());
			}
			else
			{
				list.Add(this.Type.ToColoredString());
			}

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