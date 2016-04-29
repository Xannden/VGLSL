using System.Collections.Generic;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;

namespace Xannden.GLSL.Semantics
{
	public class TypeDefinition
	{
		internal TypeDefinition(TypeSyntax type)
		{
			this.TypeToken = GetTypeToken(type);
			this.ArraySpecifiers = type.ArraySpecifiers;
		}

		internal TypeDefinition(IdentifierSyntax identifier)
		{
			this.TypeToken = identifier;
			this.ArraySpecifiers = new List<ArraySpecifierSyntax>();
		}

		internal TypeDefinition(ReturnTypeSyntax returnType)
		{
			if (returnType.VoidKeyword != null)
			{
				this.TypeToken = returnType.VoidKeyword;
				this.ArraySpecifiers = new List<ArraySpecifierSyntax>();
			}
			else
			{
				this.TypeToken = GetTypeToken(returnType.TypeSyntax);
				this.ArraySpecifiers = returnType.TypeSyntax.ArraySpecifiers;
			}
		}

		public SyntaxToken TypeToken { get; }

		public IReadOnlyList<ArraySpecifierSyntax> ArraySpecifiers { get; }

		private static SyntaxToken GetTypeToken(TypeSyntax type)
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
