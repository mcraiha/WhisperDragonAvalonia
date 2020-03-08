using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using Avalonia;
using Avalonia.Input.Platform;

namespace WhisperDragonAvalonia
{
	public class CreatePasswordViewModel : INotifyPropertyChanged
	{
		public int PasswordLength { get; set; } = 16;

		public bool IncludeUpperCaseLatinLetters { get; set; } = true;

		public bool IncludeLowerCaseLatinLetters { get; set; } = true;

		public bool IncludeDigits { get; set; } = true;

		public bool IncludeSpecialCharactersASCII { get; set; } = true;

		public bool IncludeEmojis { get; set; } = false;

		private bool visiblePassword = true;
		public bool VisiblePassword 
		{ 
			get
			{
				return this.visiblePassword;
			}
			
			set
			{
				this.visiblePassword = value;
				// Update to cause onpropertychange
				GeneratedPassword = generatedPassword;
			} 
		}
		public bool VisibilityUsePassword { get; set; }

		private string generatedPassword = "";
		public string GeneratedPassword 
		{ 
			get
			{
				if (VisiblePassword)
				{
					return this.generatedPassword;
				}
				
				return string.Create(this.generatedPassword.Length, '*', (chars, buf) => {
																			for (int i=0;i<chars.Length;i++) chars[i] = buf;
						});
			}
			set         
			{
				this.generatedPassword = value;
				OnPropertyChanged(nameof(GeneratedPassword));
			}
		
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private static readonly List<char> upperCaseLatinLetters = new List<char>()
		{
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
		};

		private static readonly List<char> lowerCaseLatinLetters = new List<char>()
		{
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
		};

		private static readonly List<char> digits = new List<char>()
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
		};

		private static readonly List<char> specialCharactersASCII = new List<char>()
		{
			'!', '"', '#', '$', '%', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~'
		};

		// Generated during runtime in ConstructEmojiList(), See https://en.wikipedia.org/wiki/Emoticons_(Unicode_block)
		private static readonly List<string> emoticonsUnicodeBlock = new List<string>();

		private readonly Action callOnClose;
		private readonly Action<string> passwordCallback;

		public CreatePasswordViewModel(Action closeAction, Action<string> passwordCallback)
		{
			this.callOnClose = closeAction;
			this.passwordCallback = passwordCallback;
			this.VisibilityUsePassword = (passwordCallback != null);
		}


		#region Buttons

		private ICommand generatePasswordCommand;
		public ICommand GeneratePasswordCommand
		{
			get
			{
				return generatePasswordCommand 
					?? (generatePasswordCommand = new ActionCommand(() =>
					{
						List<string> generated = new List<string>(this.PasswordLength);

						List<string> possibleChars = new List<string>();

						using (var generator = RandomNumberGenerator.Create())
						{
							if (IncludeUpperCaseLatinLetters)
							{
								int index = GetPositiveRandomInt(generator) % upperCaseLatinLetters.Count;
								generated.Add(upperCaseLatinLetters[index].ToString());
								possibleChars.AddRange(Array.ConvertAll<char, string>(upperCaseLatinLetters.ToArray(), element => element.ToString()));
							}

							if (IncludeLowerCaseLatinLetters)
							{
								int index = GetPositiveRandomInt(generator) % lowerCaseLatinLetters.Count;
								generated.Add(lowerCaseLatinLetters[index].ToString());
								possibleChars.AddRange(Array.ConvertAll<char, string>(lowerCaseLatinLetters.ToArray(), element => element.ToString()));
							}

							if (IncludeDigits)
							{
								int index = GetPositiveRandomInt(generator) % digits.Count;
								generated.Add(digits[index].ToString());
								possibleChars.AddRange(Array.ConvertAll<char, string>(digits.ToArray(), element => element.ToString()));
							}

							if (IncludeSpecialCharactersASCII)
							{
								int index = GetPositiveRandomInt(generator) % specialCharactersASCII.Count;
								generated.Add(specialCharactersASCII[index].ToString());
								possibleChars.AddRange(Array.ConvertAll<char, string>(specialCharactersASCII.ToArray(), element => element.ToString()));
							}

							if (IncludeEmojis)
							{
								if (emoticonsUnicodeBlock.Count < 1)
								{
									ConstructEmojiList();
								}

								int index = GetPositiveRandomInt(generator) % emoticonsUnicodeBlock.Count;
								generated.Add(emoticonsUnicodeBlock[index]);
								possibleChars.AddRange(emoticonsUnicodeBlock);
							}

							// Reorder all possible chars
							possibleChars = possibleChars.OrderBy(x => GetPositiveRandomInt(generator)).ToList();

							while (generated.Count < this.PasswordLength)
							{
								int index = GetPositiveRandomInt(generator) % possibleChars.Count;
								generated.Add(possibleChars[index]);
							}

							// Reorder all generated chars
							generated = generated.OrderBy(x => GetPositiveRandomInt(generator)).ToList();
						}

						GeneratedPassword = string.Join("", generated); //  new String( generated.ToArray());
						copyToClipboardCommand.RaiseCanExecuteChanged();
					}));
			}
		}


		private ActionConditionalCommand copyToClipboardCommand;
		public ActionConditionalCommand CopyToClipboardCommand
		{
			get
			{
				return copyToClipboardCommand 
					?? (copyToClipboardCommand = new ActionConditionalCommand(() =>
					{
						Application.Current.Clipboard.SetTextAsync(this.generatedPassword);
					}, () => this.generatedPassword.Length > 1));
			}
		}


		private ICommand closeCommand;
		public ICommand CloseCommand
		{
			get
			{
				return closeCommand 
					?? (closeCommand = new ActionCommand(() =>
					{
						this.callOnClose?.Invoke();
					}));
			}
		}

		private ICommand useCommand;
		public ICommand UseCommand
		{
			get
			{
				return useCommand 
					?? (useCommand = new ActionCommand(() =>
					{
						this.passwordCallback?.Invoke(this.generatedPassword);
						this.callOnClose?.Invoke();
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

		private static int GetPositiveRandomInt(RandomNumberGenerator rng)
		{
			int returnValue = -1;

			byte[] byteArray = new byte[4];

			while (returnValue < 0)
			{
				rng.GetBytes(byteArray);
				returnValue = BitConverter.ToInt32(byteArray, 0);
			}

			return returnValue;
		}

		private static void ConstructEmojiList()
		{
			int startValue = 0x1F600;
			emoticonsUnicodeBlock.Add(Char.ConvertFromUtf32(startValue));
			for (int i = 0; i < 80; i++)
			{
				startValue++;
				emoticonsUnicodeBlock.Add(Char.ConvertFromUtf32(startValue));
			}
		}
	}
}