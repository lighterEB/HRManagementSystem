using ReactiveUI;

namespace HRManagementSystem.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private object? _currentPage;
        
        public object? CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public MainWindowViewModel()
        {
            // 初始化
        }
    }
}