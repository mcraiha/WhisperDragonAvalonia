using System;
using System.Collections.Generic;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	public sealed class LoginSimplified
	{
		// Non visible
		public int zeroBasedIndexNumber { get; set; }

		// Visible elements

		public bool IsSecure { get; set; }
		public string Title { get; set; }
		public string URL { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Notes { get; set; }
		public byte[] Icon { get; set; }
		public string Category { get; set; }
		public string Tags { get; set; }
		public string CreationTime { get; set; }
		public string ModificationTime { get; set; }

		public static List<LoginSimplified> TurnIntoUICompatible(List<LoginInformation> loginInformations, List<LoginInformationSecret> loginInformationSecrets, Dictionary<string, byte[]> derivedPasswords, SettingsData settingsData)
		{
			List<LoginSimplified> returnList = new List<LoginSimplified>();

			if (loginInformations != null && loginInformations.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (LoginInformation loginInformation in loginInformations)
				{
					returnList.Add(new LoginSimplified() {
						zeroBasedIndexNumber = runningIndexNumber,
						IsSecure = false,
						Title = UITextFormat.FormatTextWithFilter(loginInformation.GetTitle(), settingsData.LoginTitleShowMode),
						URL = UITextFormat.FormatTextWithFilter(loginInformation.GetURL(), settingsData.LoginUrlShowMode),
						Email = UITextFormat.FormatTextWithFilter(loginInformation.GetEmail(), settingsData.LoginEmailShowMode),
						Username = UITextFormat.FormatTextWithFilter(loginInformation.GetUsername(), settingsData.LoginUsernameShowMode),
						Password = UITextFormat.FormatTextWithFilter(loginInformation.GetPassword(), settingsData.LoginPasswordShowMode),
						Notes = loginInformation.GetNotes(),
						Icon = loginInformation.GetIcon(),
						Category = UITextFormat.FormatTextWithFilter(loginInformation.GetCategory(), settingsData.LoginCategoryShowMode),
						Tags = loginInformation.GetTags(),
						CreationTime = loginInformation.GetCreationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = loginInformation.GetModificationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			if (loginInformationSecrets != null && loginInformationSecrets.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (LoginInformationSecret loginInformationSecret in loginInformationSecrets)
				{
					byte[] derivedPassword = derivedPasswords[loginInformationSecret.GetKeyIdentifier()];
					returnList.Add(new LoginSimplified() {
						zeroBasedIndexNumber = runningIndexNumber,
						IsSecure = true,
						Title = UITextFormat.FormatTextWithFilter(loginInformationSecret.GetTitle(derivedPassword), settingsData.LoginTitleShowMode),
						URL = UITextFormat.FormatTextWithFilter(loginInformationSecret.GetURL(derivedPassword), settingsData.LoginUrlShowMode),
						Email = UITextFormat.FormatTextWithFilter(loginInformationSecret.GetEmail(derivedPassword), settingsData.LoginEmailShowMode),
						Username = UITextFormat.FormatTextWithFilter(loginInformationSecret.GetUsername(derivedPassword), settingsData.LoginUsernameShowMode),
						Password = UITextFormat.FormatTextWithFilter(loginInformationSecret.GetPassword(derivedPassword), settingsData.LoginPasswordShowMode),
						Notes = loginInformationSecret.GetNotes(derivedPassword),
						Icon = loginInformationSecret.GetIcon(derivedPassword),
						Category = UITextFormat.FormatTextWithFilter(loginInformationSecret.GetCategory(derivedPassword), settingsData.LoginCategoryShowMode),
						Tags = loginInformationSecret.GetTags(derivedPassword),
						CreationTime = loginInformationSecret.GetCreationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = loginInformationSecret.GetModificationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			return returnList;
		}

		public static LoginSimplified TurnIntoEditable(LoginInformation loginInformation, int zeroBasedIndexNumber)
		{
			return new LoginSimplified() {
						zeroBasedIndexNumber = zeroBasedIndexNumber,
						IsSecure = false,
						Title = loginInformation.GetTitle(),
						URL = loginInformation.GetURL(),
						Email = loginInformation.GetEmail(),
						Username = loginInformation.GetUsername(),
						Password = loginInformation.GetPassword(),
						Notes = loginInformation.GetNotes(),
						Icon = loginInformation.GetIcon(),
						Category = loginInformation.GetCategory(),
						Tags = loginInformation.GetTags(),
						CreationTime = loginInformation.GetCreationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = loginInformation.GetModificationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					};
		}

		public static LoginSimplified TurnIntoEditable(LoginInformationSecret loginInformationSecret, byte[] derivedPassword, int zeroBasedIndexNumber)
		{
			return new LoginSimplified() {
						zeroBasedIndexNumber = zeroBasedIndexNumber,
						IsSecure = true,
						Title = loginInformationSecret.GetTitle(derivedPassword),
						URL = loginInformationSecret.GetURL(derivedPassword),
						Email = loginInformationSecret.GetEmail(derivedPassword),
						Username = loginInformationSecret.GetUsername(derivedPassword),
						Password = loginInformationSecret.GetPassword(derivedPassword),
						Notes = loginInformationSecret.GetNotes(derivedPassword),
						Icon = loginInformationSecret.GetIcon(derivedPassword),
						Category = loginInformationSecret.GetCategory(derivedPassword),
						Tags = loginInformationSecret.GetTags(derivedPassword),
						CreationTime = loginInformationSecret.GetCreationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = loginInformationSecret.GetModificationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					};
		}
	}
}