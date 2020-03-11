using Avalonia;
using System;
using System.Windows.Input;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WhisperDragonAvalonia
{
	public class AboutViewModel
	{
		public string Description { get; set; } = "WhisperDragonAvalonia is CommonSecrets compatible password/secrets manager for Avalonia.";

		private readonly Action closeWindow;

		public AboutViewModel(Action callToClose)
		{
			this.closeWindow = callToClose;
		}

		private void OpenInBrowser(object urlAsObject)
		{
			string url = (string)urlAsObject;
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				// If no associated application/json MimeType is found xdg-open opens retrun error
				// but it tries to open it anyway using the console editor (nano, vim, other..)
				ShellExec($"xdg-open {url}", waitForExit: false);
			}
			else
			{
				ProcessStartInfo psi = new ProcessStartInfo
				{
					FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? url : "open",
					Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"{url}" : "",
					CreateNoWindow = true,
					UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
				};
				using (Process process = Process.Start(psi))
				{
					
				}
			}
		}

		private static void ShellExec(string cmd, bool waitForExit = true)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            using (var process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = "/bin/sh",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            ))
            {
                if (waitForExit)
                {
                    process.WaitForExit();
                }
            }
        }
		
		#region Buttons

		private ICommand openURLCommand;
		public ICommand OpenURLCommand
		{
			get
			{
				return openURLCommand 
					?? (openURLCommand = new ActionCommandWithParameter((object param) =>
					{
						this.OpenInBrowser(param);
					}));
			}
		}

		private ICommand okCommand;
		public ICommand OkCommand
		{
			get
			{
				return okCommand 
					?? (okCommand = new ActionCommand(() =>
					{
						this.closeWindow();
					}));
			}
		}

		#endregion // Buttons
	}
}