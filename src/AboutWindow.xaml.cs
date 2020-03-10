using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WhisperDragonAvalonia
{
	public class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new AboutViewModel(this.OkClose);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		private void OkClose()
		{
			this.Close();
		}
	}
}