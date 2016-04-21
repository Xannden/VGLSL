using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Semantics;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.IntelliSense.QuickTips
{
	// TODO: rework how this works
	internal class GLSLQuickInfoSource : IQuickInfoSource
	{
		private GLSLQuickInfoSourceProvider provider;
		private Source source;
		private ITextBuffer textBuffer;

		public GLSLQuickInfoSource(GLSLQuickInfoSourceProvider provider, Source source, ITextBuffer textBuffer)
		{
			this.provider = provider;
			this.source = source;
			this.textBuffer = textBuffer;
		}

		public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan)
		{
			applicableToSpan = null;

			if (session == null)
			{
				throw new ArgumentNullException(nameof(session));
			}

			if (quickInfoContent == null)
			{
				throw new ArgumentNullException(nameof(quickInfoContent));
			}

			VSSnapshot snapshot = this.source.CurrentSnapshot as VSSnapshot;

			if (snapshot == null)
			{
				return;
			}

			int triggerPosition = session.GetTriggerPoint(this.textBuffer).GetPosition(snapshot.TextSnapshot);

			SyntaxTree tree = this.source.Tree;
			SyntaxNode node = tree.GetNodeFromPosition(snapshot, triggerPosition);

			if (node == null || node.SyntaxType != SyntaxType.IdentifierToken)
			{
				return;
			}

			IdentifierSyntax identifier = node as IdentifierSyntax;

			if (identifier == null)
			{
				return;
			}

			Definition definition = tree.Definitions.Find(def => def.Scope.GetSpan(snapshot).Contains(identifier.Span.GetSpan(snapshot)) && identifier.Identifier == def.Identifier.Identifier);

			if (definition == null)
			{
				return;
			}

			quickInfoContent.Add(new QuickTipPanel(this.GetIcon(definition), this.CreateTextBlock(definition, snapshot), null));

			applicableToSpan = (identifier.Span as VSTrackingSpan).TrackingSpan;
		}

		public void Dispose()
		{
		}

		private string GetClassificationName(SyntaxToken token, Snapshot snapshot)
		{
			if (token.SyntaxType.IsPreprocessor())
			{
				return GLSLConstants.PreprocessorKeyword;
			}

			if (token?.IsExcludedCode() ?? false)
			{
				return GLSLConstants.ExcludedCode;
			}

			if (token.SyntaxType.IsPunctuation())
			{
				return GLSLConstants.Punctuation;
			}

			IdentifierSyntax identifier = token as IdentifierSyntax;

			Definition definition = token.Tree.Definitions.Find(def => def.Scope.GetSpan(snapshot).Contains(token.Span.GetSpan(snapshot)) && identifier?.Identifier == def.Identifier.Identifier);

			if (definition?.Type == DefinitionType.Macro)
			{
				return GLSLConstants.Macro;
			}

			if (token?.IsPreprocessorText() ?? false)
			{
				return GLSLConstants.PreprocessorText;
			}

			if (token.SyntaxType.IsKeyword())
			{
				return GLSLConstants.Keyword;
			}

			if (token.SyntaxType.IsNumber())
			{
				return GLSLConstants.Number;
			}

			if (definition != null)
			{
				switch (definition.Type)
				{
					case DefinitionType.Field:
						return GLSLConstants.Field;
					case DefinitionType.Function:
						return GLSLConstants.Function;
					case DefinitionType.GlobalVariable:
						return GLSLConstants.GlobalVariable;
					case DefinitionType.LocalVariable:
						return GLSLConstants.LocalVariable;
					case DefinitionType.Parameter:
						return GLSLConstants.Parameter;
					case DefinitionType.TypeName:
						return GLSLConstants.TypeName;
				}
			}

			if (identifier != null)
			{
				return GLSLConstants.Identifier;
			}

			return PredefinedClassificationTypeNames.FormalLanguage;
		}

		private FrameworkElement GetIcon(Definition definition)
		{
			ImageSource imageSource = null;

			switch (definition.Type)
			{
				case DefinitionType.Field:
					imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupField, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionType.LocalVariable:
				case DefinitionType.GlobalVariable:
				case DefinitionType.Parameter:
					imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionType.Macro:
					imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMacro, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionType.TypeName:
					imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupStruct, StandardGlyphItem.GlyphItemPublic);
					break;
				case DefinitionType.Function:
					imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMethod, StandardGlyphItem.GlyphItemPublic);
					break;
			}

			if (imageSource != null)
			{
				return new Image
				{
					Source = imageSource
				};
			}

			return null;
		}

		private List<Run> GetRuns(Definition definition, IClassificationFormatMap formatMap, Snapshot snapshot)
		{
			List<Run> runs = new List<Run>();

			Run typeRun = null;

			switch (definition.Type)
			{
				case DefinitionType.LocalVariable:
					typeRun = new Run("(local variable) ");
					break;
				case DefinitionType.Field:
					typeRun = new Run("(field) ");
					break;
				case DefinitionType.GlobalVariable:
					typeRun = new Run("(global variable) ");
					break;
				case DefinitionType.Macro:
					typeRun = new Run("(macro) ");
					break;
				case DefinitionType.Parameter:
					typeRun = new Run("(parameter) ");
					break;
				case DefinitionType.TypeName:
					typeRun = new Run("(struct) ");
					break;
			}

			if (typeRun != null)
			{
				typeRun.SetTextProperties(formatMap.GetTextProperties(this.provider.TypeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage)));
				runs.Add(typeRun);
			}

			Run spaceRun = new Run(" ");
			spaceRun.SetTextProperties(formatMap.GetTextProperties(this.provider.TypeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage)));

			switch (definition.Node.SyntaxType)
			{
				case SyntaxType.InitPart:
					InitDeclaratorListSyntax list = definition.Node.Parent as InitDeclaratorListSyntax;
					InitPartSyntax initPart = definition.Node as InitPartSyntax;

					if (list.TypeQualifier != null)
					{
						this.AddTokenRuns(list.TypeQualifier, runs, formatMap, snapshot);
					}

					this.AddTypeRuns(list.TypeNode, runs, formatMap, snapshot);

					this.AddTokenRuns(initPart.Identifier, runs, formatMap, snapshot);

					foreach (SyntaxNode item in initPart.ArraySpecifiers)
					{
						this.AddTokenRuns(item, runs, formatMap, snapshot);
					}

					break;
				case SyntaxType.StructDeclarator:
					StructDeclaratorSyntax declarator = definition.Node as StructDeclaratorSyntax;

					if (definition.Type == DefinitionType.Field)
					{
						StructDeclarationSyntax declaration = declarator.Parent as StructDeclarationSyntax;

						if (declaration.TypeQualifier != null)
						{
							this.AddTokenRuns(declaration.TypeQualifier, runs, formatMap, snapshot);
						}

						this.AddTypeRuns(declaration.TypeSyntax, runs, formatMap, snapshot);
					}
					else
					{
						StructDefinitionSyntax structDef = declarator.Parent as StructDefinitionSyntax;

						if (structDef.TypeQualifier != null)
						{
							this.AddTokenRuns(structDef.TypeQualifier, runs, formatMap, snapshot);
						}

						this.AddTokenRuns(structDef.TypeName, runs, formatMap, snapshot);

						runs.Add(spaceRun);
					}

					this.AddTokenRuns(declarator.Identifier, runs, formatMap, snapshot);

					foreach (SyntaxNode item in declarator.ArraySpecifiers)
					{
						this.AddTokenRuns(item, runs, formatMap, snapshot);
					}

					break;
				case SyntaxType.StructDefinition:
					StructDefinitionSyntax structDefinition = definition.Node as StructDefinitionSyntax;

					if (structDefinition.TypeQualifier != null)
					{
						this.AddTokenRuns(structDefinition.TypeQualifier, runs, formatMap, snapshot);
					}

					this.AddTokenRuns(structDefinition.TypeName, runs, formatMap, snapshot);

					runs.Add(spaceRun);

					StructDeclaratorSyntax declaratorNode = structDefinition.StructDeclarator;

					this.AddTokenRuns(declaratorNode.Identifier, runs, formatMap, snapshot);

					foreach (SyntaxNode item in declaratorNode.ArraySpecifiers)
					{
						this.AddTokenRuns(item, runs, formatMap, snapshot);
					}

					break;
				default:
					this.AddTokenRuns(definition.Node, runs, formatMap, snapshot);
					break;
			}

			return runs;
		}

		private TextBlock CreateTextBlock(Definition definition, Snapshot snapshot)
		{
			TextBlock block = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};

			IClassificationFormatMap formatMap = this.provider.FormatMap.GetClassificationFormatMap("tooltip");

			block.SetTextProperties(formatMap.DefaultTextProperties);

			block.Inlines.AddRange(this.GetRuns(definition, formatMap, snapshot));

			return block;
		}

		private IEnumerable<SyntaxToken> GetTokens(SyntaxNode node)
		{
			if (node is SyntaxToken)
			{
				yield return node as SyntaxToken;
			}
			else
			{
				for (int i = 0; i < node.Children.Count; i++)
				{
					if (node.Children[i].SyntaxType != SyntaxType.Preprocessor)
					{
						foreach (SyntaxToken token in this.GetTokens(node.Children[i]))
						{
							yield return token;
						}
					}
				}
			}
		}

		private void AddTokenRuns(SyntaxNode node, List<Run> runs, IClassificationFormatMap formatMap, Snapshot snapshot)
		{
			foreach (var item in this.GetTokens(node))
			{
				runs.Add(item.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(item, snapshot))));
			}
		}

		private void AddTypeRuns(TypeSyntax type, List<Run> runs, IClassificationFormatMap formatMap, Snapshot snapshot)
		{
			if (type.TypeNonArray.StructSpecifier != null)
			{
				this.AddTokenRuns(type.TypeNonArray.StructSpecifier.TypeName, runs, formatMap, snapshot);

				Run spaceRun = new Run(" ");
				spaceRun.SetTextProperties(formatMap.GetTextProperties(this.provider.TypeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage)));

				runs.Add(spaceRun);
			}
			else
			{
				this.AddTokenRuns(type, runs, formatMap, snapshot);
			}

			foreach (SyntaxNode item in type.ArraySpecifiers)
			{
				this.AddTokenRuns(item, runs, formatMap, snapshot);
			}
		}
	}
}