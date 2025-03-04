using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using HRManagementSystem.Commands;
using HRManagementSystem.Views.Dialogs;
using ReactiveUI;

namespace HRManagementSystem.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly SimpleCommand _loginCommand;
    private readonly SimpleCommand _navigateToRegisterCommand;
    private readonly SimpleCommand _forgotPasswordCommand;
    private string _errorMessage = string.Empty;
    private bool _isLoading;
    private string _password = string.Empty;
    private string _username = string.Empty;

    public LoginViewModel()
    {
        // 使用简单的Command实现，而非ReactiveCommand
        _loginCommand = new SimpleCommand(
            _ => Dispatcher.UIThread.Post(async () => await LoginActionAsync()),
            _ => !string.IsNullOrWhiteSpace(Username) &&
                 !string.IsNullOrWhiteSpace(Password) &&
                 !IsLoading
        );
        
        // 添加导航到注册页面的命令
        _navigateToRegisterCommand = new SimpleCommand(
            _ => Dispatcher.UIThread.Post(() => OnNavigateToRegister())
        );
        
        // 添加忘记密码命令
        _forgotPasswordCommand = new SimpleCommand(
            _ => Dispatcher.UIThread.Post(() => OnForgotPassword())
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
    public ICommand NavigateToRegisterCommand => _navigateToRegisterCommand;
    public ICommand ForgotPasswordCommand => _forgotPasswordCommand;

    public event EventHandler? LoginSuccessful;
    public event EventHandler? NavigateToRegister;

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
                // 用户验证成功
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            else
                // 用户验证失败
                await ShowErrorDialogAsync("登录失败", "用户名或密码错误，请重试");
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
    
    // 导航到注册页面的处理方法
    public void OnNavigateToRegister()
    {
        NavigateToRegister?.Invoke(this, EventArgs.Empty);
    }
    
    // 忘记密码的处理方法
    public void OnForgotPassword()
    {
        Dispatcher.UIThread.Post(async () => 
        {
            await ShowInfoDialogAsync("提示", "密码重置功能正在开发中...");
        });
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
                        // 使用异步方式显示对话框
                        await dialog.ShowDialog(parentWindow);
                    else
                        dialog.Show();
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
    
    // 显示信息对话框
    private async Task ShowInfoDialogAsync(string title, string message)
    {
        try
        {
            // 同样使用Post方法避免歧义
            Dispatcher.UIThread.Post(async () =>
            {
                var dialog = new InfoDialog(title, message);
                
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var parentWindow = desktop.MainWindow;
                    if (parentWindow != null)
                        await dialog.ShowDialog(parentWindow);
                    else
                        dialog.Show();
                }
                else
                {
                    dialog.Show();
                }
            });
        }
        catch (Exception dialogEx)
        {
            Console.WriteLine($"显示信息对话框失败: {dialogEx}");
        }
    }
}