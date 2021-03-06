﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Sources;

namespace Xannden.VSGLSL.Commands
{
	[PartCreationPolicy(CreationPolicy.NonShared)]
	internal class GoToDefinitionCommand : VSCommand<VSConstants.VSStd97CmdID>
	{
		private readonly VSSource source;

		public GoToDefinitionCommand(IVsTextView textViewAdapter, ITextView textView) : base(textViewAdapter, textView)
		{
			this.source = VSSource.GetOrCreate(this.TextView.TextBuffer);

			this.AddCommand(VSConstants.VSStd97CmdID.GotoDefn);
		}

		protected override bool IsEnabled(VSConstants.VSStd97CmdID commandId)
		{
			return true;
		}

		protected override bool Run(VSConstants.VSStd97CmdID commandId)
		{
			SyntaxTree tree = this.source.Tree;

			if (tree == null)
			{
				return false;
			}

			VSSnapshot snapshot = this.source.CurrentSnapshot as VSSnapshot;
			int position = this.TextView.Caret.Position.BufferPosition.Position;

			IdentifierSyntax identifier = tree.GetNodeFromPosition(snapshot, position) as IdentifierSyntax;

			Span span = identifier?.Definition.DefinitionSpan?.GetSpan(snapshot);

			if (span == null)
			{
				return false;
			}

			this.TextView.Caret.MoveTo(new Microsoft.VisualStudio.Text.SnapshotPoint(snapshot.TextSnapshot, span.Start));

			return true;
		}
	}
}
