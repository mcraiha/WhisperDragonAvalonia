using System;
using System.Text;
using System.Numerics;
using System.Collections.Generic;

namespace WhisperDragonAvalonia
{
	public enum PasswordSecurityLevel
	{
		// These aren't exact science so do NOT assume anything
		Very_Weak = 0,
		Weak,
		Reasonable,
		Strong,
		Very_Strong,
		Unknown,
	}

	public static class EntropyCalcs
	{
		private static readonly List<(PasswordSecurityLevel level, int min, int max)> limitsInBits = new List<(PasswordSecurityLevel level, int min, int max)>()
		{
			(PasswordSecurityLevel.Very_Weak, -1, 27),
			(PasswordSecurityLevel.Weak, 28, 35),
			(PasswordSecurityLevel.Reasonable, 36, 59),
			(PasswordSecurityLevel.Strong, 60, 127),
			(PasswordSecurityLevel.Very_Strong, 128, int.MaxValue),
		};

		/// <summary>
		/// Calculate password entropy
		/// </summary>
		/// <remarks>
		/// Logic is from https://www.pleacher.com/mp/mlessons/algebra/entropy.html
		/// </remarks>
		/// <param name="password">Password for calculations</param>
		/// <returns>Returns password entropy in bits</returns>
		public static int CalcutePasswordEntropy(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				return 0;
			}

			int charsInPassword = password.Length;
			int totalPool = SumUpTotalPool(password);

			BigInteger beforeLog = BigInteger.Pow(totalPool, charsInPassword);
			return (int) BigInteger.Log(beforeLog);
		}

		private static int SumUpTotalPool(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				return 0;
			}

			bool includesLowercase = false;
			bool includesUppercase = false;
			bool includesDigits = false;
			bool includesSymbols = false;

			foreach (char c in password)
			{
				if (char.IsLower(c))
				{
					includesLowercase = true;
				}
				else if (char.IsUpper(c))
				{
					includesUppercase = true;
				}
				else if (char.IsDigit(c))
				{
					includesDigits = true;
				}
				else if (char.IsSymbol(c))
				{
					includesSymbols = true;
				}
			}

			int returnValue = 0;

			if (includesLowercase)
			{
				returnValue += 26;
			}

			if (includesUppercase)
			{
				returnValue += 26;
			}

			if (includesDigits)
			{
				returnValue += 10;
			}

			if (includesSymbols)
			{
				returnValue += 30;
			}

			return returnValue;
		}

		public static PasswordSecurityLevel GetPasswordSecurityLevel(int passwordEntropyInBits)
		{
			foreach ((PasswordSecurityLevel level, int min, int max) in limitsInBits)
			{
				if (passwordEntropyInBits >= min && passwordEntropyInBits <= max)
				{
					return level;
				}
			}

			return PasswordSecurityLevel.Unknown;
		}

		public static double ShannonEntropy(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return 0;
			}

			return ShannonEntropy(Encoding.UTF8.GetBytes(input));
		}

		public static double ShannonEntropy(byte[] byteArray)
		{
			var dictionary = new Dictionary<byte, int>();

			foreach (byte b in byteArray)
			{
				if (!dictionary.ContainsKey(b))
				{
					dictionary.Add(b, 1);
				}
				else
				{
					dictionary[b] += 1;
				}
			}

			double result = 0.0;
			int length = byteArray.Length;
			
			foreach (var item in dictionary)
			{
				var frequency = (double)item.Value / length;
				result -= frequency * (Math.Log(frequency) / Math.Log(2));
			}

			return result;
		}
	}
}