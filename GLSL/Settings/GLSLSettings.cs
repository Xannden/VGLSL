using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Settings
{
	internal class GLSLSettings
	{
		protected List<IfPreprocessor> Preprocessors { get; set; } = new List<IfPreprocessor>();

		public IfPreprocessor GetPreprocessor(Snapshot snapshot, int position)
		{
			return this.Preprocessors.Find(preprocessor => preprocessor.Contains(snapshot, position));
		}

		public bool GetPreprocessorValue(Snapshot snapshot, int position, bool defaultValue = false)
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

		public bool IsPreprocessor(Snapshot snapshot, int position)
		{
			return this.Preprocessors.Find(preprocessor => preprocessor.Contains(snapshot, position)) != null;
		}

		public void SetPreprocessors(Snapshot snapshot, List<IfPreprocessor> preprocessors)
		{
			this.Preprocessors = preprocessors;
		}
	}
}