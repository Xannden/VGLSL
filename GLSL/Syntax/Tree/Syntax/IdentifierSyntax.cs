using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Syntax.Tree.Syntax
{
	public sealed class IdentifierSyntax : SyntaxToken
	{
		internal IdentifierSyntax(SyntaxTree tree, TrackingSpan span, string text, SyntaxTrivia leadingTrivia, SyntaxTrivia trailingTrivia, Snapshot snapshot, bool isMissing) : base(tree, SyntaxType.IdentifierToken, span, text, leadingTrivia, trailingTrivia, snapshot, isMissing)
		{
		}

		internal IdentifierSyntax(string text) : this(null, null, text, null, null, null, false)
		{
		}

		public string Identifier => this.Text;

		public Definition Definition { get; internal set; }

		protected override void ToColoredString(List<ColoredString> list)
		{
			ColorType type = ColorType.Identifier;

			if (this.SyntaxType.IsPreprocessor())
			{
				type = ColorType.PreprocessorKeyword;
			}

			if (this.Definition?.Kind == DefinitionKind.Macro)
			{
				type = ColorType.Macro;
			}

			if (this.IsExcludedCode())
			{
				type = ColorType.ExcludedCode;
			}

			if (this.SyntaxType.IsPunctuation())
			{
				type = ColorType.Punctuation;
			}

			if (this.IsPreprocessorText())
			{
				type = ColorType.PreprocessorText;
			}

			if (this.SyntaxType.IsKeyword())
			{
				type = ColorType.Keyword;
			}

			if (this.SyntaxType.IsNumber())
			{
				type = ColorType.Number;
			}

			if (this.Definition != null)
			{
				switch (this.Definition.Kind)
				{
					case DefinitionKind.Field:
						type = ColorType.Field;
						break;
					case DefinitionKind.Function:
						type = ColorType.Function;
						break;
					case DefinitionKind.GlobalVariable:
						type = ColorType.GlobalVariable;
						break;
					case DefinitionKind.LocalVariable:
						type = ColorType.LocalVariable;
						break;
					case DefinitionKind.Macro:
						type = ColorType.Macro;
						break;
					case DefinitionKind.Parameter:
						type = ColorType.Parameter;
						break;
					case DefinitionKind.TypeName:
					case DefinitionKind.InterfaceBlock:
						type = ColorType.TypeName;
						break;
				}
			}

			list.Add(ColoredString.Create(this.Text, type));

			if (this.HasTrailingTrivia)
			{
				this.TrailingTrivia.ToColoredString(list);
			}
		}
	}
}