using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WhisperDragonAvalonia
{
	public class CreatePasswordWindow : Window
	{
		public CreatePasswordWindow()
		{
			InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			DataContext = new CreatePasswordViewModel(this.OkClose, null);
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