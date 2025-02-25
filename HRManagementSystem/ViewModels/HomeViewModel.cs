using System;
using System.Windows.Input;
using Avalonia.Controls;
using HRManagementSystem.Views.Dialogs;
using HRManagementSystem.Commands;
using ReactiveUI;

namespace HRManagementSystem.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;
        private readonly Window _mainWindow;
        private int _selectedIndex;
        private string _userName;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public HomeViewModel(Window mainWindow, MainWindowViewModel mainWindowViewModel)
        {
            _mainWindow = mainWindow;
            _mainWindowViewModel = mainWindowViewModel;
            _userName = "管理员";  // 设置默认用户名
            _selectedIndex = 0;
            _currentPage = new DashboardViewModel();

            // 创建退出命令
            LogoutCommand = new DelegateCommand(ShowLogoutDialog);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedIndex, value);
                UpdateCurrentPage(value);
            }
        }

        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            private set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        // 确保 UserName 属性可以正常工作
        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        // 确保 LogoutCommand 是公开的
        public ICommand LogoutCommand { get; }

        private void ShowLogoutDialog()
        {
            var dialog = new LogoutDialog
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var result = dialog.ShowDialog<bool>(_mainWindow).Result;
            if (result)
            {
                _mainWindowViewModel.SwitchToLogin();
            }
        }

        private void UpdateCurrentPage(int index)
        {
            CurrentPage = index switch
            {
                0 => new DashboardViewModel(),
                1 => new EmployeeViewModel(),
                2 => new DepartmentViewModel(),
                3 => new PositionViewModel(),
                4 => new AttendanceViewModel(),
                5 => new SalaryViewModel(),
                _ => new DashboardViewModel()
            };
        }
    }

    // 临时的视图模型类
    public class DashboardViewModel : ViewModelBase { }
    public class EmployeeViewModel : ViewModelBase { }
    public class DepartmentViewModel : ViewModelBase { }
    public class PositionViewModel : ViewModelBase { }
    public class AttendanceViewModel : ViewModelBase { }
    public class SalaryViewModel : ViewModelBase { }
}