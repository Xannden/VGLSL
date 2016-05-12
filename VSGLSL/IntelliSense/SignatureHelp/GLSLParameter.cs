using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Xannden.GLSL.Semantics.Definitions.Base;

namespace Xannden.VSGLSL.Intellisense.SignatureHelp
{
	internal sealed class GLSLParameter : IParameter
	{
		public GLSLParameter(ISignature signature, ParameterDefinition parameter, Span locus)
		{
			this.Signature = signature;
			this.Name = parameter.Name.Text;
			this.Locus = locus;
		}

		public string Documentation { get; }

		public Span Locus { get; }

		public string Name { get; }

		public Span PrettyPrintedLocus { get; }

		public ISignature Signature { get; }
	}
}
