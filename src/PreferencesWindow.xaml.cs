using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace WhisperDragonAvalonia
{
	public class PreferencesWindow : Window
	{
		// TODO: Remove once Avalonia fixes XAML parsing
		public PreferencesWindow()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new PreferencesViewModel(null, null, null, this.CloseCall);
		}

		public PreferencesWindow(SettingsData settingsData, string location, Action<SettingsData> callOnSave)
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new PreferencesViewModel(settingsData, location, callOnSave, this.CloseCall);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void CloseCall()
		{
			this.Close();
		}
	}
}