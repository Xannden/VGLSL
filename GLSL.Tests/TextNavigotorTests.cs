﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xannden.GLSL.Lexing;
using Xannden.GLSL.Test.Text;
using Xannden.GLSL.Text;

namespace Xannden.GLSL.Tests
{
	[TestClass]
	public sealed class TextNavigatorTests
	{

		[TestMethod]
		public void AdvancePastEnd()
		{
			TextSource source = new TextSource("test");
			TextNavigator nav = new TextNavigator(source.CurrentSnapshot);

			nav.Advance();
			nav.Advance();
			nav.Advance();
			nav.Advance();

			Assert.AreEqual(TextNavigator.EndCharacter, nav.PeekChar());
		}

		[TestMethod]
		public void AdvanceToNextLine()
		{
			string lines = "one\r\ntwo\r\n";

			MultiLineTextSource source = MultiLineTextSource.FromString(lines);

			TextNavigator nav = new TextNavigator(source.CurrentSnapshot);

			int index = 0;

			while (nav.PeekChar() != TextNavigator.EndCharacter)
			{
				Assert.AreEqual(lines[index++], nav.PeekChar());
				nav.Advance();
			}

			Assert.AreEqual(TextNavigator.EndCharacter, nav.PeekChar());
		}

		[TestMethod]
		public void FromString()
		{
			string lines = "one\r\ntwo\r\nthree\r\n";

			MultiLineTextSource source = MultiLineTextSource.FromString(lines);

			Assert.AreEqual(3, source.CurrentSnapshot.LineCount);
			Assert.AreEqual("one\r\n", source.CurrentSnapshot.GetLineFromLineNumber(0).Text);
			Assert.AreEqual("two\r\n", source.CurrentSnapshot.GetLineFromLineNumber(1).Text);
			Assert.AreEqual("three\r\n", source.CurrentSnapshot.GetLineFromLineNumber(2).Text);
		}

		[TestMethod]
		public void PeakAhead()
		{
			TextSource source = new TextSource("test");
			TextNavigator nav = new TextNavigator(source.CurrentSnapshot);

			Assert.AreEqual('t', nav.PeekChar(0));
			Assert.AreEqual('e', nav.PeekChar(1));
			Assert.AreEqual('s', nav.PeekChar(2));
			Assert.AreEqual('t', nav.PeekChar(3));
		}

		[TestMethod]
		public void PeekAheadToNextLine()
		{
			string lines = "one\r\ntwo\r\nthree\r\n";

			MultiLineTextSource source = MultiLineTextSource.FromString(lines);

			TextNavigator nav = new TextNavigator(source.CurrentSnapshot);

			int index = 0;

			while (nav.PeekChar() != TextNavigator.EndCharacter)
			{
				Assert.AreEqual(lines[index++], nav.PeekChar());
				nav.Advance();
			}
		}

		[TestMethod]
		public void ShortSpan()
		{
			string lines = "one\r\ntwo\r\nthree\r\n";

			MultiLineTextSource source = MultiLineTextSource.FromString(lines);

			TextNavigator nav = new TextNavigator(source.CurrentSnapshot, Span.Create(5, 12));

			for (int i = 5; i <= 12; i++)
			{
				Assert.AreEqual(lines[i], nav.PeekChar());

				nav.Advance();
			}

			Assert.AreEqual(TextNavigator.EndCharacter, nav.PeekChar());
		}
	}

}
