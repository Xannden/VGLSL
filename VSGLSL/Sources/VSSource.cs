using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Syntax.Tokens;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Data;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Sources
{
	internal sealed class VSSource : Source, IDisposable
	{
		private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		private readonly GLSLLexer lexer = new GLSLLexer();
		private readonly object lockObject = new object();
		private bool isParsing = false;
		private bool hasChanged = false;

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
			string filePath = buffer.GetFilePath();

			VSSource source = buffer.Properties.GetOrCreateSingletonProperty(() => new VSSource(buffer, buffer.GetFilePath()));

			if (filePath.EndsWith(GLSLConstants.VertexExtension, StringComparison.Ordinal))
			{
				source.Type = ShaderType.Vertex;
			}
			else if (filePath.EndsWith(GLSLConstants.FragmentExtension, StringComparison.Ordinal))
			{
				source.Type = ShaderType.Fragment;
			}
			else if (filePath.EndsWith(GLSLConstants.GeometryExtension, StringComparison.Ordinal))
			{
				source.Type = ShaderType.Geometry;
			}
			else if (filePath.EndsWith(GLSLConstants.ComputeExtension, StringComparison.Ordinal))
			{
				source.Type = ShaderType.Compute;
			}
			else if (filePath.EndsWith(GLSLConstants.TessellationControlExtension, StringComparison.Ordinal))
			{
				source.Type = ShaderType.TessellationControl;
			}
			else if (filePath.EndsWith(GLSLConstants.TessellationEvaluationExtension, StringComparison.Ordinal))
			{
				source.Type = ShaderType.TessellationEvaluation;
			}

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
				else
				{
					this.hasChanged = true;
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
			while (true)
			{
				this.hasChanged = false;

				while (this.autoResetEvent.WaitOne(200))
				{
				}

				Snapshot snapshot = this.CurrentSnapshot;

				LinkedList<Token> tokens = this.lexer.Run(snapshot);

				if (this.hasChanged)
				{
					continue;
				}

				this.CommentSpans = this.lexer.CommentSpans;

				this.Tree = this.Parser.Run(snapshot, tokens);

				if (this.hasChanged)
				{
					continue;
				}

				this.Settings.SetPreprocessors(this.Parser.Preprocessors);

				if (this.hasChanged)
				{
					continue;
				}

				this.DoneParsing?.Invoke(this, new EventArgs());

				lock (this.lockObject)
				{
					if (this.hasChanged)
					{
						continue;
					}

					this.isParsing = false;

					break;
				}
			}
		}
	}
}