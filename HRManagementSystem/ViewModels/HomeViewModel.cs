using System.Collections.ObjectModel;
using ReactiveUI;

namespace HRManagementSystem.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ViewModelBase _currentPage;
        private int _selectedIndex;

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

        public string UserName { get; } = "管理员";

        public HomeViewModel()
        {
            // _currentPage = new DashboardViewModel();
            _selectedIndex = 0;
        }

        private void UpdateCurrentPage(int index)
        {
            CurrentPage = index switch
            {
                // 0 => new DashboardViewModel(),
                // 1 => new EmployeeViewModel(),
                // 2 => new DepartmentViewModel(),
                // 3 => new PositionViewModel(),
                // 4 => new AttendanceViewModel(),
                // 5 => new SalaryViewModel(),
                // _ => new DashboardViewModel()
            };
        }
    }
}