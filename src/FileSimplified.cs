using System;
using System.Collections.Generic;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	public sealed class FileSimplified
	{
		// Non visible
		public int zeroBasedIndexNumber { get; set; }

		// Visible elements

		public bool IsSecure { get; set; }

		public string Filename { get; set; }

		public string Filesize { get; set; }

		public string Filetype { get; set; }

		public string CreationTime { get; set; }

		public string ModificationTime { get; set; }

		public static List<FileSimplified> TurnIntoUICompatible(List<FileEntry> files, List<FileEntrySecret> fileSecrets, Dictionary<string, byte[]> derivedPasswords, SettingsData settingsData)
		{
			List<FileSimplified> returnList = new List<FileSimplified>();

			if (files != null && files.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (FileEntry file in files)
				{
					returnList.Add(new FileSimplified() {
						zeroBasedIndexNumber = runningIndexNumber,
						IsSecure = false,
						Filename = UITextFormat.FormatTextWithFilter(file.GetFilename(), settingsData.FileFilenameShowMode),
						Filesize = "N/A",
						Filetype = "N/A",
						CreationTime = file.GetCreationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = file.GetModificationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			if (fileSecrets != null && fileSecrets.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (FileEntrySecret fileSecret in fileSecrets)
				{
					byte[] derivedPassword = derivedPasswords[fileSecret.GetKeyIdentifier()];
					returnList.Add(new FileSimplified() {
						zeroBasedIndexNumber = runningIndexNumber,
						IsSecure = true,
						Filename = UITextFormat.FormatTextWithFilter(fileSecret.GetFilename(derivedPassword), settingsData.FileFilenameShowMode),
						Filesize = "N/A",
						Filetype = "N/A",
						CreationTime = fileSecret.GetCreationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = fileSecret.GetModificationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			return returnList;
		}
	}
}