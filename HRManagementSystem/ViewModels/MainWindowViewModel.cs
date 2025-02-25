using ReactiveUI;

namespace HRManagementSystem.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

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

        private void OnLoginSuccessful(object? sender, System.EventArgs e)
        {
            // 切换到主界面
            CurrentViewModel = new HomeViewModel();
        }
    }
}