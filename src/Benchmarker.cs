using System.Diagnostics;
using CSCommonSecrets;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WhisperDragonAvalonia
{
	public static class Benchmarker
	{
		public static int Benchmark(double howLongToRunInMilliseconds, int iterations)
		{
			int returnValue = 0;
			byte[] salt = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
			KeyDerivationFunctionEntry kdfe = new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, salt, iterations, 32, "Does not matter");

			int valueToIncrease = 100000;

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			while (stopwatch.Elapsed.TotalMilliseconds < howLongToRunInMilliseconds)
			{
				kdfe.GeneratePasswordBytes(valueToIncrease.ToString());
				valueToIncrease++;
				returnValue++;

				kdfe.GeneratePasswordBytes(valueToIncrease.ToString());
				valueToIncrease++;
				returnValue++;

				kdfe.GeneratePasswordBytes(valueToIncrease.ToString());
				valueToIncrease++;
				returnValue++;

				kdfe.GeneratePasswordBytes(valueToIncrease.ToString());
				valueToIncrease++;
				returnValue++;

				kdfe.GeneratePasswordBytes(valueToIncrease.ToString());
				valueToIncrease++;
				returnValue++;
			}
			stopwatch.Stop();

			return returnValue;
		}
	}
}