using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
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