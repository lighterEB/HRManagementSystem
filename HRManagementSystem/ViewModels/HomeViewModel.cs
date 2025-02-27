using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using HRManagementSystem.Commands;
using HRManagementSystem.Views;
using HRManagementSystem.Views.Dialogs;
using ReactiveUI;

namespace HRManagementSystem.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private readonly Window _mainWindow;
    private readonly MainWindowViewModel _mainWindowViewModel;
    private ViewModelBase _currentPage;

    private ObservableCollection<MenuItem> _menuItems;
    private int _selectedIndex;
    private string _userName;

    public HomeViewModel(Window mainWindow, MainWindowViewModel mainWindowViewModel)
    {
        _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        _mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        _userName = "管理员";
        _selectedIndex = 0;
        _currentPage = new DashboardViewModel(_mainWindow);

        // 直接使用 DelegateCommand
        LogoutCommand = new DelegateCommand(OnLogout);

        // 初始化菜单项
        _menuItems = new ObservableCollection<MenuItem>
        {
            new() { Title = "工作仪表盘", Icon = "ViewDashboard" },
            new() { Title = "员工管理", Icon = "AccountMultiple" },
            new() { Title = "部门管理", Icon = "Domain" },
            new() { Title = "职位管理", Icon = "WorkOutline" },
            new() { Title = "考勤管理", Icon = "CalendarClock" },
            new() { Title = "薪资管理", Icon = "CashMultiple" }
        };
    }

    public ObservableCollection<MenuItem> MenuItems
    {
        get => _menuItems;
        private set => this.RaiseAndSetIfChanged(ref _menuItems, value);
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

    public string UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    public ICommand LogoutCommand { get; }

    private void OnLogout()
    {
        // 使用 async Task 而不是 async void
        Dispatcher.UIThread.Post(() => { _ = ShowLogoutDialogSafe(); });
    }

    private async Task ShowLogoutDialogSafe()
    {
        try
        {
            var dialog = new LogoutDialog();
            var result = await dialog.ShowDialog<bool>(_mainWindow);
            if (result)
            {
                // 创建并显示新的登录窗口
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // 关闭当前主窗口
                _mainWindow.Close();
            }
        }
        catch (Exception ex)
        {
            var errorDialog = new ErrorDialog("退出登录时发生错误", ex.Message);
            await errorDialog.ShowDialog(_mainWindow);
        }
    }

    private void UpdateCurrentPage(int index)
    {
        CurrentPage = index switch
        {
            0 => new DashboardViewModel(_mainWindow),
            1 => new EmployeeViewModel(),
            2 => new DepartmentViewModel(),
            3 => new PositionViewModel(),
            4 => new AttendanceViewModel(),
            5 => new SalaryViewModel(),
            _ => new DashboardViewModel(_mainWindow)
        };
    }

    // 菜单项模型
    public class MenuItem
    {
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}

// 临时的视图模型类

public class EmployeeViewModel : ViewModelBase
{
}

public class DepartmentViewModel : ViewModelBase
{
}

public class PositionViewModel : ViewModelBase
{
}

public class AttendanceViewModel : ViewModelBase
{
}

public class SalaryViewModel : ViewModelBase
{
}