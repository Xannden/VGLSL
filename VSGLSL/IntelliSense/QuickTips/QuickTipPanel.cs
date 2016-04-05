using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Xannden.VSGLSL.IntelliSense.QuickTips
{
	internal class QuickTipPanel : StackPanel
	{
		public QuickTipPanel(FrameworkElement glyph, FrameworkElement mainDiscription, FrameworkElement documentation)
		{
			Border glyphBorder = null;
			if (glyph != null)
			{
				glyph.Margin = new Thickness(1, 1, 3, 1);
				glyphBorder = new Border
				{
					BorderThickness = new Thickness(0),
					BorderBrush = Brushes.Transparent,
					VerticalAlignment = VerticalAlignment.Top,
					Child = glyph
				};
			}

			mainDiscription.Margin = new Thickness(1);
			Border mainDiscriptionBorder = new Border
			{
				BorderThickness = new Thickness(0),
				BorderBrush = Brushes.Transparent,
				VerticalAlignment = VerticalAlignment.Center,
				Child = mainDiscription
			};

			DockPanel glyphAndMainDiscriptionDock = new DockPanel
			{
				LastChildFill = true,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				Background = Brushes.Transparent
			};

			if (glyphBorder != null)
			{
				glyphAndMainDiscriptionDock.Children.Add(glyphBorder);
			}

			glyphAndMainDiscriptionDock.Children.Add(mainDiscriptionBorder);

			this.Children.Add(glyphAndMainDiscriptionDock);

			if (documentation != null)
			{
				this.Children.Add(documentation);
			}
		}
	}
}
