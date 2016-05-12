namespace Xannden.GLSL.Text
{
	public class ColoredString
	{
		private ColoredString(string text, ColorType type)
		{
			this.Text = text;
			this.Type = type;
		}

		public string Text { get; }

		public ColorType Type { get; }

		public static bool operator ==(ColoredString left, ColoredString right)
		{
			return left?.Text == right?.Text && left?.Type == right?.Type;
		}

		public static bool operator !=(ColoredString left, ColoredString right)
		{
			return left?.Text != right?.Text || left?.Type != right?.Type;
		}

		public static ColoredString Create(string text, ColorType type)
		{
			return new ColoredString(text, type);
		}

		public override bool Equals(object obj)
		{
			ColoredString other = obj as ColoredString;

			if (other == null)
			{
				return false;
			}

			return this == other;
		}

		public override int GetHashCode()
		{
			return this.Text.GetHashCode();
		}

		public override string ToString()
		{
			return this.Text;
		}
	}
}