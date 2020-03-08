using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using Avalonia;
using CSCommonSecrets;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WhisperDragonAvalonia
{
	public class CreateCommonSecretsViewModel : INotifyPropertyChanged
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
				OnPropertyChanged(nameof(PasswordBoxVisibility));
				OnPropertyChanged(nameof(Password));
			} 
		}

		public string Password { get; set; } = "";
		public string ConfirmationPassword { get; set; } = "";

		public char CurrentPasswordChar { get; set; } = '#';

		public string PasswordEntropy { get; set; } = "";

		public string ShannonEntropy { get; set; } = "";

		public ObservableCollection<string> Algorithms { get; }
		public string selectedAlgorithm;

		public string SelectedAlgorithm
		{
			get
			{
				return this.selectedAlgorithm;
			}
			set
			{
				if (this.selectedAlgorithm != value)
				{
					this.selectedAlgorithm = value;
					OnPropertyChanged(nameof(SelectedAlgorithm));
				}
			}
		}

		public ObservableCollection<string> PseudorandomFunctions { get; }

		public string selectedPseudorandomFunction;

		public string SelectedPseudorandomFunction
		{
			get
			{
				return this.selectedPseudorandomFunction;
			}
			set
			{
				if (this.selectedPseudorandomFunction != value)
				{
					this.selectedPseudorandomFunction = value;
					OnPropertyChanged(nameof(SelectedPseudorandomFunction));
				}
			}
		}

		private byte[] salt = new byte[16];

		public string Salt
		{
			get
			{
				return BitConverter.ToString(this.salt).Replace("-", string.Empty);
			}
		}

		public int Iterations { get; set; }

		public string Identifier { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private readonly Action<KeyDerivationFunctionEntry, string> callOnPositive;
		private readonly Action callOnNegative;

		public CreateCommonSecretsViewModel(Action<KeyDerivationFunctionEntry, string> positiveAction, Action negativeAction)
		{
			this.callOnPositive = positiveAction;
			this.callOnNegative = negativeAction;

			//this.passwordBox1.PasswordChanged += Password1Changed;
			//this.passwordBox2.PasswordChanged += Password2Changed;

			Algorithms = this.GenerateAlgorithms();
			this.selectedAlgorithm = Algorithms[0];

			PseudorandomFunctions = this.GeneratePseudorandomFunctions();
			this.selectedPseudorandomFunction = PseudorandomFunctions[0];

			this.GenerateSalt();

			int seed = BitConverter.ToInt32(this.salt, 0);
			Iterations = KeyDerivationFunctionEntry.suggestedMinIterationsCount + new Random(Seed: seed).Next(100, 4242);

			Identifier = "master";
		}

		private ObservableCollection<string> GenerateAlgorithms()
		{
			var returnValue = new ObservableCollection<string>();

			foreach (KDFAlgorithm algorithm in Enum.GetValues(typeof(KDFAlgorithm)))
			{
				returnValue.Add(algorithm.ToString());
			}

			return returnValue;
		}

		private ObservableCollection<string> GeneratePseudorandomFunctions()
		{
			var returnValue = new ObservableCollection<string>();

			returnValue.Add(KeyDerivationPrf.HMACSHA256.ToString());
			returnValue.Add(KeyDerivationPrf.HMACSHA512.ToString());

			return returnValue;
		}

		private void GenerateSalt()
		{
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(this.salt);
			}
		}

		#region Visibilities

		public bool PasswordTextVisibility
		{ 
			get
			{
				return this.visiblePassword;
			} 
			set
			{

			}
		}

		public bool PasswordBoxVisibility
		{ 
			get
			{
				return !this.visiblePassword;
			} 
			set
			{

			}
		}

		#endregion // Visibilities
		
		#region Buttons

		private ICommand createCommand;
		public ICommand CreateCommand
		{
			get
			{
				return createCommand 
					?? (createCommand = new ActionCommand(() =>
					{
						Enum.TryParse(this.selectedPseudorandomFunction, out KeyDerivationPrf keyDerivationPrf);
						int neededBytes = (keyDerivationPrf == KeyDerivationPrf.HMACSHA256) ? 32 : 64;
						this.callOnPositive(new KeyDerivationFunctionEntry(keyDerivationPrf, this.salt, this.Iterations, neededBytes, this.Identifier), this.Password);
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
						this.callOnNegative();
					}));
			}
		}

		#endregion // Buttons

		#region Values changed

		/*private void Password1Changed(Object sender, RoutedEventArgs args)
		{
			this.UpdatePasswordEntropy(this.Password);
			this.UpdateShannonEntropy(this.Password);
		}

		private void Password2Changed(Object sender, RoutedEventArgs args)
		{
			
		}*/

		private void UpdatePasswordEntropy(string pw)
		{
			int entropyInBits = EntropyCalcs.CalcutePasswordEntropy(pw);
			PasswordSecurityLevel level = EntropyCalcs.GetPasswordSecurityLevel(entropyInBits);
			this.PasswordEntropy = $"{LocMan.Get("Password entropy:")} {entropyInBits} {LocMan.Get("bits")} ({level})";
			OnPropertyChanged(nameof(PasswordEntropy));
		}

		private void UpdateShannonEntropy(string pw)
		{
			double entropyInBits = EntropyCalcs.ShannonEntropy(pw);
			this.ShannonEntropy = $"{LocMan.Get("Shannon entropy:")} {entropyInBits.ToString("F2")} {LocMan.Get("bits")}";
			OnPropertyChanged(nameof(ShannonEntropy));
		}

		#endregion // Values changed

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