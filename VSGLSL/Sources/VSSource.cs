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
	internal sealed class VSSource : Source, IDisposable
	{
		private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		private readonly GLSLLexer lexer = new GLSLLexer();
		private readonly object lockObject = new object();
		private bool isParsing = false;

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

		public void Dispose()
		{
			this.Dispose(true);
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

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.autoResetEvent.Dispose();
			}
		}

		private void Parse(object obj)
		{
			while (this.autoResetEvent.WaitOne(200))
#pragma warning disable S108 // Nested blocks of code should not be left empty
			{
			}
#pragma warning restore S108 // Nested blocks of code should not be left empty

			this.ErrorHandler.ClearErrors();

			Snapshot snapshot = this.CurrentSnapshot;

			LinkedList<Token> tokens = this.lexer.Run(snapshot);

			this.CommentSpans = this.lexer.CommentSpans;

			this.Tree = this.Parser.Run(snapshot, tokens);

			this.Settings.SetPreprocessors(this.Parser.Preprocessors);

			this.DoneParsing?.Invoke(this, new EventArgs());

			this.isParsing = false;
		}
	}
}