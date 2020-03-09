using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	public class WhisperDragonViewModel
	{
		public const string appName = "WhisperDragon WPF";

		public const string untitledTempName = "Untitled";

		public string MainTitle { get; set; } = appName;

		public bool IsSaveEnabled 
		{ 
			get { return csc != null; }
		}

		private string csc = null;

		// Logins
		private ObservableCollection<LoginSimplified> logins = new ObservableCollection<LoginSimplified>();
		public ObservableCollection<LoginSimplified> Logins
		{
			get { return this.logins; }
		}

		public LoginSimplified SelectedLogin { get; set; }


		// Notes
		private ObservableCollection<NoteSimplified> notes = new ObservableCollection<NoteSimplified>();
		public ObservableCollection<NoteSimplified> Notes
		{
			get { return this.notes; }
		}

		public NoteSimplified SelectedNote { get; set; }


		// Files
		private ObservableCollection<FileSimplified> files = new ObservableCollection<FileSimplified>();
		public ObservableCollection<FileSimplified> Files
		{
			get { return this.files; }
		}

		public FileSimplified SelectedFile { get; set; }

		#region Vibility

		public bool TabsVisibility
		{ 
			get
			{
				return this.csc != null;
			} 
			set
			{

			}
		}

		public bool WizardVisibility
		{ 
			get
			{
				return this.csc == null;
			} 
			set
			{

			}
		}


		#endregion // Visibility


		#region Tools

		private ICommand createNewCommonSecretsContainerViaMenu;

		public ICommand CreateNewCommonSecretsContainerViaMenu
		{
			get
			{
				return createNewCommonSecretsContainerViaMenu 
					?? (createNewCommonSecretsContainerViaMenu = new ActionCommand(() =>
					{
						CreateCommonSecretsWindow createCommonSecretsWindow = new CreateCommonSecretsWindow(this.CreateNewCommonSecrets);
						if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
						{
							createCommonSecretsWindow.ShowDialog(desktopLifetime.MainWindow);
						}
					}));
			}
		}

		private ICommand generatePasswordViaMenu;
		public ICommand GeneratePasswordViaMenu
		{
			get
			{
				return generatePasswordViaMenu 
					?? (generatePasswordViaMenu = new ActionCommand(() =>
					{
						CreatePasswordWindow passwordWindow = new CreatePasswordWindow();
						if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
						{
							passwordWindow.ShowDialog(desktopLifetime.MainWindow);
						}
					}));
			}
		}


		#endregion // Tools


		#region New, Open, Save, Close

		private void CreateNewCommonSecrets(KeyDerivationFunctionEntry kdfe, string password)
		{

		}

		#endregion // New, Open, Save, Close
	}
}