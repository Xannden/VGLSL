﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Outlining
{
	internal sealed class GLSLOutliningTagger : ITagger<IOutliningRegionTag>
	{
		private readonly VSSource source;
		private List<Region> regions = new List<Region>();
		private object lockObject = new object();

		public GLSLOutliningTagger(VSSource source)
		{
			this.source = source;

			this.source.DoneParsing += this.Source_DoneParsing;
		}

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

		public IEnumerable<ITagSpan<IOutliningRegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			VSSnapshot snapshot = this.source.CurrentSnapshot as VSSnapshot;

			List<Region> list;

			lock (this.lockObject)
			{
				list = this.regions;
			}

			foreach (Region region in list)
			{
				// TODO: make the collapsed text have syntax coloring
				yield return new TagSpan<IOutliningRegionTag>(new SnapshotSpan(snapshot.TextSnapshot, region.Span.GetSpan(snapshot).ToVSSpan()), new OutliningRegionTag(false, true, "...", region.Text));
			}
		}

		private void Source_DoneParsing(object sender, EventArgs e)
		{
			this.UpdateRegions(this.source.CurrentSnapshot as VSSnapshot);
		}

		private void UpdateRegions(VSSnapshot snapshot)
		{
			SyntaxTree tree = this.source.Tree;

			if (tree == null || snapshot == null)
			{
				return;
			}

			List<Region> newRegions = new List<Region>();

			foreach (SyntaxNode node in tree.Root.Descendants)
			{
				switch (node.SyntaxType)
				{
					case SyntaxType.FunctionDefinition:
						FunctionDefinitionSyntax functionDefinition = node as FunctionDefinitionSyntax;

						if (functionDefinition.FunctionHeader?.RightParentheses != null && functionDefinition.Block?.RightBrace != null)
						{
							this.AddRegion(newRegions, snapshot, functionDefinition.FunctionHeader.RightParentheses.Span.GetSpan(snapshot).End + 1, functionDefinition.FunctionHeader.RightParentheses, functionDefinition.Block.RightBrace);
						}

						break;

					case SyntaxType.InterfaceBlock:
						InterfaceBlockSyntax interfaceBlock = node as InterfaceBlockSyntax;

						if (interfaceBlock.Identifier != null && interfaceBlock.RightBrace != null)
						{
							this.AddRegion(newRegions, snapshot, interfaceBlock.Identifier.Span.GetSpan(snapshot).End + 1, interfaceBlock.Identifier, interfaceBlock.RightBrace);
						}

						break;

					case SyntaxType.StructSpecifier:
						StructSpecifierSyntax structSpecifier = node as StructSpecifierSyntax;

						if (structSpecifier.RightBrace != null)
						{
							if (structSpecifier.TypeName != null)
							{
								this.AddRegion(newRegions, snapshot, structSpecifier.TypeName.Span.GetSpan(snapshot).End, structSpecifier.TypeName, structSpecifier.RightBrace);
							}
							else if (structSpecifier.StructKeyword != null)
							{
								this.AddRegion(newRegions, snapshot, structSpecifier.StructKeyword.Span.GetSpan(snapshot).End + 1, structSpecifier.StructKeyword, structSpecifier.RightBrace);
							}
						}

						break;
					case SyntaxType.IfDefinedPreprocessor:
						IfDefinedPreprocessorSyntax ifDefined = node as IfDefinedPreprocessorSyntax;

						if (ifDefined.EndIfKeyword != null)
						{
							this.AddRegion(newRegions, snapshot, ifDefined.IfDefinedKeyword.Span.GetSpan(snapshot).End + 1, ifDefined.IfDefinedKeyword, ifDefined.EndIfKeyword);
						}

						break;
					case SyntaxType.IfPreprocessor:
						IfPreprocessorSyntax ifPreprocessor = node as IfPreprocessorSyntax;

						if (ifPreprocessor.EndIfKeyword != null)
						{
							this.AddRegion(newRegions, snapshot, ifPreprocessor.IfKeyword.Span.GetSpan(snapshot).End + 1, ifPreprocessor.IfKeyword, ifPreprocessor.EndIfKeyword);
						}

						break;
					case SyntaxType.IfNotDefinedPreprocessor:
						IfNotDefinedPreprocessorSyntax ifNotDefined = node as IfNotDefinedPreprocessorSyntax;

						if (ifNotDefined.EndIfKeyword != null)
						{
							this.AddRegion(newRegions, snapshot, ifNotDefined.IfNotDefinedKeyword.Span.GetSpan(snapshot).End + 1, ifNotDefined.IfNotDefinedKeyword, ifNotDefined.EndIfKeyword);
						}

						break;
				}
			}

			foreach (IfPreprocessor preprocessor in this.source.Settings.Preprocessors)
			{
				if (preprocessor.EndIf != null)
				{
					this.AddRegion(newRegions, snapshot, preprocessor.Keyword.Span.GetSpan(snapshot).End + 1, preprocessor.Keyword, preprocessor.EndIf.EndIfKeyword);
				}

				for (int i = 0; i < preprocessor.ElsePreprocessors.Count; i++)
				{
					SyntaxToken next;

					if (i < preprocessor.ElsePreprocessors.Count - 1)
					{
						ElseIfPreprocessorSyntax elseIf = preprocessor.ElsePreprocessors[i].Keyword.Parent as ElseIfPreprocessorSyntax;

						if (elseIf != null)
						{
							next = elseIf.ExcludedCode.Code.Last() as SyntaxToken;
						}
						else
						{
							next = preprocessor.ElsePreprocessors[i].Keyword;
						}
					}
					else
					{
						next = preprocessor.EndIf.EndIfKeyword;
					}

					this.AddRegion(newRegions, snapshot, preprocessor.ElsePreprocessors[i].Keyword.Span.GetSpan(snapshot).End + 1, preprocessor.ElsePreprocessors[i].Keyword, next);
				}
			}

			lock (this.lockObject)
			{
				this.regions = newRegions;
			}

			this.TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshot.TextSnapshot, snapshot.Span.ToVSSpan())));
		}

		private void AddRegion(List<Region> list, VSSnapshot snapshot, int startPosition, SyntaxNode startNode, SyntaxNode endNode)
		{
			int start = startNode.Span.GetSpan(snapshot).End;
			int end = endNode.Span.GetSpan(snapshot).End;

			SourceLine startLine = snapshot.GetLineFromPosition(start);
			SourceLine endLine = snapshot.GetLineFromPosition(end);

			string text;

			if (endLine.LineNumber - startLine.LineNumber > 10)
			{
				int textStart = snapshot.GetLineFromLineNumber(startLine.LineNumber).Span.Start;

				text = snapshot.GetText(textStart, snapshot.GetLineFromLineNumber(startLine.LineNumber + 10).Span.End - textStart);
			}
			else
			{
				text = snapshot.GetText(startLine.Span.Start, endLine.Span.End - startLine.Span.Start).Trim('\r', '\n');
			}

			list.Add(new Region(snapshot.CreateTrackingSpan(GLSL.Text.Span.Create(startPosition, end)), text));
		}
	}
}