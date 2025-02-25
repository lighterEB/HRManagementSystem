using ReactiveUI;
using System;

namespace HRManagementSystem.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        
        public event EventHandler? LoginSuccessful;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }

        public MainWindowViewModel()
        {
            var loginViewModel = new LoginViewModel();
            loginViewModel.LoginSuccessful += OnLoginSuccessful;
            CurrentViewModel = loginViewModel;
        }

        private void OnLoginSuccessful(object? sender, EventArgs e)
        {
            CurrentViewModel = new HomeViewModel();
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }
    }
}