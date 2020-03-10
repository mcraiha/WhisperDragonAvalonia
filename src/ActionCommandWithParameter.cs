using System;
using System.Windows.Input;

/// <summary>
/// Basic action command with parameter implementation
/// </summary>
public class ActionCommandWithParameter : ICommand
{
	private readonly Action<object> wanteAction;

	public ActionCommandWithParameter(Action<object> action)
	{
		this.wanteAction = action;
	}

	public void Execute(object parameter)
	{
		this.wanteAction(parameter);
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