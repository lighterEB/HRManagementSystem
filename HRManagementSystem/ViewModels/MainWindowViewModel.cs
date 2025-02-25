using ReactiveUI;
using System;
using Avalonia.Controls;

namespace HRManagementSystem.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private double _windowWidth;
        private double _windowHeight;
        private bool _canResize;
        private Window? _window;

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

        public MainWindowViewModel()
        {
            SwitchToLogin();
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        private void OnLoginSuccessful(object? sender, EventArgs e)
        {
            WindowWidth = 1200;
            WindowHeight = 800;
            CanResize = true;
            
            CurrentViewModel = new HomeViewModel(_window!, this);
        }

        public void SwitchToLogin()
        {
            WindowWidth = 360;
            WindowHeight = 520;
            CanResize = false;
            
            var loginViewModel = new LoginViewModel();
            loginViewModel.LoginSuccessful += OnLoginSuccessful;
            CurrentViewModel = loginViewModel;
        }
    }
}