using System;
using Avalonia.Controls;
using HRManagementSystem.Models.Identity;
using HRManagementSystem.Views;
using Microsoft.AspNetCore.Identity;
using ReactiveUI;

namespace HRManagementSystem.ViewModels;

public class LoginWindowViewModel : ViewModelBase
{
    private readonly LoginViewModel _loginViewModel;
    private readonly RegisterViewModel _registerViewModel;

    private readonly CustomSignInManager _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly Window _window;
    private ViewModelBase _currentView;

    public LoginWindowViewModel(Window window, CustomSignInManager signInManager, UserManager<User> userManager)
    {
        _window = window ?? throw new ArgumentNullException(nameof(window));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        // 创建视图模型
        _loginViewModel = new LoginViewModel(_signInManager, _userManager);
        _registerViewModel = new RegisterViewModel();

        // 设置初始视图
        _currentView = _loginViewModel;

        // 事件订阅
        _loginViewModel.LoginSuccessful += OnLoginSuccessful;
        _loginViewModel.NavigateToRegister += OnNavigateToRegister;

        _registerViewModel.BackToLogin += OnBackToLogin;
        _registerViewModel.RegisterSuccessful += OnRegisterSuccessful;
    }

    public ViewModelBase CurrentView
    {
        get => _currentView;
        private set
        {
            this.RaiseAndSetIfChanged(ref _currentView, value);
            AdjustWindowSize();
        }
    }

    private void AdjustWindowSize()
    {
        // 根据当前视图调整窗口大小
        if (_currentView is RegisterViewModel)
            // 注册页面需要更大的高度
            _window.Height = 680;
        else if (_currentView is LoginViewModel)
            // 登录页面使用原始高度
            _window.Height = 520;
    }

    private void OnLoginSuccessful(object? sender, EventArgs e)
    {
        // 创建并显示主窗口
        var mainWindow = new MainWindow(_signInManager, _userManager);
        mainWindow.Show();

        // 关闭登录窗口
        _window.Close();
    }

    private void OnNavigateToRegister(object? sender, EventArgs e)
    {
        // 切换到注册视图
        CurrentView = _registerViewModel;
    }

    private void OnBackToLogin(object? sender, EventArgs e)
    {
        // 切换回登录视图
        CurrentView = _loginViewModel;
    }

    private void OnRegisterSuccessful(object? sender, EventArgs e)
    {
        // 注册成功后，可以在这里添加额外的逻辑
    }
}