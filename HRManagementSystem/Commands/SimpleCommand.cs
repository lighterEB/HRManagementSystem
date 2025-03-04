using System;
using System.Windows.Input;

namespace HRManagementSystem.Commands;

/// <summary>
/// 提供简单的命令实现
/// </summary>
public class SimpleCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    /// <summary>
    /// 创建一个新的命令实例
    /// </summary>
    /// <param name="execute">执行命令的方法</param>
    /// <param name="canExecute">决定命令是否可以执行的方法，如不提供则永远返回true</param>
    public SimpleCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// 判断命令是否可以执行
    /// </summary>
    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    /// <summary>
    /// 执行命令
    /// </summary>
    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    /// <summary>
    /// 当可执行状态改变时发生
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// 手动触发CanExecuteChanged事件
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}