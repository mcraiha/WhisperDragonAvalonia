using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using Avalonia;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	/// <summary>
	/// This window is shown when CommonSecrets container has password protected content
	/// </summary>
	public class SecondaryOpenStepViewModel : INotifyPropertyChanged
	{
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

				OnPropertyChanged(nameof(CurrentPasswordChar));
				OnPropertyChanged(nameof(Password));
			} 
		}

		public string Password { get; set; } = "";

		public char CurrentPasswordChar { get; set; } = '#';

		// TODO: Add get secret from file method

		public event PropertyChangedEventHandler PropertyChanged;

		private readonly List<KeyDerivationFunctionEntry> keyDerivationFunctionEntries;
		private readonly Action<Dictionary<string, byte[]>> giveBackDerivatedPasswords;
		private readonly Action onNegativeClose;
		private readonly Action onPositiveClose;

		public SecondaryOpenStepViewModel(List<KeyDerivationFunctionEntry> kdfes, Action<Dictionary<string, byte[]>> finalizeOpenWithDerivedPasswords, Action cancelCallback, Action openCallBack)
		{
			this.keyDerivationFunctionEntries = kdfes;
			this.giveBackDerivatedPasswords = finalizeOpenWithDerivedPasswords;

			this.onNegativeClose = cancelCallback;
			this.onPositiveClose = openCallBack;
		}

		private Dictionary<string, byte[]> GenerateDerivedPasswordsFromFields()
		{
			if (this.keyDerivationFunctionEntries.Count == 1)
			{
				return new Dictionary<string, byte[]>() { { this.keyDerivationFunctionEntries[0].GetKeyIdentifier(), this.keyDerivationFunctionEntries[0].GeneratePasswordBytes(this.Password) } };
			}

			return null;
		}


		#region Buttons
	
		private ICommand openCommand;
		public ICommand OpenCommand
		{
			get
			{
				return openCommand 
					?? (openCommand = new ActionCommand(() =>
					{
						this.giveBackDerivatedPasswords(this.GenerateDerivedPasswordsFromFields());
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