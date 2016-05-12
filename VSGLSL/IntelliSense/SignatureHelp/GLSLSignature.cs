using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Semantics;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Intellisense.SignatureHelp
{
	internal sealed class GLSLSignature : ISignature
	{
		private readonly ITextView textView;
		private IParameter currentParameter;
		private ISignatureHelpSession session;

		public GLSLSignature(Definition definition, TrackingSpan trackingSpan, Snapshot snapshot, ITextView textView, ISignatureHelpSession session)
		{
			GLSL.Text.Span span = trackingSpan.GetSpan(snapshot);

			this.ApplicableToSpan = snapshot.CreateTrackingSpan(GLSL.Text.Span.Create(span.Start, span.End - 1)).ToITrackingSpan();
			this.Content = definition.ToString();
			this.Documentation = definition.Documentation;
			this.Parameters = new ReadOnlyCollection<IParameter>(definition.GetParameters(this, snapshot));
			this.textView = textView;
			this.session = session;

			this.textView.TextBuffer.Changed += this.TextBufferChanged;
			this.textView.Caret.PositionChanged += this.CaretPositionChanged;

			this.session.Dismissed += this.OnDismissed;

			this.ComputeCurrentParamter(this.textView.TextBuffer.CurrentSnapshot);
		}

		public event EventHandler<CurrentParameterChangedEventArgs> CurrentParameterChanged;

		public ITrackingSpan ApplicableToSpan { get; }

		public string Content { get; }

		public IParameter CurrentParameter
		{
			get
			{
				return this.currentParameter;
			}

			set
			{
				IParameter old = this.currentParameter;
				this.currentParameter = value;

				this.CurrentParameterChanged?.Invoke(this, new CurrentParameterChangedEventArgs(old, this.currentParameter));
			}
		}

		public string Documentation { get; }

		public ReadOnlyCollection<IParameter> Parameters { get; }

		public string PrettyPrintedContent { get; }

		private void OnDismissed(object sender, EventArgs e)
		{
			this.session.Dismissed -= this.OnDismissed;
			this.textView.TextBuffer.Changed -= this.TextBufferChanged;
			this.textView.Caret.PositionChanged -= this.CaretPositionChanged;
		}

		private void TextBufferChanged(object sender, TextContentChangedEventArgs e)
		{
			this.ComputeCurrentParamter(e.Before);
		}

		private void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
		{
			this.ComputeCurrentParamter(e.TextView.TextBuffer.CurrentSnapshot);
		}

		private void ComputeCurrentParamter(ITextSnapshot snapshot)
		{
			if (this.Parameters.Count == 0)
			{
				this.currentParameter = null;
				return;
			}

			string text = snapshot.GetText(this.ApplicableToSpan.GetStartPoint(snapshot).Position, this.ApplicableToSpan.GetSpan(snapshot).Length + 1);

			int parameterIndex = 0;
			int cursorPosition = this.textView.Caret.Position.BufferPosition.Position - this.ApplicableToSpan.GetSpan(snapshot).Start.Position;
			int parenthesisDepth = 0;

			for (int i = 0; i < text.Length; i++)
			{
				if (i >= cursorPosition)
				{
					break;
				}

				switch (text[i])
				{
					case ',':
						if (parenthesisDepth <= 1)
						{
							parameterIndex++;
						}

						break;
					case '(':
						parenthesisDepth++;
						break;
					case ')':
						parenthesisDepth--;
						break;
				}
			}

			if (this.Parameters.Count <= parameterIndex)
			{
				this.currentParameter = this.Parameters.Last();
			}
			else
			{
				this.CurrentParameter = this.Parameters[parameterIndex];
			}
		}
	}
}