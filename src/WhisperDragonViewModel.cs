using Avalonia;

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
	}
}