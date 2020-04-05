using Avalonia;
using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading.Tasks;

namespace WhisperDragonAvalonia
{
	public class BenchmarkViewModel : INotifyPropertyChanged
	{
		public string Description { get; set; } = "Pressing benchmark button will start 5 second benchmark of PBKDF2 key derivation";

        public string Result { get; set; } = "Press benchmark button to start";

        private readonly Action closeWindow;

        public event PropertyChangedEventHandler PropertyChanged;

        public BenchmarkViewModel(Action callToClose)
        {
            this.closeWindow = callToClose;
        }
        
        #region Buttons

        private ICommand benchmarkCommand;
        public ICommand BenchmarkCommand
        {
            get
            {
                return benchmarkCommand 
                    ?? (benchmarkCommand = new ActionCommand(async () =>
                    {
                        Result = "Started";          
                        OnPropertyChanged(nameof(Result));
                        await Task.Delay(10);
                        int runs = Benchmarker.Benchmark(howLongToRunInMilliseconds: 5000, iterations: 100000);
                        Result = $"In 5 seconds your computer did {runs} PBKDF2 brute force attempts with a single CPU core";
                        OnPropertyChanged(nameof(Result));
                    }));
            }
        }

        private ICommand stopBenchmarkCommand;
        public ICommand StopBenchmarkCommand
        {
            get
            {
                return stopBenchmarkCommand 
                    ?? (stopBenchmarkCommand = new ActionCommand(() =>
                    {
                        Result = "Stopped";
                        OnPropertyChanged(nameof(Result));
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