using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace WhisperDragonAvalonia
{
	/// <summary>
	/// Interaction logic for AddLoginWindow.xaml
	/// </summary>
	public class AddNoteWindow : Window
	{
		public AddNoteWindow()
		{
			
		}

		public AddNoteWindow(List<string> keyIdentifiers, Action<NoteSimplified, string /* Key identifier */> addNote)
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new AddNoteViewModel(keyIdentifiers, AddClose, CancelClose, addNote);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void AddClose()
		{
			this.Close();
		}

		private void CancelClose()
		{
			this.Close();
		}
	}
}