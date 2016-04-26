﻿using System.Collections.Generic;
using Xannden.GLSL.Errors;
using Xannden.GLSL.Parsing;
using Xannden.GLSL.Settings;
using Xannden.GLSL.Syntax.Tree;

namespace Xannden.GLSL.Text
{
	public abstract class Source
	{
		private IReadOnlyList<TrackingSpan> commentSpans = new List<TrackingSpan>();
		private object lockObject = new object();
		private SyntaxTree tree;

		protected Source(ErrorHandler errorHandler)
		{
			this.ErrorHandler = errorHandler;

			this.Parser = new GLSLParser(this.ErrorHandler, this.Settings);
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

		public ErrorHandler ErrorHandler { get; }

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