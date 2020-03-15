using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	public class CreateCommonSecretsWindow : Window
	{
		private Action<KeyDerivationFunctionEntry, string> positive;

		// TODO: Remove once Avalonia fixes XAML parsing
		public CreateCommonSecretsWindow()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new CreateCommonSecretsViewModel(this.OkClose, this.CancelClose);
		}

		public CreateCommonSecretsWindow(Action<KeyDerivationFunctionEntry, string> onPositive)
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			this.positive = onPositive;
			DataContext = new CreateCommonSecretsViewModel(this.OkClose, this.CancelClose);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void CancelClose()
		{
			this.Close();
		}

		private void OkClose(KeyDerivationFunctionEntry kdfe, string password)
		{
			this.Close();
			this.positive(kdfe, password);
		}
	}
}