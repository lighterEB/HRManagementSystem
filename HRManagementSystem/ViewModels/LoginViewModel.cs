using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using HRManagementSystem.Views.Dialogs;
using ReactiveUI;

namespace HRManagementSystem.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _errorMessage = string.Empty;
        private bool _isLoading;
        private string _password = string.Empty;
        private string _username = string.Empty;
        private SimpleCommand _loginCommand;

        public LoginViewModel()
        {
            // 使用简单的Command实现，而非ReactiveCommand
            _loginCommand = new SimpleCommand(
                execute: _ => Dispatcher.UIThread.Post(async () => await LoginActionAsync()),
                canExecute: _ => !string.IsNullOrWhiteSpace(Username) && 
                                !string.IsNullOrWhiteSpace(Password) &&
                                !IsLoading
            );
        }

        public string Username
        {
            get => _username;
            set 
            { 
                this.RaiseAndSetIfChanged(ref _username, value);
                _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set 
            { 
                this.RaiseAndSetIfChanged(ref _password, value);
                _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set 
            { 
                this.RaiseAndSetIfChanged(ref _isLoading, value);
                _loginCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand LoginCommand => _loginCommand;

        public event EventHandler? LoginSuccessful;

        // 使用异步方法，避免阻塞UI线程
        private async Task LoginActionAsync()
        {
            try
            {
                // 设置加载中状态
                IsLoading = true;
                
                // 模拟网络延迟 - 使用异步等待
                await Task.Delay(1000);
                
                if (Username == "admin" && Password == "admin")
                {
                    // 用户验证成功
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    // 用户验证失败
                    await ShowErrorDialogAsync("登录失败", "用户名或密码错误，请重试");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync("系统错误", $"登录过程中发生异常：{ex.Message}");
            }
            finally
            {
                // 无论结果如何，关闭加载状态
                IsLoading = false;
            }
        }

        // 改进后的异步错误对话框
        private async Task ShowErrorDialogAsync(string title, string message)
        {
            try
            {
                // 使用异步方式在UI线程上显示对话框
                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    var dialog = new ErrorDialog(title, message);
                    
                    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        var parentWindow = desktop.MainWindow;
                        if (parentWindow != null)
                        {
                            // 使用异步方式显示对话框
                            await dialog.ShowDialog(parentWindow);
                        }
                        else
                        {
                            dialog.Show();
                        }
                    }
                    else
                    {
                        dialog.Show();
                    }
                });
            }
            catch (Exception dialogEx)
            {
                Console.WriteLine($"显示错误对话框失败: {dialogEx}");
            }
        }
    }

    // Avalonia 兼容的简单Command实现
    public class SimpleCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool> _canExecute;

        public SimpleCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (_ => true);
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        // 手动触发CanExecuteChanged事件
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}