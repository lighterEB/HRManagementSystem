using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using HRManagementSystem.Commands;
using HRManagementSystem.Models.Identity;
using HRManagementSystem.Views.Dialogs;
using Microsoft.AspNetCore.Identity;
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
    private readonly CustomSignInManager _signInManager;
    private readonly UserManager<User> _userManager;

    public LoginViewModel(CustomSignInManager signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;

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

    private async Task LoginActionAsync()
    {
        try
        {
            IsLoading = true;

            var result = await _signInManager.PasswordSignInAsync(Username, Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(Username);
                if (user != null && user.IsActive)
                {
                    user.LastLoginTime = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                    LoginSuccessful?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    await ShowErrorDialogAsync("登录失败", "账户已被禁用，请联系管理员");
                }
            }
            else
            {
                await ShowErrorDialogAsync("登录失败", "用户名或密码错误，请重试");
            }
        }
        catch (Exception ex)
        {
            await ShowErrorDialogAsync("系统错误", $"登录过程中发生异常：{ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void OnNavigateToRegister()
    {
        NavigateToRegister?.Invoke(this, EventArgs.Empty);
    }

    public void OnForgotPassword()
    {
        _ = ShowInfoDialogAsync("提示", "密码重置功能正在开发中...");
    }

    private async Task ShowDialogAsync(string title, string message, Type dialogType)
    {
        try
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var dialog = Activator.CreateInstance(dialogType, title, message) as Window;

                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var parentWindow = desktop.MainWindow;
                    if (parentWindow != null)
                    {
                        await (dialog ?? throw new InvalidOperationException("Dialog is null")).ShowDialog(parentWindow);
                    }
                    else
                    {
                        (dialog ?? throw new InvalidOperationException("Dialog is null")).Show();
                    }
                }
                else
                {
                    dialog?.Show();
                }
            });
        }
        catch (Exception dialogEx)
        {
            Console.WriteLine($"显示对话框失败: {dialogEx}");
        }
    }

    private async Task ShowErrorDialogAsync(string title, string message)
    {
        await ShowDialogAsync(title, message, typeof(ErrorDialog));
    }

    private async Task ShowInfoDialogAsync(string title, string message)
    {
        await ShowDialogAsync(title, message, typeof(InfoDialog));
    }
}