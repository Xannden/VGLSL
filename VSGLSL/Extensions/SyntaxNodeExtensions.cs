using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text.Classification;
using Xannden.GLSL.Syntax.Tree;

namespace Xannden.VSGLSL.Extensions
{
	internal static class SyntaxNodeExtensions
	{
		public static List<Run> ToRuns(this SyntaxToken token, IClassificationFormatMap formatMap, string tokenType, IClassificationTypeRegistryService typeRegistry)
		{
			List<Run> runs = new List<Run>();

			if (token.HasLeadingTrivia)
			{
				runs.Add(token.LeadingTrivia.GetTextAndReplaceNewLines(string.Empty).ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
			}

			runs.Add(token.Text.ToRun(formatMap, typeRegistry.GetClassificationType(tokenType)));

			if (token.HasTrailingTrivia)
			{
				runs.Add(token.TrailingTrivia.GetTextAndReplaceNewLines(" ").ToRun(formatMap, typeRegistry.GetClassificationType(PredefinedClassificationTypeNames.WhiteSpace)));
			}

			return runs;
		}
	}
}
