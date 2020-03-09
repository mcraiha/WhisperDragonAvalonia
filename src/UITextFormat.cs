using System;

namespace WhisperDragonAvalonia
{
	// Static class for formatting UI text (because not all text should be visible by default)
	public static class UITextFormat
	{
		private static readonly char passwordChar = '\u2022';

		private static readonly Random rng = new Random(Seed: 2047);

		public static string FormatTextWithFilter(string input, ShowMode showMode)
		{
			if (showMode == ShowMode.ShowFull)
			{
				return input;
			}
			else if (showMode == ShowMode.ShowFirstFour)
			{
				if (input.Length < 5)
				{
					return input;
				}
				else
				{
					return input.Substring(0, 4);
				}
			}
			else if (showMode == ShowMode.ShowFirst)
			{
				if (input.Length < 2)
				{
					return input;
				}
				else
				{
					return input.Substring(0, 1);
				}
			}
			else if (showMode == ShowMode.HiddenCorrectLength)
			{
				return new string(passwordChar, input.Length);
			}
			else if (showMode == ShowMode.HiddenConstantLenght)
			{
				return new string(passwordChar, 8);
			}
			else if (showMode == ShowMode.HiddenRandomLength)
			{
				return new string(passwordChar, rng.Next(1, 16));
			}

			throw new NotImplementedException("Missing ShowMode");
		}
	}
}