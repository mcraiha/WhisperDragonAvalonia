using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	/// <summary>
	/// Interaction logic for SecondaryOpenStepWindow.xaml
	/// </summary>
	public class SecondaryOpenStepWindow  : Window
	{
		private Action<KeyDerivationFunctionEntry, string> positive;

		// TODO: Remove once Avalonia fixes XAML parsing
		public SecondaryOpenStepWindow ()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new SecondaryOpenStepViewModel(null, null, this.CancelClose, this.OpenClose);
		}

		public SecondaryOpenStepWindow (List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries, Action<Dictionary<string, byte[]>> finalizeOpenWithDerivedPasswords)
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new SecondaryOpenStepViewModel(keyDerivationFunctionEntries, finalizeOpenWithDerivedPasswords, this.CancelClose, this.OpenClose);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void OpenClose()
		{
			this.Close();
		}

		private void CancelClose()
		{
			this.Close();
		}
	}
}