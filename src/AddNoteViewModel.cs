using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace WhisperDragonAvalonia
{
	public class AddNoteViewModel : INotifyPropertyChanged
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

		public string Text { get; set; } = "";

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

		private readonly Action<NoteSimplified, string /* Key identifier */> addNote;

		public AddNoteViewModel(List<string> keyIds, Action positiveAction, Action negativeAction, Action<NoteSimplified, string /* Key identifier */> add)
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
			this.addNote = add;
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

		private ICommand addNoteCommand;
		public ICommand AddNoteCommand
		{
			get
			{
				return addNoteCommand 
					?? (addNoteCommand = new ActionCommand(() =>
					{
						this.addNote(new NoteSimplified() {
							Title = this.Title,
							Text = this.Text,
							IsSecure = this.IsSecret,
						},this.selectedKeyIdentifier);
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