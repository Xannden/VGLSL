using System.Collections.Generic;
using Xannden.GLSL.BuiltIn;
using Xannden.GLSL.Parsing;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Syntax.Tree;

namespace Xannden.GLSL.Text
{
	public abstract class Source
	{
		private readonly object lockObject = new object();
		private IReadOnlyList<TrackingSpan> commentSpans = new List<TrackingSpan>();
		private SyntaxTree tree;

		protected Source(string fileName)
		{
			this.FileName = fileName;

			this.Parser = new GLSLParser(this.Settings);

			BuiltInData.Instance.LoadData();
		}

		public IReadOnlyList<TrackingSpan> CommentSpans
		{
			get
			{
				lock (this.lockObject)
				{
					return this.commentSpans;
				}
			}

			protected set
			{
				lock (this.lockObject)
				{
					this.commentSpans = value;
				}
			}
		}

		public abstract Snapshot CurrentSnapshot { get; }

		public string FileName { get; }

		public ShaderProfile Profile { get; } = ShaderProfile.Core;

		public ShaderType Type { get; protected set; } = ShaderType.Vertex;

		public GLSLSettings Settings { get; } = new GLSLSettings();

		public SyntaxTree Tree
		{
			get
			{
				lock (this.lockObject)
				{
					return this.tree;
				}
			}

			set
			{
				lock (this.lockObject)
				{
					this.tree = value;
				}
			}
		}

		protected GLSLParser Parser { get; }
	}
}