using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using HRManagementSystem.Commands;
using HRManagementSystem.Views.Dialogs;
using ReactiveUI;

namespace HRManagementSystem.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    private readonly SimpleCommand _registerCommand;
    private string _confirmPassword = string.Empty;
    private string _email = string.Empty;
    private bool _isLoading;
    private string _password = string.Empty;
    private string _username = string.Empty;

    public RegisterViewModel()
    {
        _registerCommand = new SimpleCommand(
            _ => Dispatcher.UIThread.Post(async () => await RegisterActionAsync()),
            _ => !IsLoading && IsFormValid
        );
    }

    public string Username
    {
        get => _username;
        set
        {
            this.RaiseAndSetIfChanged(ref _username, value);
            _registerCommand.RaiseCanExecuteChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            this.RaiseAndSetIfChanged(ref _password, value);
            _registerCommand.RaiseCanExecuteChanged();
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            this.RaiseAndSetIfChanged(ref _confirmPassword, value);
            _registerCommand.RaiseCanExecuteChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            this.RaiseAndSetIfChanged(ref _email, value);
            _registerCommand.RaiseCanExecuteChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            this.RaiseAndSetIfChanged(ref _isLoading, value);
            _registerCommand.RaiseCanExecuteChanged();
        }
    }

    public bool IsFormValid =>
        !string.IsNullOrWhiteSpace(Username) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(ConfirmPassword) &&
        !string.IsNullOrWhiteSpace(Email) &&
        Password == ConfirmPassword;

    public ICommand RegisterCommand => _registerCommand;

    public event EventHandler? RegisterSuccessful;
    public event EventHandler? BackToLogin;

    public void OnBackToLogin()
    {
        BackToLogin?.Invoke(this, EventArgs.Empty);
    }

    private async Task RegisterActionAsync()
    {
        try
        {
            IsLoading = true;

            // 模拟注册过程
            await Task.Delay(1500);

            // 在此处添加真实的注册逻辑
            // 例如：调用API或服务

            // 注册成功
            var dialog = new InfoDialog("注册成功", "账号已创建，请登录");
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                await dialog.ShowDialog(desktop.MainWindow);

            // 触发注册成功事件
            RegisterSuccessful?.Invoke(this, EventArgs.Empty);

            // 触发返回登录事件
            BackToLogin?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            var errorDialog = new ErrorDialog("注册失败", ex.Message);
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                await errorDialog.ShowDialog(desktop.MainWindow);
        }
        finally
        {
            IsLoading = false;
        }
    }
}