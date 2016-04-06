using System.Collections.Generic;
using Xannden.GLSL.Syntax.Trivia;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class IdentifierSyntax : SyntaxToken
	{
		internal IdentifierSyntax(SyntaxTree tree, TrackingSpan span, string text, SyntaxTrivia leadingTrivia, SyntaxTrivia trailingTrivia, Snapshot snapshot, bool isMissing) : base(tree, SyntaxType.IdentifierToken, span, text, leadingTrivia, trailingTrivia, snapshot, isMissing)
		{
		}

		public string Name => this.Text;

		public bool IsField()
		{
			return (this.Parent?.SyntaxType == SyntaxType.FieldSelection) || (this.Parent?.SyntaxType == SyntaxType.StructDeclarator && this.Parent?.Parent?.SyntaxType == SyntaxType.StructDeclaration);
		}

		public bool IsFunction()
		{
			return this.Parent?.SyntaxType == SyntaxType.FunctionHeader || this.Parent?.SyntaxType == SyntaxType.FunctionCall;
		}

		public bool IsGlobalVariable()
		{
			foreach (SyntaxNode node in this.Tree.Root.Children)
			{
				if (node.SyntaxType == SyntaxType.FunctionDefinition)
				{
					continue;
				}

				IReadOnlyList<InitPartSyntax> initParts = (node as DeclarationSyntax)?.InitDeclaratorList?.InitParts.Nodes;

				for (int i = 0; i < (initParts?.Count ?? 0); i++)
				{
					if (this.Text == initParts[i].Identifier.Name)
					{
						return true;
					}
				}

				StructDefinitionSyntax structDefinition = (node as DeclarationSyntax)?.StructDefinition;

				if (this.Text == structDefinition?.StructDeclarator?.Identifier?.Name)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsLocalVariable()
		{
			foreach (SyntaxNode ancestor in this.Ancestors)
			{
				if (ancestor.SyntaxType == SyntaxType.FunctionDefinition)
				{
					break;
				}

				foreach (SyntaxNode sibling in ancestor.SiblingsAndSelf)
				{
					IReadOnlyList<InitPartSyntax> initparts = (sibling as SimpleStatementSyntax)?.Declaration?.InitDeclaratorList?.InitParts.Nodes;

					for (int i = 0; i < (initparts?.Count ?? 0); i++)
					{
						if (initparts[i].Identifier.Name == this.Text)
						{
							return true;
						}
					}

					StructDefinitionSyntax structDef = (sibling as SimpleStatementSyntax)?.Declaration?.StructDefinition;

					if (structDef?.StructDeclarator?.Identifier.Name == this.Text)
					{
						return true;
					}
				}
			}

			return false;
		}

		public bool IsMacro()
		{
			if (this.Parent?.SyntaxType == SyntaxType.DefinePreprocessor)
			{
				return true;
			}
			else
			{
				for (int i = 0; i < this.Tree.MacroDefinitions.Count; i++)
				{
					if (this.Tree.MacroDefinitions[i]?.Identifier.Text == this.Text)
					{
						return true;
					}
				}
			}

			return false;
		}

		public bool IsParameter()
		{
			if (this.Parent?.SyntaxType == SyntaxType.FunctionHeader)
			{
				return false;
			}

			foreach (SyntaxNode ancestor in this.Ancestors)
			{
				if (ancestor.SyntaxType == SyntaxType.FunctionDefinition)
				{
					FunctionDefinitionSyntax functionDef = ancestor as FunctionDefinitionSyntax;

					IReadOnlyList<ParameterSyntax> parameters = functionDef.FunctionHeader.Parameters;

					for (int i = 0; i < parameters.Count; i++)
					{
						if (parameters[i].Identifier?.Name == this.Text)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public bool IsTypeName()
		{
			return this.Parent?.SyntaxType == SyntaxType.TypeName;
		}
	}
}