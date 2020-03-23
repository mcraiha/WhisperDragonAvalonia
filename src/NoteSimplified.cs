using System;
using System.Collections.Generic;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	public sealed class NoteSimplified
	{
		// Non visible
		public int zeroBasedIndexNumber { get; set; }

		// Visible elements

		public bool IsSecure { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public string CreationTime { get; set; }

		public string ModificationTime { get; set; }

		public static List<NoteSimplified> TurnIntoUICompatible(List<Note> notes, List<NoteSecret> noteSecrets, Dictionary<string, byte[]> derivedPasswords, SettingsData settingsData)
		{
			List<NoteSimplified> returnList = new List<NoteSimplified>();

			if (notes != null && notes.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (Note note in notes)
				{
					returnList.Add(new NoteSimplified() {
						zeroBasedIndexNumber = runningIndexNumber,
						IsSecure = false,
						Title = UITextFormat.FormatTextWithFilter(note.GetNoteTitle(), settingsData.NoteTitleShowMode),
						Text = UITextFormat.FormatTextWithFilter(note.GetNoteText(), settingsData.NoteTextShowMode),
						CreationTime = note.GetCreationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = note.GetModificationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			if (noteSecrets != null && noteSecrets.Count > 0)
			{
				int runningIndexNumber = 0;
				foreach (NoteSecret noteSecret in noteSecrets)
				{
					byte[] derivedPassword = derivedPasswords[noteSecret.GetKeyIdentifier()];
					returnList.Add(new NoteSimplified() {
						zeroBasedIndexNumber = runningIndexNumber,
						IsSecure = true,
						Title = UITextFormat.FormatTextWithFilter(noteSecret.GetNoteTitle(derivedPassword), settingsData.NoteTitleShowMode),
						Text = UITextFormat.FormatTextWithFilter(noteSecret.GetNoteText(derivedPassword), settingsData.NoteTextShowMode),
						CreationTime = noteSecret.GetCreationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = noteSecret.GetModificationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					});

					runningIndexNumber++;
				}
			}

			return returnList;
		}

		public static NoteSimplified TurnIntoEditable(Note note, int zeroBasedIndexNumber)
		{
			return new NoteSimplified() {
						zeroBasedIndexNumber = zeroBasedIndexNumber,
						IsSecure = false,
						Title = note.GetNoteTitle(),
						Text = note.GetNoteText(),
						CreationTime = note.GetCreationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = note.GetModificationTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					};
		}

		public static NoteSimplified TurnIntoEditable(NoteSecret noteSecret, byte[] derivedPassword, int zeroBasedIndexNumber)
		{
			return new NoteSimplified() {
						zeroBasedIndexNumber = zeroBasedIndexNumber,
						IsSecure = true,
						Title = noteSecret.GetNoteTitle(derivedPassword),
						Text = noteSecret.GetNoteText(derivedPassword),
						CreationTime = noteSecret.GetCreationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
						ModificationTime = noteSecret.GetModificationTime(derivedPassword).ToString("s", System.Globalization.CultureInfo.InvariantCulture),
					};
		}
	}
}
