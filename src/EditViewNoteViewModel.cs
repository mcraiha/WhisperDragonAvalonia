using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace WhisperDragonAvalonia
{
	/// <summary>
	/// Edit or view existing note
	/// </summary>
	public class EditViewNoteViewModel : INotifyPropertyChanged
	{
		// User should NOT be able to edit these
		private int zeroBasedIndexNumber;
		private bool wasOriginallySecret = true;

		// User editable values
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

		private readonly Action<NoteSimplified, bool /* Was Security Modified */, string /* Key identifier */> editNote;

		public EditViewNoteViewModel(NoteSimplified current, List<string> keyIds, Action positiveAction, Action negativeAction, Action<NoteSimplified, bool /* Was Security Modified */, string /* Key identifier */> edit)
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
			this.editNote = edit;

			this.zeroBasedIndexNumber = current.zeroBasedIndexNumber;
			this.wasOriginallySecret = current.IsSecure;
			this.IsSecret = current.IsSecure;
			this.Title = current.Title;
			this.Text = current.Text;
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

		private ICommand editNoteCommand;
		public ICommand EditNoteCommand
		{
			get
			{
				return editNoteCommand 
					?? (editNoteCommand = new ActionCommand(() =>
					{
						this.editNote(new NoteSimplified() {
							zeroBasedIndexNumber = this.zeroBasedIndexNumber,
							Title = this.Title,
							Text = this.Text,
							IsSecure = this.IsSecret,
						}, this.IsSecret != this.wasOriginallySecret, this.selectedKeyIdentifier);
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