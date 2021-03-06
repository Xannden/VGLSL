﻿using System.Collections.Generic;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Semantics.Definitions.Base
{
	public class MacroDefinition : Definition
	{
		public MacroDefinition(string name, IReadOnlyList<ColoredString> parameters, IReadOnlyList<ColoredString> tokenString, string documentation, Scope scope, TrackingSpan span)
			: base(ColoredString.Create(name, ColorType.Macro), documentation, DefinitionKind.Macro, scope, span)
		{
			this.Parameters = parameters ?? new List<ColoredString>();
			this.TokenString = tokenString ?? new List<ColoredString>();
		}

		public IReadOnlyList<ColoredString> Parameters { get; }

		public IReadOnlyList<ColoredString> TokenString { get; }

		public override bool Equals(Definition definition)
		{
			MacroDefinition other = definition as MacroDefinition;

			if (other == null)
			{
				return false;
			}

			if (this.Name != other.Name)
			{
				return false;
			}

			return true;
		}

		public override IReadOnlyList<ColoredString> GetColoredText()
		{
			List<ColoredString> list = new List<ColoredString>();

			list.Add(this.Name);

			if (this.Parameters.Count > 0)
			{
				list.Add(ColoredString.Create("(", ColorType.Punctuation));

				list.AddRange(this.Parameters);

				list.Add(ColoredString.Create(")", ColorType.Punctuation));
			}

			list.Add(ColoredString.Space);

			list.AddRange(this.TokenString);

			return list;
		}
	}
}
