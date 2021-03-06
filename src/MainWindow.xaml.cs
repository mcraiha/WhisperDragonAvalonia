using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace WhisperDragonAvalonia
{
	public class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			
			DataContext = new WhisperDragonViewModel(this.FindControl<TabControl>("tabSections"), this);
#if DEBUG
			this.AttachDevTools();
#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}