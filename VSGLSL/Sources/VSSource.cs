using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Text;

namespace Xannden.VSGLSL.Sources
{
	internal sealed class VSSource : Source
	{
		private AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		private bool isParsing = false;
		private GLSLLexer lexer = new GLSLLexer();
		private object lockObject = new object();

		private VSSource(ITextBuffer buffer, ErrorHandler errorHandler) : base(errorHandler)
		{
			this.TextBuffer = buffer;

			this.TextBuffer.Changed += this.Buffer_Changed;
		}

		internal event EventHandler DoneParsing;

		public override Snapshot CurrentSnapshot => new VSSnapshot(this, this.TextBuffer.CurrentSnapshot);

		internal ITextBuffer TextBuffer { get; }

		public static VSSource GetOrCreate(ITextBuffer buffer)
		{
			ErrorHandler errors = buffer.Properties.GetOrCreateSingletonProperty(() => new ErrorHandler());

			VSSource source = buffer.Properties.GetOrCreateSingletonProperty(() => new VSSource(buffer, errors));

			if (source.Tree == null)
			{
				source.Buffer_Changed(null, null);
			}

			return source;
		}

		public void Parse()
		{
			this.Buffer_Changed(null, null);
		}

		private void Buffer_Changed(object sender, TextContentChangedEventArgs e)
		{
			this.autoResetEvent.Set();

			lock (this.lockObject)
			{
				if (!this.isParsing)
				{
					this.isParsing = true;
					ThreadPool.QueueUserWorkItem(this.Parse);
				}
			}
		}

		private void Parse(object obj)
		{
			while (this.autoResetEvent.WaitOne(200))
			{
			}

			this.ErrorHandler.ClearErrors();

			Snapshot snapshot = this.CurrentSnapshot;

			LinkedList<Token> tokens = this.lexer.Run(snapshot);

			this.CommentSpans = this.lexer.GetCommentSpans();

			this.Tree = this.Parser.Run(this.CurrentSnapshot, tokens);

			this.Settings.SetPreprocessors(snapshot, this.Parser.GetPreprocessors());

			this.DoneParsing?.Invoke(this, new EventArgs());

			this.isParsing = false;
		}
	}
}