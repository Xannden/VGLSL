using Microsoft.VisualStudio.Text.BraceCompletion;

namespace Xannden.VSGLSL.Formatting.BraceCompletion
{
	internal sealed class GLSLBraceCompletionContext : IBraceCompletionContext
	{
		public bool AllowOverType(IBraceCompletionSession session)
		{
			return true;
		}

		public void Finish(IBraceCompletionSession session)
		{
		}

		public void OnReturn(IBraceCompletionSession session)
		{
		}

		public void Start(IBraceCompletionSession session)
		{
		}
	}
}
