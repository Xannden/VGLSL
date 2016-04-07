﻿using System.Windows.Documents;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Syntax.Tree;

namespace Xannden.VSGLSL.Extensions
{
	internal static class SyntaxNodeExtensions
	{
		public static Run ToRun(this SyntaxToken token, IClassificationFormatMap formatMap, IClassificationType type, string extraText = "")
		{
			Run run = new Run(token.ToStringWithoutNewLines() + extraText);
			run.SetTextProperties(formatMap.GetTextProperties(type));

			return run;
		}
	}
}