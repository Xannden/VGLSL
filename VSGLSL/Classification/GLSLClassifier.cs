using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Extensions.Text;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Classification
{
	internal sealed class GLSLClassifier : IClassifier
	{
		private IClassificationTypeRegistryService classificationTypeRegistryService;
		private GLSLLexer lexer = new GLSLLexer();
		private VSSource source;

		internal GLSLClassifier(VSSource source, IClassificationTypeRegistryService classificationTypeRegistryService)
		{
			this.source = source;
			this.classificationTypeRegistryService = classificationTypeRegistryService;

			this.source.DoneParsing += this.Source_DoneParsing;
		}

		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			LinkedList<Token> tokens = this.lexer.Run(new VSSnapshot(this.source, span.Snapshot), span.Start.GetContainingLine().ExtentIncludingLineBreak.Span.ToGLSLSpan());
			List<ClassificationSpan> spans = new List<ClassificationSpan>();
			List<GLSL.Text.TrackingSpan> commentSpans = this.source.CommentSpans;
			VSSnapshot currentSnapshot = new VSSnapshot(this.source, span.Snapshot);

			SyntaxTree tree = this.source.Tree;

			if (tree == null)
			{
				return spans;
			}

			foreach (Token token in tokens)
			{
				if (!span.OverlapsWith(token.Span.ToVSSpan()))
				{
					continue;
				}

				SyntaxNode node = tree?.GetNodeFromPosition(this.source.CurrentSnapshot, token.Span.Start);

				string classificationName = string.Empty;

				if (token.IsPreprocessor)
				{
					classificationName = GLSLConstants.PreprocessorKeyword;
				}
				else if (token.Type == SyntaxType.IdentifierToken && this.IsMacro(token, node))
				{
					classificationName = GLSLConstants.Macro;
				}
				else if (this.IsExcludedCode(node))
				{
					classificationName = GLSLConstants.ExcludedCode;
				}
				else if (token.IsPuctuation)
				{
					classificationName = GLSLConstants.Punctuation;
				}
				else if (this.IsPreprocessorText(node))
				{
					classificationName = GLSLConstants.PreprocessorText;
				}
				else if (token.IsKeyword)
				{
					classificationName = GLSLConstants.Keyword;
				}
				else if (token.IsNumber)
				{
					classificationName = GLSLConstants.Number;
				}
				else if (token.Type == SyntaxType.IdentifierToken)
				{
					if (this.IsParameter(token, node))
					{
						classificationName = GLSLConstants.Parameter;
					}
					else if (this.IsFunction(token, node))
					{
						classificationName = GLSLConstants.Function;
					}
					else if (this.IsTypeName(token, node))
					{
						classificationName = GLSLConstants.TypeName;
					}
					else if (this.IsField(token, node))
					{
						classificationName = GLSLConstants.Field;
					}
					else if (this.IsLocalVariable(token, node))
					{
						classificationName = GLSLConstants.LocalVariable;
					}
					else if (this.IsGlobalVariable(token, tree))
					{
						classificationName = GLSLConstants.GlobalVariable;
					}
					else
					{
						classificationName = GLSLConstants.Identifier;
					}
				}

				if (classificationName != string.Empty)
				{
					SnapshotSpan snapshotSpan = new SnapshotSpan(span.Snapshot, token.Span.ToVSSpan());

					spans.Add(new ClassificationSpan(snapshotSpan, this.classificationTypeRegistryService.GetClassificationType(classificationName)));
				}
			}

			this.ColorComments(span, spans, currentSnapshot);

			return spans;
		}

		private void ColorComments(SnapshotSpan span, List<ClassificationSpan> spans, VSSnapshot snapshot)
		{
			List<GLSL.Text.TrackingSpan> commentSpans = this.source.CommentSpans;

			for (int i = 0; i < commentSpans.Count; i++)
			{
				Span commentSpan = commentSpans[i].GetSpan(snapshot).ToVSSpan();
				if (span.IntersectsWith(commentSpan))
				{
					spans.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, commentSpan), this.classificationTypeRegistryService.GetClassificationType(GLSLConstants.Comment)));
				}
			}
		}

		private bool IsExcludedCode(SyntaxNode node)
		{
			return node.Parent?.SyntaxType == SyntaxType.ExcludedCode;
		}

		private bool IsField(Token token, SyntaxNode node)
		{
			return (node?.SyntaxType == SyntaxType.IdentifierToken && node?.Parent?.SyntaxType == SyntaxType.FieldSelection) || (node?.SyntaxType == SyntaxType.IdentifierToken && node?.Parent?.SyntaxType == SyntaxType.StructDeclarator && node?.Parent?.Parent?.SyntaxType == SyntaxType.StructDeclaration);
		}

		private bool IsFunction(Token token, SyntaxNode node)
		{
			if (node.Parent?.SyntaxType == SyntaxType.FunctionHeader && node.SyntaxType == SyntaxType.IdentifierToken)
			{
				IdentifierSyntax identifier = node as IdentifierSyntax;

				return token.Text == identifier.Name;
			}
			else if (node.Parent?.SyntaxType == SyntaxType.FunctionCall && node.SyntaxType == SyntaxType.IdentifierToken)
			{
				IdentifierSyntax identifier = node as IdentifierSyntax;

				return token.Text == identifier.Name;
			}

			return false;
		}

		private bool IsGlobalVariable(Token token, SyntaxTree tree)
		{
			foreach (SyntaxNode node in tree.Root.Children)
			{
				if (node.SyntaxType == SyntaxType.FunctionDefinition)
				{
					continue;
				}

				List<InitPartSyntax> initParts = (node as DeclarationSyntax)?.InitDeclaratorListDeclaration?.InitDeclaratorList.InitParts.GetNodes();

				for (int i = 0; i < (initParts?.Count ?? 0); i++)
				{
					if (token.Text == initParts[i].Identifier.Name)
					{
						return true;
					}
				}

				StructDefinitionSyntax structDefinition = (node as DeclarationSyntax)?.StructDefinition;

				if (token.Text == structDefinition?.StructDeclarator?.Identifier?.Name)
				{
					return true;
				}
			}

			return false;
		}

		private bool IsLocalVariable(Token token, SyntaxNode node)
		{
			foreach (SyntaxNode ancestor in node.Ancestors)
			{
				if (ancestor.SyntaxType == SyntaxType.FunctionDefinition)
				{
					break;
				}

				foreach (SyntaxNode sibling in ancestor.SiblingsAndSelf)
				{
					List<InitPartSyntax> initparts = (sibling as SimpleStatementSyntax)?.Declaration?.InitDeclaratorListDeclaration?.InitDeclaratorList.InitParts.GetNodes();

					for (int i = 0; i < (initparts?.Count ?? 0); i++)
					{
						if (initparts[i].Identifier.Name == token.Text)
						{
							return true;
						}
					}

					StructDefinitionSyntax structDef = (sibling as SimpleStatementSyntax)?.Declaration?.StructDefinition;

					if (structDef?.StructDeclarator?.Identifier.Name == token.Text)
					{
						return true;
					}
				}
			}

			return false;
		}

		private bool IsMacro(Token token, SyntaxNode node)
		{
			return node.SyntaxType == SyntaxType.IdentifierToken && node.Parent?.SyntaxType == SyntaxType.DefinePreprocessor;
		}

		private bool IsParameter(Token token, SyntaxNode node)
		{
			if (node.Parent?.SyntaxType == SyntaxType.FunctionHeader)
			{
				return false;
			}

			foreach (SyntaxNode ancestor in node.Ancestors)
			{
				if (ancestor.SyntaxType == SyntaxType.FunctionDefinition)
				{
					FunctionDefinitionSyntax functionDef = ancestor as FunctionDefinitionSyntax;

					List<ParameterSyntax> parameters = functionDef.FunctionHeader.Parameters;

					for (int i = 0; i < parameters.Count; i++)
					{
						if (parameters[i].Identifier?.Name == token.Text)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		private bool IsPreprocessorText(SyntaxNode node)
		{
			foreach (SyntaxNode ancestor in node.AncestorsAndSelf)
			{
				if (ancestor.SyntaxType == SyntaxType.Preprocessor)
				{
					return true;
				}
			}

			return false;
		}

		private bool IsTypeName(Token token, SyntaxNode node)
		{
			return node.SyntaxType == SyntaxType.IdentifierToken && node.Parent?.SyntaxType == SyntaxType.TypeName;
		}

		private void Source_DoneParsing(object sender, EventArgs e)
		{
			this.ClassificationChanged.Invoke(this, new ClassificationChangedEventArgs(this.source.CurrentSnapshot.GetSnapshotSpan(this.source.CurrentSnapshot.Span)));
		}
	}
}