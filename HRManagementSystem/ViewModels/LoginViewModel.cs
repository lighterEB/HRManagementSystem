using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace HRManagementSystem.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private string _errorMessage = string.Empty;
    private bool _isLoading;
    private string _password = string.Empty;
    private string _username = string.Empty;

    public LoginViewModel()
    {
        var canLogin = this.WhenAnyValue(
            x => x.Username,
            x => x.Password,
            x => x.IsLoading,
            (username, password, isLoading) =>
                !string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(password) &&
                !isLoading
        );

        LoginCommand = ReactiveCommand.CreateFromTask(
            async () => await Login(),
            canLogin
        );
    }

    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public ICommand LoginCommand { get; }

    public event EventHandler? LoginSuccessful;

    private async Task Login()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            await Task.Delay(1000); // 模拟网络延迟

            if (Username == "admin" && Password == "admin")
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            else
                ErrorMessage = "用户名或密码错误";
        }
        catch (Exception ex)
        {
            ErrorMessage = "登录时发生错误：" + ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }
}