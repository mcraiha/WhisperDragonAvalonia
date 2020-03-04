using System;
using System.Windows.Input;

/// <summary>
/// Basic action command with conditional can execute check
/// </summary>
public class ActionConditionalCommand : ICommand
{
	private readonly Action wanteAction;
	private readonly Func<bool> canExecuteCheck;

	public ActionConditionalCommand(Action action, Func<bool> executeCheck)
	{
		this.wanteAction = action;
		this.canExecuteCheck = executeCheck;
	}

	public void Execute(object parameter)
	{
		this.wanteAction();
	}

	public bool CanExecute(object parameter)
	{
		return this.canExecuteCheck();
	}

	public void RaiseCanExecuteChanged()
	{
		if (CanExecuteChanged != null)
		{
			CanExecuteChanged(this, new EventArgs());
		}
	}

	public event EventHandler CanExecuteChanged;
}