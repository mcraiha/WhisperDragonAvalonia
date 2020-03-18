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
	public class AddLoginWindow : Window
	{
		public AddLoginWindow()
		{
			
		}

		public AddLoginWindow(List<string> keyIdentifiers, Action<LoginSimplified, string /* Key identifier */> addLogin)
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new AddLoginViewModel(keyIdentifiers, AddClose, CancelClose, addLogin);
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