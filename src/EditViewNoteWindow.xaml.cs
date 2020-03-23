using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace WhisperDragonAvalonia
{
	/// <summary>
	/// Interaction logic for EditViewNoteWindow.xaml
	/// </summary>
	public class EditViewNoteWindow : Window
	{
		public EditViewNoteWindow()
		{
			
		}

		public EditViewNoteWindow(NoteSimplified current, List<string> keyIdentifiers, Action<NoteSimplified, bool /* Was Security Modified */, string /* Key identifier */> editNote)
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new EditViewNoteViewModel(current, keyIdentifiers, EditClose, CancelClose, editNote);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void EditClose()
		{
			this.Close();
		}

		private void CancelClose()
		{
			this.Close();
		}
	}
}