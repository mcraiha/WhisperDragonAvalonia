using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	public class WhisperDragonViewModel : INotifyPropertyChanged
	{
		public const string appName = "WhisperDragon Avalonia";

		public const string untitledTempName = "Untitled";

		public string MainTitle { get; set; } = appName;

		public bool IsSaveEnabled 
		{ 
			get { return csc != null; }
		}

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

		private TabControl tabSections;

		private Window mainWindow;

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Our current common secrets container reference
		/// </summary>
		private CommonSecretsContainer csc = null;

		/// <summary>
		/// What save format should be used
		/// </summary>
		private DeserializationFormat saveFormat = DeserializationFormat.None;

		/// <summary>
		/// Path to current file (might be null)
		/// </summary>
		private string filePath = null;

		/// <summary>
		/// Is CommonSecretsContainer modified
		/// </summary>
		private bool isModified = false;

		/// <summary>
		/// Because we do not want to store actual passwords in memory, keep collection of derived ones (TODO: encrypt at some point)
		/// </summary>
		/// <typeparam name="string">Key Identifier</typeparam>
		/// <typeparam name="byte[]">Derived password as bytes</typeparam>
		private readonly Dictionary<string, byte[]> derivedPasswords = new Dictionary<string, byte[]>();

		private static readonly string normalSettingsDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WhisperDragonAvalonia", "settings.json");
		private static readonly string nearExeSettingsDataPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), "settings.json");

		private string currentSettingsPath = null;
		private SettingsData settingsData = null;

		public WhisperDragonViewModel(TabControl sections, Window window)
		{
			this.tabSections = sections;
			this.mainWindow = window;

			// Settings data priority is :
			// 1. near .exe (USB mode)
			// 2. in app data local
			// 3. no settings, so create new instance
			if (File.Exists(nearExeSettingsDataPath))
			{
				this.currentSettingsPath = nearExeSettingsDataPath;
				this.settingsData = JsonSerializer.Deserialize<SettingsData>(File.ReadAllText(nearExeSettingsDataPath));
			}
			else if (File.Exists(normalSettingsDataPath))
			{
				this.currentSettingsPath = normalSettingsDataPath;
				this.settingsData = JsonSerializer.Deserialize<SettingsData>(File.ReadAllText(normalSettingsDataPath));
			}
			else
			{
				this.currentSettingsPath = normalSettingsDataPath;
				this.settingsData = new SettingsData();
			}
		}

		#region Select tabs

		private ICommand selectFirstTabCommand;
		public ICommand SelectFirstTab
		{
			get
			{
				return selectFirstTabCommand 
					?? (selectFirstTabCommand = new ActionCommand(() =>
					{
						this.tabSections.SelectedIndex = 0;
					}));
			}
		}

		private ICommand selectSecondTabCommand;
		public ICommand SelectSecondTab
		{
			get
			{
				return selectSecondTabCommand 
					?? (selectSecondTabCommand = new ActionCommand(() =>
					{
						this.tabSections.SelectedIndex = 1;
					}));
			}
		}

		private ICommand selectThirdTabCommand;
		public ICommand SelectThirdTab
		{
			get
			{
				return selectThirdTabCommand 
					?? (selectThirdTabCommand = new ActionCommand(() =>
					{
						this.tabSections.SelectedIndex = 2;
					}));
			}
		}

		#endregion // Select tabs

		#region Title generation

		private void UpdateMainTitle(string fileName)
		{
			string possibleAsterisk = this.isModified ? "*" : "";
			this.MainTitle = $"{possibleAsterisk}{fileName} - {appName}";
			OnPropertyChanged(nameof(this.MainTitle));
		}

		#endregion // Title generation

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


		#region Common

		private void GenerateLoginSimplifiedsFromCommonSecrets()
		{
			this.logins.Clear();
			List<LoginSimplified> newLogins = LoginSimplified.TurnIntoUICompatible(this.csc.loginInformations, this.csc.loginInformationSecrets, this.derivedPasswords, this.settingsData);

			foreach (LoginSimplified login in newLogins)
			{
				this.logins.Add(login);
			}
		}

		private void GenerateNoteSimplifiedsFromCommonSecrets()
		{
			this.notes.Clear();
			List<NoteSimplified> newNotes = NoteSimplified.TurnIntoUICompatible(this.csc.notes, this.csc.noteSecrets, this.derivedPasswords, this.settingsData);

			foreach (NoteSimplified note in newNotes)
			{
				this.notes.Add(note);
			}
		}

		private void GenerateFileSimplifiedsFromCommonSecrets()
		{
			this.files.Clear();
			List<FileSimplified> newFiles = FileSimplified.TurnIntoUICompatible(this.csc.files, this.csc.fileSecrets, this.derivedPasswords, this.settingsData);

			foreach (FileSimplified file in newFiles)
			{
				this.files.Add(file);
			}
		}

		#endregion // Common


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

		#region Help

		private ICommand showAboutViaMenu;

		public ICommand ShowAboutViaMenu
		{
			get
			{
				return showAboutViaMenu
					?? (showAboutViaMenu = new ActionCommand(() =>
					{
						AboutWindow aboutWindow = new AboutWindow();
						if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
						{
							aboutWindow.ShowDialog(desktopLifetime.MainWindow);
						}
					}));
			}
		}

		#endregion // Help



		#region New, Open, Save, Close

		private void CreateNewCommonSecrets(KeyDerivationFunctionEntry kdfe, string password)
		{
			this.derivedPasswords.Clear();
			this.derivedPasswords[kdfe.GetKeyIdentifier()] = kdfe.GeneratePasswordBytes(password);

			this.csc = new CommonSecretsContainer(kdfe);

			LoginInformation demoLogin = new LoginInformation("Demo login", "https://localhost", "sample@email.com", "Dragon", "gwWTY#¤&%36", "This login will expire in 2030", new byte[] {}, "Samples", "Samples\tDemo");

			(bool successAddLoginInformation, string possibleErrorAddLoginInformation) = this.csc.AddLoginInformationSecret(password, demoLogin, kdfe.GetKeyIdentifier());

			if (!successAddLoginInformation)
			{
				//MessageBox.Show($"Error when adding demo secret: {possibleErrorAddLoginInformation}", "Error");
				return;
			}

			Note demoNote = new Note("Sample topic", "You can easily create notes");

			(bool successAddNote, string possibleErrorAddNote) = this.csc.AddNoteSecret(password, demoNote, kdfe.GetKeyIdentifier());

			if (!successAddNote)
			{
				//MessageBox.Show($"Error when adding demo note: {possibleErrorAddNote}", "Error");
				return;
			}
			
			// Update UI lists
			this.GenerateLoginSimplifiedsFromCommonSecrets();
			this.GenerateNoteSimplifiedsFromCommonSecrets();
			this.GenerateFileSimplifiedsFromCommonSecrets();
			
			this.isModified = true;
			this.UpdateMainTitle(untitledTempName);

			OnPropertyChanged(nameof(this.IsSaveEnabled));

			// Change UI
			OnPropertyChanged(nameof(this.TabsVisibility));
			OnPropertyChanged(nameof(this.WizardVisibility));
		}

		#endregion // New, Open, Save, Close


		#region Property changed

		// Create the OnPropertyChanged method to raise the event
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		#endregion // Property changed
	}
}