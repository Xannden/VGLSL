using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Xannden.GLSL.BuiltIn;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Syntax;
using Xannden.GLSL.Syntax.Tree;
using Xannden.GLSL.Syntax.Tree.Syntax;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Intellisense.SignatureHelp
{
	internal sealed class GLSLSignatureHelpSource : ISignatureHelpSource
	{
		private readonly Source source;

		public GLSLSignatureHelpSource(Source source)
		{
			this.source = source;
		}

		public void AugmentSignatureHelpSession(ISignatureHelpSession session, IList<ISignature> signatures)
		{
			Snapshot snapshot = this.source.CurrentSnapshot;
			SyntaxTree tree = this.source.Tree;

			int triggerPosition = session.GetTriggerPoint(snapshot.ToITextSnapshot()).Value.Position;
			SyntaxNode node = tree.GetNodeFromPosition(snapshot, triggerPosition);
			IdentifierSyntax identifier = node as IdentifierSyntax;

			if (identifier?.Definition?.Kind != DefinitionKind.Function)
			{
				FunctionCallSyntax functionCall = null;

				foreach (SyntaxNode decendent in node.DescendantsAndSelf)
				{
					if (decendent.SyntaxType == SyntaxType.FunctionCall)
					{
						functionCall = decendent as FunctionCallSyntax;
						break;
					}
				}

				if (functionCall == null)
				{
					foreach (SyntaxNode ancestor in node.AncestorsAndSelf)
					{
						if (ancestor.SyntaxType == SyntaxType.FunctionCall)
						{
							functionCall = ancestor as FunctionCallSyntax;
							break;
						}
					}
				}

				if (functionCall?.Identifier?.Definition != null && functionCall.Span.GetSpan(snapshot).Contains(triggerPosition))
				{
					TrackingSpan span = null;

					if (functionCall.RightParentheses == null)
					{
						SourceLine line = snapshot.GetLineFromPosition(functionCall.Span.GetSpan(snapshot).Start);

						span = snapshot.CreateTrackingSpan(Span.Create(functionCall.Span.GetSpan(snapshot).Start, line.Span.End));
					}
					else
					{
						span = functionCall.Span;
					}

					for (int i = 0; i < functionCall.Identifier.Definition.Overloads.Count; i++)
					{
						signatures.Add(new GLSLSignature(functionCall.Identifier.Definition.Overloads[i], span, snapshot, session.TextView, session));
					}
				}
			}
			else if (identifier?.Definition != null)
			{
				TrackingSpan span = null;

				if (node?.Parent.SyntaxType == SyntaxType.FunctionCall)
				{
					span = node.Parent.Span;
				}
				else
				{
					span = node.Span;
				}

				for (int i = 0; i < identifier.Definition.Overloads.Count; i++)
				{
					signatures.Add(new GLSLSignature(identifier.Definition.Overloads[i], span, snapshot, session.TextView, session));
				}

				if (BuiltInData.Instance.Definitions.ContainsKey(identifier.Definition.Name.Text))
				{
					for (int i = 0; i < BuiltInData.Instance.Definitions[identifier.Definition.Name.Text].Count; i++)
					{
						signatures.Add(new GLSLSignature(BuiltInData.Instance.Definitions[identifier.Definition.Name.Text][i], span, snapshot, session.TextView, session));
					}
				}
			}
		}

		public void Dispose()
		{
		}

		public ISignature GetBestMatch(ISignatureHelpSession session)
		{
			return null;
		}
	}
}
