using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.IntelliSense.QuickTips
{
	internal class GLSLQuickInfoSource : IQuickInfoSource
	{
		private readonly GLSLQuickInfoSourceProvider provider;
		private readonly Source source;
		private readonly ITextBuffer textBuffer;

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
			IdentifierSyntax identifier = tree.GetNodeFromPosition(snapshot, triggerPosition) as IdentifierSyntax;
			IClassificationFormatMap formatMap = this.provider.FormatMap.GetClassificationFormatMap("text");

			if (identifier?.Definition == null)
			{
				return;
			}

			quickInfoContent.Add(new QuickTipPanel(identifier.Definition.GetIcon(this.provider.GlyphService), identifier.Definition.ToTextBlock(formatMap, this.provider.TypeRegistry), null));
			applicableToSpan = (identifier.Span as VSTrackingSpan).TrackingSpan;
		}

		public void Dispose()
		{
			// Method intentionally left empty.
		}
	}
}