using Avalonia;
using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;

namespace WhisperDragonAvalonia
{
	public class PreferencesViewModel : INotifyPropertyChanged
	{
		public string PreferencesLocation { get; set; }

		public List<string /* string to show */> ShowModes { get; }

		// Logins
		private int selectedLoginTitleShowMode;

		public int SelectedLoginTitleShowMode
		{
			get
			{
				return this.selectedLoginTitleShowMode;
			}
			set
			{
				if (this.selectedLoginTitleShowMode != value)
				{
					this.selectedLoginTitleShowMode = value;
					OnPropertyChanged(nameof(SelectedLoginTitleShowMode));
				}
			}
		}

		private int selectedLoginUrlShowMode;

		public int SelectedLoginUrlShowMode
		{
			get
			{
				return this.selectedLoginUrlShowMode;
			}
			set
			{
				if (this.selectedLoginUrlShowMode != value)
				{
					this.selectedLoginUrlShowMode = value;
					OnPropertyChanged(nameof(SelectedLoginUrlShowMode));
				}
			}
		}

		private int selectedLoginEmailShowMode;

		public int SelectedLoginEmailShowMode
		{
			get
			{
				return this.selectedLoginEmailShowMode;
			}
			set
			{
				if (this.selectedLoginEmailShowMode != value)
				{
					this.selectedLoginEmailShowMode = value;
					OnPropertyChanged(nameof(SelectedLoginEmailShowMode));
				}
			}
		}
		
		private int selectedLoginUsernameShowMode;

		public int SelectedLoginUsernameShowMode
		{
			get
			{
				return this.selectedLoginUsernameShowMode;
			}
			set
			{
				if (this.selectedLoginUsernameShowMode != value)
				{
					this.selectedLoginUsernameShowMode = value;
					OnPropertyChanged(nameof(SelectedLoginUsernameShowMode));
				}
			}
		}

		private int selectedLoginPasswordShowMode;

		public int SelectedLoginPasswordShowMode
		{
			get
			{
				return this.selectedLoginPasswordShowMode;
			}
			set
			{
				if (this.selectedLoginPasswordShowMode != value)
				{
					this.selectedLoginPasswordShowMode = value;
					OnPropertyChanged(nameof(SelectedLoginPasswordShowMode));
				}
			}
		}

		private int selectedLoginCategoryShowMode;

		public int SelectedLoginCategoryShowMode
		{
			get
			{
				return this.selectedLoginCategoryShowMode;
			}
			set
			{
				if (this.selectedLoginCategoryShowMode != value)
				{
					this.selectedLoginCategoryShowMode = value;
					OnPropertyChanged(nameof(SelectedLoginCategoryShowMode));
				}
			}
		}
		
		// Notes
		private int selectedNoteTitleShowMode;

		public int SelectedNoteTitleShowMode
		{
			get
			{
				return this.selectedNoteTitleShowMode;
			}
			set
			{
				if (this.selectedNoteTitleShowMode != value)
				{
					this.selectedNoteTitleShowMode = value;
					OnPropertyChanged(nameof(SelectedNoteTitleShowMode));
				}
			}
		}

		private int selectedNoteTextShowMode;

		public int SelectedNoteTextShowMode
		{
			get
			{
				return this.selectedNoteTextShowMode;
			}
			set
			{
				if (this.selectedNoteTextShowMode != value)
				{
					this.selectedNoteTextShowMode = value;
					OnPropertyChanged(nameof(SelectedNoteTextShowMode));
				}
			}
		}

		private int selectedFilenameShowMode;

		public int SelectedFilenameShowMode
		{
			get
			{
				return this.selectedFilenameShowMode;
			}
			set
			{
				if (this.selectedFilenameShowMode != value)
				{
					this.selectedFilenameShowMode = value;
					OnPropertyChanged(nameof(SelectedFilenameShowMode));
				}
			}
		}

		private int selectedFileSizeShowMode;

