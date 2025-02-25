using System;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;

namespace HRManagementSystem.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool _canResize;
    private ViewModelBase _currentViewModel;
    private Window? _window;
    private double _windowHeight;
    private double _windowWidth;

    public MainWindowViewModel()
    {
        // 初始化为临时的空视图模型
        _currentViewModel = new ViewModelBase();

        // 设置初始窗口大小为登录窗口大小
        WindowWidth = 360;
        WindowHeight = 520;
        CanResize = false;

        // 创建登录视图模型
        var loginViewModel = new LoginViewModel();
        loginViewModel.LoginSuccessful += OnLoginSuccessful;
        CurrentViewModel = loginViewModel;
    }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
    }

    public double WindowWidth
    {
        get => _windowWidth;
        private set => this.RaiseAndSetIfChanged(ref _windowWidth, value);
    }

    public double WindowHeight
    {
        get => _windowHeight;
        private set => this.RaiseAndSetIfChanged(ref _windowHeight, value);
    }

    public bool CanResize
    {
        get => _canResize;
        private set => this.RaiseAndSetIfChanged(ref _canResize, value);
    }

    public void SetWindow(Window window)
    {
        _window = window ?? throw new ArgumentNullException(nameof(window));

        // 应用初始窗口设置
        Dispatcher.UIThread.Post(() =>
        {
            if (_window != null && _currentViewModel is HomeViewModel)
            {
                // 确保在主界面时窗口可调整大小
                _window.CanResize = true;
                CanResize = true;
            }
        });

        // 设置窗口后创建 HomeViewModel
        CurrentViewModel = new HomeViewModel(_window, this);
    }

    private void OnLoginSuccessful(object? sender, EventArgs e)
    {
        WindowWidth = 1200;
        WindowHeight = 800;
        CanResize = true;
        
        // 确保窗口应用新的尺寸和属性
        if (_window != null)
        {
            Dispatcher.UIThread.Post(() => {
                _window.Width = WindowWidth;
                _window.Height = WindowHeight;
                _window.CanResize = true;  // 直接设置为 true
            });
        }
        
        CurrentViewModel = new HomeViewModel(_window!, this);
    }
}