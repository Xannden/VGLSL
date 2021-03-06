﻿using System;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Text;
using Xannden.VSGLSL.Extensions;

namespace Xannden.VSGLSL.Sources
{
	internal sealed class VSSnapshot : Snapshot
	{
		public VSSnapshot(Source source, ITextSnapshot snapshot) : base(source)
		{
			this.TextSnapshot = snapshot;
		}

		public override int Length => this.TextSnapshot.Length;

		public override int LineCount => this.TextSnapshot.LineCount;

		public ITextSnapshot TextSnapshot { get; }

		public override TrackingSpan CreateTrackingSpan(GLSL.Text.Span span)
		{
			if (span == null)
			{
				throw new ArgumentNullException(nameof(span));
			}

			return new VSTrackingSpan(this.TextSnapshot.CreateTrackingSpan(span.ToVSSpan(), SpanTrackingMode.EdgeExclusive));
		}

		public override TrackingPoint CreateTrackingPoint(int position)
		{
			return new VSTrackingPoint(this.TextSnapshot.CreateTrackingPoint(position, PointTrackingMode.Positive));
		}

		public override SourceLine GetLineFromLineNumber(int lineNumber) => new VSSourceLine(this, this.TextSnapshot.GetLineFromLineNumber(lineNumber));

		public override SourceLine GetLineFromPosition(int position) => new VSSourceLine(this, this.TextSnapshot.GetLineFromPosition(position));

		public override string GetText(int start, int length) => this.TextSnapshot.GetText(start, length);
	}
}