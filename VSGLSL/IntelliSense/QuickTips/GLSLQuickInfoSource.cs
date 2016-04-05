using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Syntax;
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
			SyntaxNode declarationNode = this.FindDefinition(identifier);

			if (declarationNode == null)
			{
				return;
			}

			quickInfoContent.Add(new QuickTipPanel(this.GetIcon(identifier), this.CreateTextBlock(declarationNode, identifier), null));
			applicableToSpan = (identifier.Span as VSTrackingSpan).TrackingSpan;
		}

		public void Dispose()
		{
		}

		private SyntaxNode FindDefinition(IdentifierSyntax identifier)
		{
			if (identifier == null)
			{
				return null;
			}

			for (int i = 0; i < identifier.Tree.MacroDefinitions.Count; i++)
			{
				if (identifier.Tree.MacroDefinitions[i].Identifier.Text == identifier.Text)
				{
					return identifier.Tree.MacroDefinitions[i];
				}
			}

			SyntaxNode node = identifier.Parent;

			while (node != null)
			{
				foreach (SyntaxNode sibling in node.SiblingsAndSelf)
				{
					DeclarationSyntax declaration;

					if (sibling.SyntaxType == SyntaxType.SimpleStatement)
					{
						declaration = (sibling as SimpleStatementSyntax)?.Declaration;
					}
					else
					{
						declaration = sibling as DeclarationSyntax;
					}

					if (declaration?.InitDeclaratorList != null)
					{
						List<InitPartSyntax> initParts = declaration.InitDeclaratorList.InitParts?.GetNodes();

						InitPartSyntax initPart = initParts?.Find(part => part.Identifier.Name == identifier.Name);

						if (initPart != null)
						{
							return initPart;
						}
					}
					else if (sibling.SyntaxType == SyntaxType.StructDeclarator && sibling.Parent?.SyntaxType == SyntaxType.StructDefinition)
					{
						StructDefinitionSyntax structDefinition = sibling.Parent as StructDefinitionSyntax;

						if (structDefinition.StructDeclarator?.Identifier.Name == identifier.Name)
						{
							return structDefinition;
						}
					}
					else if (sibling.SyntaxType == SyntaxType.FunctionDefinition)
					{
						FunctionDefinitionSyntax functionDefinition = sibling as FunctionDefinitionSyntax;

						if (functionDefinition?.FunctionHeader?.Identifier.Name == identifier.Name)
						{
							return functionDefinition.FunctionHeader;
						}
					}
					else if (sibling.SyntaxType == SyntaxType.Parameter)
					{
						ParameterSyntax parameter = sibling as ParameterSyntax;

						if (parameter.Identifier.Name == identifier.Name)
						{
							return parameter;
						}
					}
					else if (sibling.SyntaxType == SyntaxType.Block)
					{
						FunctionHeaderSyntax header = (sibling.Parent as FunctionDefinitionSyntax)?.FunctionHeader;

						for (int i = 0; i < header?.Parameters.Count; i++)
						{
							if (header.Parameters[i].Identifier.Name == identifier.Name)
							{
								return header.Parameters[i];
							}
						}
					}
					else if (declaration?.StructDefinition != null)
					{
						if (identifier.Parent?.SyntaxType == SyntaxType.FieldSelection || identifier.Parent?.SyntaxType == SyntaxType.StructDeclarator)
						{
							for (int i = 0; i < declaration.StructDefinition.StructDeclarations.Count; i++)
							{
								foreach (StructDeclaratorSyntax declarator in declaration.StructDefinition.StructDeclarations[i].StructDeclarators.GetNodes())
								{
									if (declarator.Identifier.Name == identifier.Name)
									{
										return declarator;
									}
								}
							}
						}

						if (declaration.StructDefinition.TypeName?.Identifier.Name == identifier.Name)
						{
							return declaration.StructDefinition.TypeName;
						}

						if (declaration.StructDefinition.StructDeclarator?.Identifier.Name == identifier.Name)
						{
							return declaration.StructDefinition;
						}
					}
				}

				node = node.Parent;
			}

			return null;
		}

		private string GetClassificationName(SyntaxToken token)
		{
			if (token.SyntaxType.IsPreprocessor())
			{
				return GLSLConstants.PreprocessorKeyword;
			}

			if ((token as IdentifierSyntax)?.IsMacro() ?? false)
			{
				return GLSLConstants.Macro;
			}

			if (token?.IsExcludedCode() ?? false)
			{
				return GLSLConstants.ExcludedCode;
			}

			if (token.SyntaxType.IsPuctuation())
			{
				return GLSLConstants.Punctuation;
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

			if (token.SyntaxType == SyntaxType.IdentifierToken)
			{
				IdentifierSyntax identifier = token as IdentifierSyntax;

				if (identifier.IsParameter())
				{
					return GLSLConstants.Parameter;
				}

				if (identifier.IsFunction())
				{
					return GLSLConstants.Function;
				}

				if (identifier.IsTypeName())
				{
					return GLSLConstants.TypeName;
				}

				if (identifier.IsField())
				{
					return GLSLConstants.Field;
				}

				if (identifier.IsLocalVariable())
				{
					return GLSLConstants.LocalVariable;
				}

				if (identifier.IsGlobalVariable())
				{
					return GLSLConstants.GlobalVariable;
				}

				return GLSLConstants.Identifier;
			}

			return PredefinedClassificationTypeNames.FormalLanguage;
		}

		private FrameworkElement GetIcon(IdentifierSyntax identifier)
		{
			ImageSource imageSource = null;

			if (identifier.IsField())
			{
				imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupField, StandardGlyphItem.GlyphItemPublic);
			}
			else if (identifier.IsMacro())
			{
				imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMacro, StandardGlyphItem.GlyphItemPublic);
			}
			else if (identifier.IsFunction())
			{
				imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMethod, StandardGlyphItem.GlyphItemPublic);
			}
			else if (identifier.IsGlobalVariable())
			{
				imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
			}
			else if (identifier.IsLocalVariable())
			{
				imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
			}
			else if (identifier.IsParameter())
			{
				imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
			}
			else if (identifier.IsTypeName())
			{
				imageSource = this.provider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupStruct, StandardGlyphItem.GlyphItemPublic);
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

		private List<Run> GetRuns(SyntaxNode node, IClassificationFormatMap formatMap, IdentifierSyntax identifier)
		{
			List<Run> runs = new List<Run>();

			string identifierType = this.GetClassificationName(identifier);

			Run typeRun = null;

			switch (identifierType)
			{
				case GLSLConstants.LocalVariable:
					typeRun = new Run("(local variable) ");
					break;
				case GLSLConstants.Field:
					typeRun = new Run("(field) ");
					break;
				case GLSLConstants.GlobalVariable:
					typeRun = new Run("(global variable) ");
					break;
				case GLSLConstants.Macro:
					typeRun = new Run("(macro) ");
					break;
				case GLSLConstants.Parameter:
					typeRun = new Run("(parameter) ");
					break;
				case GLSLConstants.TypeName:
					typeRun = new Run("(struct) ");
					break;
			}

			if (typeRun != null)
			{
				typeRun.SetTextProperties(formatMap.GetTextProperties(this.provider.TypeRegistry.GetClassificationType(PredefinedClassificationTypeNames.FormalLanguage)));
				runs.Add(typeRun);
			}

			switch (node.SyntaxType)
			{
				case SyntaxType.InitPart:
					InitPartSyntax initPart = node as InitPartSyntax;
					InitDeclaratorListSyntax declaratorList = node.Parent as InitDeclaratorListSyntax;

					if (declaratorList.TypeQualifier != null)
					{
						foreach (SyntaxToken token in declaratorList.TypeQualifier.SyntaxTokens)
						{
							runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
						}
					}

					foreach (SyntaxToken token in declaratorList.Type.SyntaxTokens)
					{
						runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
					}

					runs.Add(initPart.Identifier.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(initPart.Identifier))));

					for (int i = 0; i < initPart.ArraySpecifiers.Count; i++)
					{
						foreach (SyntaxToken token in initPart.ArraySpecifiers[i].SyntaxTokens)
						{
							runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
						}
					}

					break;

				case SyntaxType.FunctionHeader:

					foreach (SyntaxToken token in node.SyntaxTokens)
					{
						runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
					}

					break;

				case SyntaxType.StructDefinition:
					StructDefinitionSyntax structDef = node as StructDefinitionSyntax;

					runs.Add(structDef.TypeName.Identifier.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(GLSLConstants.TypeName), " "));

					foreach (SyntaxToken token in structDef.StructDeclarator.SyntaxTokens)
					{
						runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
					}

					break;

				case SyntaxType.DefinePreprocessor:
					DefinePreprocessorSyntax definePreprocessor = node as DefinePreprocessorSyntax;

					foreach (SyntaxToken token in definePreprocessor.SyntaxTokens)
					{
						runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
					}

					break;
				case SyntaxType.Parameter:

					foreach (SyntaxToken token in node.SyntaxTokens)
					{
						runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
					}

					break;

				case SyntaxType.TypeName:

					foreach (SyntaxToken token in node.SyntaxTokens)
					{
						runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
					}

					break;

				case SyntaxType.StructDeclarator:
					StructDeclarationSyntax declaration = node.Parent as StructDeclarationSyntax;
					StructDeclaratorSyntax declarator = node as StructDeclaratorSyntax;

					if (declaration.TypeQualifier != null)
					{
						foreach (SyntaxToken token in declaration.TypeQualifier.SyntaxTokens)
						{
							runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
						}
					}

					foreach (SyntaxToken token in declaration.Type.SyntaxTokens)
					{
						runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
					}

					runs.Add(declarator.Identifier.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(declarator.Identifier))));

					for (int i = 0; i < declarator.ArraySpecifiers.Count; i++)
					{
						foreach (SyntaxToken token in declarator.ArraySpecifiers[i].SyntaxTokens)
						{
							runs.Add(token.ToRun(formatMap, this.provider.TypeRegistry.GetClassificationType(this.GetClassificationName(token))));
						}
					}

					break;
			}

			return runs;
		}

		private TextBlock CreateTextBlock(SyntaxNode node, IdentifierSyntax identifier)
		{
			TextBlock block = new TextBlock
			{
				TextWrapping = TextWrapping.Wrap
			};

			IClassificationFormatMap formatMap = this.provider.FormatMap.GetClassificationFormatMap("tooltip");

			block.SetTextProperties(formatMap.DefaultTextProperties);

			block.Inlines.AddRange(this.GetRuns(node, formatMap, identifier));

			return block;
		}
	}
}