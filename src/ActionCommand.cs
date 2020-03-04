using System;
using System.Windows.Input;

/// <summary>
/// Basic action command implementation
/// </summary>
public class ActionCommand : ICommand
{
	private readonly Action wanteAction;

	public ActionCommand(Action action)
	{
		this.wanteAction = action;
	}

	public void Execute(object parameter)
	{
		this.wanteAction();
	}

	public bool CanExecute(object parameter)
	{
		return true;
	}

	public event EventHandler CanExecuteChanged
	{
		// These are added here to remove warning CS0067
		add { }
		remove { }
	}
}