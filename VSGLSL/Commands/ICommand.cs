using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Xannden.VSGLSL.Commands
{
	internal interface ICommand
	{
		void Create(IVsTextView textViewAdapter, ITextView textView);
	}
}
