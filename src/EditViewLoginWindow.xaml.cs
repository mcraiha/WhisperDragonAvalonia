using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace WhisperDragonAvalonia
{
	/// <summary>
	/// Interaction logic for EditViewLoginWindow.xaml
	/// </summary>
	public class EditViewLoginWindow : Window
	{
		public EditViewLoginWindow()
		{
			
		}

		public EditViewLoginWindow(LoginSimplified current, List<string> keyIdentifiers, Action<LoginSimplified, bool /* Was Security Modified */, string /* Key identifier */> editLogin)
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new EditViewLoginViewModel(current, keyIdentifiers, EditClose, CancelClose, editLogin);
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