		public int SelectedFileSizeShowMode
		{
			get
			{
				return this.selectedFileSizeShowMode;
			}
			set
			{
				if (this.selectedFileSizeShowMode != value)
				{
					this.selectedFileSizeShowMode = value;
					OnPropertyChanged(nameof(SelectedFileSizeShowMode));
				}
			}
		}

		private int selectedFileTypeShowMode;

		public int SelectedFileTypeShowMode
		{
			get
			{
				return this.selectedFileTypeShowMode;
			}
			set
			{
				if (this.selectedFileTypeShowMode != value)
				{
					this.selectedFileTypeShowMode = value;
					OnPropertyChanged(nameof(SelectedFileTypeShowMode));
				}
			}
		}

		private readonly SettingsData settings;
		private readonly Action<SettingsData> saveAction;
		private readonly Action closeAction;

		public event PropertyChangedEventHandler PropertyChanged;

		public PreferencesViewModel(SettingsData settingsData, string fileLocation, Action<SettingsData> saveAction, Action closeAction)
		{
			this.settings = settingsData;
			this.PreferencesLocation = fileLocation;
			this.saveAction = saveAction;
			this.closeAction = closeAction;

			this.ShowModes = new List<string>();
			foreach (ShowMode showMode in (ShowMode[]) Enum.GetValues(typeof(ShowMode)))
			{
				this.ShowModes.Add(showMode.ToString());
			}
			
			// Logins
			this.SelectedLoginTitleShowMode = (int)settingsData.LoginTitleShowMode;
			this.SelectedLoginUrlShowMode = (int)settings.LoginUrlShowMode;
			this.SelectedLoginEmailShowMode = (int)settings.LoginEmailShowMode;
			this.SelectedLoginUsernameShowMode = (int)settings.LoginUsernameShowMode;
			this.SelectedLoginPasswordShowMode = (int)settings.LoginPasswordShowMode;
			this.SelectedLoginCategoryShowMode = (int)settings.LoginCategoryShowMode;

			// Notes
			this.SelectedNoteTitleShowMode = (int)settingsData.NoteTitleShowMode;
			this.SelectedNoteTextShowMode = (int)settingsData.NoteTextShowMode;

			// Files
			this.SelectedFilenameShowMode = (int)settingsData.FileFilenameShowMode;
			this.SelectedFileSizeShowMode = (int)settingsData.FileFileSizeShowMode;
			this.SelectedFileTypeShowMode = (int)settingsData.FileFileTypeShowMode;
		}

		
		
		#region Buttons

		private ICommand saveCommand;
		public ICommand SaveCommand
		{
			get
			{
				return saveCommand 
					?? (saveCommand = new ActionCommand(() =>
					{
						// Logins
						this.settings.LoginTitleShowMode = (ShowMode) this.SelectedLoginTitleShowMode;
						this.settings.LoginUrlShowMode = (ShowMode) this.SelectedLoginUrlShowMode;
						this.settings.LoginEmailShowMode = (ShowMode) this.SelectedLoginEmailShowMode;
						this.settings.LoginUsernameShowMode = (ShowMode) this.SelectedLoginUsernameShowMode;
						this.settings.LoginPasswordShowMode = (ShowMode) this.SelectedLoginPasswordShowMode;
						this.settings.LoginCategoryShowMode = (ShowMode) this.SelectedLoginCategoryShowMode;

						// Notes
						this.settings.NoteTitleShowMode = (ShowMode) this.SelectedNoteTitleShowMode;
						this.settings.NoteTextShowMode = (ShowMode) this.SelectedNoteTextShowMode;

						// Files
						this.settings.FileFilenameShowMode = (ShowMode) this.SelectedFilenameShowMode;
						this.settings.FileFileSizeShowMode = (ShowMode) this.SelectedFileSizeShowMode;
						this.settings.FileFileTypeShowMode = (ShowMode) this.SelectedFileTypeShowMode;

						this.saveAction(this.settings);
						this.closeAction();
					}));
			}
		}

		private ICommand cancelCommand;
		public ICommand CancelCommand
		{
			get
			{
				return cancelCommand 
					?? (cancelCommand = new ActionCommand(() =>
					{
						this.closeAction();
					}));
			}
		}

		#endregion // Buttons


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