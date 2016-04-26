using System.Windows.Documents;
using Microsoft.VisualStudio.Text.Classification;

namespace Xannden.VSGLSL.Extensions
{
	internal static class StringExtensions
	{
		public static Run ToRun(this string text, IClassificationFormatMap formatMap, IClassificationType classificationType)
		{
			Run run = new Run(text);
			run.SetTextProperties(formatMap.GetTextProperties(classificationType));

			return run;
		}
	}
}
