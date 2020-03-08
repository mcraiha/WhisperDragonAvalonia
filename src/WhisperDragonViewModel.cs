using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.Windows.Input;
using CSCommonSecrets;

namespace WhisperDragonAvalonia
{
	public class WhisperDragonViewModel
	{
		private string csc = null; 
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