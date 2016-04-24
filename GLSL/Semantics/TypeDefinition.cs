using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public class TypeDefinition
	{
		internal TypeDefinition(TypeSyntax type)
		{
			this.Type = this.GetTypeToken(type);
			this.ArraySpecifiers = type.ArraySpecifiers;
		}

		internal TypeDefinition(IdentifierSyntax identifier)
		{
			this.Type = identifier;
			this.ArraySpecifiers = new List<ArraySpecifierSyntax>();
		}

		internal TypeDefinition(ReturnTypeSyntax returnType)
		{
			if (returnType.VoidKeyword != null)
			{
				this.Type = returnType.VoidKeyword;
				this.ArraySpecifiers = new List<ArraySpecifierSyntax>();
			}
			else
			{
				this.Type = this.GetTypeToken(returnType.TypeSyntax);
				this.ArraySpecifiers = returnType.TypeSyntax.ArraySpecifiers;
			}
		}

		public SyntaxToken Type { get; }

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers { get; }

		private SyntaxToken GetTypeToken(TypeSyntax type)
		{
			if (type.TypeNonArray.TypeName != null)
			{
				return type.TypeNonArray.TypeName.Identifier;
			}
			else if (type.TypeNonArray.StructSpecifier != null)
			{
				return type.TypeNonArray.StructSpecifier.TypeName.Identifier;
			}
			else
			{
				return type.TypeNonArray.Keyword;
			}
		}
	}
}
