using System.Windows;
using System.Windows.Documents;
using Microsoft.VisualStudio.Text.Formatting;

namespace Xannden.VSGLSL.Extensions
{
	internal static class DependencyObjectExtensions
	{
		public static void SetTextProperties(this DependencyObject dependencyObject, TextFormattingRunProperties textProperties)
		{
			dependencyObject?.SetValue(TextElement.FontStyleProperty, textProperties.Italic ? FontStyles.Italic : FontStyles.Normal);
			dependencyObject?.SetValue(TextElement.FontWeightProperty, textProperties.Bold ? FontWeights.Bold : FontWeights.Normal);
			dependencyObject?.SetValue(TextElement.BackgroundProperty, textProperties.BackgroundBrush);
			dependencyObject?.SetValue(TextElement.ForegroundProperty, textProperties.ForegroundBrush);
		}
	}
}
