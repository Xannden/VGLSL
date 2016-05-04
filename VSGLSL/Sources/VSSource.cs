using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Sources
{
	internal sealed class VSSource : Source, IDisposable
	{
		private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		private readonly GLSLLexer lexer = new GLSLLexer();
		private readonly object lockObject = new object();
		private bool isParsing = false;

		private VSSource(ITextBuffer buffer, string fileName) : base(fileName)
		{
			this.TextBuffer = buffer;

			this.TextBuffer.Changed += this.Buffer_Changed;
		}

		internal event EventHandler DoneParsing;

		public override Snapshot CurrentSnapshot => new VSSnapshot(this, this.TextBuffer.CurrentSnapshot);

		internal ITextBuffer TextBuffer { get; }

		public static VSSource GetOrCreate(ITextBuffer buffer)
		{
			VSSource source = buffer.Properties.GetOrCreateSingletonProperty(() => new VSSource(buffer, buffer.GetFileName()));

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
#pragma warning disable S108 // Nested blocks of code should not be left empty
			while (this.autoResetEvent.WaitOne(200))
			{
			}
#pragma warning restore S108 // Nested blocks of code should not be left empty

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