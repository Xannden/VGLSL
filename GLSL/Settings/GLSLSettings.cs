using System.Collections.Generic;
using Xannden.GLSL.Extensions;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Settings
{
	public sealed class GLSLSettings
	{
		private IReadOnlyList<IfPreprocessor> Preprocessors { get; set; } = new List<IfPreprocessor>();

		public IfPreprocessor GetPreprocessor(Snapshot snapshot, int position) => this.Preprocessors.Find(preprocessor => preprocessor.Contains(snapshot, position));

		public bool GetPreprocessorValue(Snapshot snapshot, int position, bool defaultValue)
		{
			for (int i = 0; i < this.Preprocessors.Count; i++)
			{
				if (this.Preprocessors[i].Contains(snapshot, position))
				{
					return this.Preprocessors[i].GetPreprocessor(snapshot, position).Value;
				}
			}

			return defaultValue;
		}

		public bool GetPreprocessorValue(Snapshot snapshot, int position) => this.GetPreprocessorValue(snapshot, position, false);

		public bool IsPreprocessor(Snapshot snapshot, int position) => this.Preprocessors.Contains(preprocessor => preprocessor.Contains(snapshot, position));

		public void SetPreprocessors(IReadOnlyList<IfPreprocessor> preprocessors) => this.Preprocessors = preprocessors;
	}
}