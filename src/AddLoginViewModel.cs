using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace WhisperDragonAvalonia
{
	public class AddLoginViewModel : INotifyPropertyChanged
	{
		private bool isSecret = true;
		public bool IsSecret 
		{ 
			get
			{
				return this.isSecret;
			} 
			set
			{
				this.isSecret = value;
				OnPropertyChanged(nameof(KeyIdentifierVisibility));
			}
		}

		public string Title { get; set; } = "";

		public string URL { get; set; } = "";

		public string Email { get; set; } = "";

		public string Username { get; set; } = "";

		private bool visiblePassword = false;
		public bool VisiblePassword 
		{ 
			get
			{
				return this.visiblePassword;
			}
			
			set
			{
				this.visiblePassword = value;
				
				if (this.visiblePassword)
				{
					this.CurrentPasswordChar = default(char);
				}
				else
				{
					this.CurrentPasswordChar = '#';
				}

				OnPropertyChanged(nameof(Password));
				OnPropertyChanged(nameof(CurrentPasswordChar));
			} 
		}

		private string password = "";

		public string Password 
		{ 
			get { return this.password; }
			set
			{
				if (this.password != value)
				{
					this.password = value;
					OnPropertyChanged(nameof(Password));

					this.isCopyButtonEnabled = !string.IsNullOrEmpty(this.Password);
					OnPropertyChanged(nameof(IsCopyButtonEnabled));
				}
			} 
		}

		public char CurrentPasswordChar { get; set; } = '#';

		public string Notes { get; set; } = "";

		// TODO: Add icon here when supported

		public string Category { get; set; } = "";

		public string Tags { get; set; } = "";

		public ObservableCollection<string> KeyIdentifiers { get; }
		public string selectedKeyIdentifier;

		public string SelectedKeyIdentifier
		{
			get
			{
				return this.selectedKeyIdentifier;
			}
			set
			{
				if (this.selectedKeyIdentifier != value)
				{
					this.selectedKeyIdentifier = value;
					OnPropertyChanged(nameof(SelectedKeyIdentifier));
				}
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private readonly Action onPositiveClose;

		private readonly Action onNegativeClose;

		private readonly Action<LoginSimplified, string /* Key identifier */> addLogin;

		public AddLoginViewModel(List<string> keyIds, Action positiveAction, Action negativeAction, Action<LoginSimplified, string /* Key identifier */> add)
		{
			this.KeyIdentifiers = new ObservableCollection<string>();
			foreach (string keyIdentifier in keyIds)
			{
				this.KeyIdentifiers.Add(keyIdentifier);
			}

			if (keyIds.Count > 0)
			{
				this.SelectedKeyIdentifier = keyIds[0];
			}
			
			this.onPositiveClose = positiveAction;
			this.onNegativeClose = negativeAction;
			this.addLogin = add;
		}


		#region Visibilities

		public bool KeyIdentifierVisibility
		{ 
			get
			{
				return this.IsSecret;
			} 
			set
			{

			}
		}

		#endregion // Visibilities

		
		#region Buttons

		private ActionConditionalCommand copyPasswordCommand;
		public ActionConditionalCommand CopyPasswordCommand
		{
			get
			{
				return copyPasswordCommand 
					?? (copyPasswordCommand =new ActionConditionalCommand(() =>
					{
						Application.Current.Clipboard.SetTextAsync(this.Password);
					}, () => this.Password.Length > 1));
			}
		}

		private bool isCopyButtonEnabled = false;

		public bool IsCopyButtonEnabled
		{
			get
			{
				return this.isCopyButtonEnabled;
			}
		}

		private ICommand generatePasswordCommand;
		public ICommand GeneratePasswordCommand
		{
			get
			{
				return generatePasswordCommand 
					?? (generatePasswordCommand = new ActionCommand(() =>
					{
						//CreatePasswordWindow passwordWindow = new CreatePasswordWindow(this.UpdatePassword);
						//passwordWindow.ShowDialog();

						CreatePasswordWindow passwordWindow = new CreatePasswordWindow();
						if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
						{
							passwordWindow.ShowDialog(desktopLifetime.MainWindow);
						}
					}));
			}
		}

		private void UpdatePassword(string newPassword)
		{
			this.Password = newPassword;
			OnPropertyChanged(nameof(Password));
		}
		
		private ICommand addLoginCommand;
		public ICommand AddLoginCommand
		{
			get
			{
				return addLoginCommand 
					?? (addLoginCommand = new ActionCommand(() =>
					{
						this.addLogin(new LoginSimplified() {
							Title = this.Title,
							URL = this.URL,
							Email = this.Email,
							Username = this.Username,
							Password = this.Password,
							Notes = this.Notes,
							Icon = new byte[] { 0 },
							Category = this.Category,
							Tags = this.Tags,
							IsSecure = this.IsSecret,
							//CreationTime = DateTime.UtcNow,

						}, this.selectedKeyIdentifier);
						this.onPositiveClose();
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
						this.onNegativeClose();
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