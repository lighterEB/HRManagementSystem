using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;
using Avalonia.Media;
using HRManagementSystem.Views.Dialogs;

namespace HRManagementSystem.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private int _totalEmployees;
        private int _totalDepartments;
        private int _totalPositions;
        private double _absenceRate;
        private ObservableCollection<AnnouncementItem> _announcements;
        private ObservableCollection<TodoItem> _todos;
        private ObservableCollection<DepartmentDistributionItem> _departmentDistribution;
        private readonly Window? _parentWindow;

        public DashboardViewModel(Window? parentWindow = null)
        {
            _parentWindow = parentWindow;
            
            // 初始化命令
            NewAnnouncementCommand = new Commands.DelegateCommand(OnNewAnnouncement);
            NewTodoCommand = new Commands.DelegateCommand(OnNewTodo);
            NewEmployeeCommand = new Commands.DelegateCommand(OnNewEmployee);
            NewDepartmentCommand = new Commands.DelegateCommand(OnNewDepartment);
            NewPositionCommand = new Commands.DelegateCommand(OnNewPosition);
            NewAttendanceCommand = new Commands.DelegateCommand(OnNewAttendance);
            NewSalaryCommand = new Commands.DelegateCommand(OnNewSalary);
            // 加载模拟数据
            LoadDemoData();
        }

        public int TotalEmployees
        {
            get => _totalEmployees;
            set => this.RaiseAndSetIfChanged(ref _totalEmployees, value);
        }

        public int TotalDepartments
        {
            get => _totalDepartments;
            set => this.RaiseAndSetIfChanged(ref _totalDepartments, value);
        }

        public int TotalPositions
        {
            get => _totalPositions;
            set => this.RaiseAndSetIfChanged(ref _totalPositions, value);
        }

        public double AbsenceRate
        {
            get => _absenceRate;
            set => this.RaiseAndSetIfChanged(ref _absenceRate, value);
        }

        public ObservableCollection<AnnouncementItem> Announcements
        {
            get => _announcements;
            set => this.RaiseAndSetIfChanged(ref _announcements, value);
        }

        public ObservableCollection<TodoItem> Todos
        {
            get => _todos;
            set => this.RaiseAndSetIfChanged(ref _todos, value);
        }

        public ObservableCollection<DepartmentDistributionItem> DepartmentDistribution
        {
            get => _departmentDistribution;
            set => this.RaiseAndSetIfChanged(ref _departmentDistribution, value);
        }
        
        // 快速操作命令
        public ICommand NewAnnouncementCommand { get; }
        public ICommand NewTodoCommand { get; }
        public ICommand NewEmployeeCommand { get; }
        public ICommand NewDepartmentCommand { get; }
        public ICommand NewPositionCommand { get; }
        public ICommand NewAttendanceCommand { get; }
        public ICommand NewSalaryCommand { get; }
        
        // 命令处理方法
        private void OnNewAnnouncement()
        {
            // 获取主窗口作为备选
            var mainWindow = Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : null;
        
            // 使用传入的窗口或主窗口
            var owner = _parentWindow ?? mainWindow;
            
            // 显示新建公告对话框
            Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
            {
                try
                {
                    var dialog = new NewAnnouncementDialog();
                    if (owner != null)
                    {
                        var result = await dialog.ShowDialog<AnnouncementItem?>(owner);
                        if (result != null)
                        {
                            Announcements.Insert(0, result);
                        }
                    }
                    else
                    {
                        // 如果没有有效的 owner，直接显示对话框（不是模态的）
                        dialog.Show();
                    }
                }
                catch (Exception ex)
                {
                    // 记录异常信息但不抛出
                    System.Diagnostics.Debug.WriteLine($"创建公告时出错: {ex.Message}");
                }
            });
        }

        private void OnNewTodo()
        {
            // 获取主窗口作为备选
            var mainWindow = Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : null;
        
            // 使用传入的窗口或主窗口
            var owner = _parentWindow ?? mainWindow;
            // 显示新建待办对话框
            Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
            {
                try
                {
                    var dialog = new NewTodoDialog();
                    if (owner != null)
                    {
                        var result = await dialog.ShowDialog<TodoItem?>(owner);
                        if (result != null)
                        {
                            Todos.Add(result);
                        }
                    }
                    else
                    {
                        // 如果没有有效的 owner，直接显示对话框（不是模态的）
                        dialog.Show();
                    }
                }
                catch (Exception ex)
                {
                    // 记录异常信息但不抛出
                    System.Diagnostics.Debug.WriteLine($"创建待办事项时出错: {ex.Message}");
                }
            });
        }

        private void OnNewEmployee()
        {
            // 导航到员工管理页面并显示新建表单
            // 此处仅模拟功能，实际实现需要与导航系统集成
            ShowNotImplementedMessage("新增员工");
        }

        private void OnNewDepartment()
        {
            // 导航到部门管理页面并显示新建表单
            ShowNotImplementedMessage("新增部门");
        }

        private void OnNewPosition()
        {
            // 导航到职位管理页面并显示新建表单
            ShowNotImplementedMessage("新增职位");
        }

        private void OnNewAttendance()
        {
            // 导航到考勤管理页面并显示新建表单
            ShowNotImplementedMessage("新增考勤");
        }
        
        private void OnNewSalary()
        {
            // 导航到薪资管理页面并显示新建表单
            ShowNotImplementedMessage("新增薪资");
        }

        private void ShowNotImplementedMessage(string feature)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
            {
                var dialog = new InfoDialog(
                    $"{feature}功能",
                    $"{feature}功能尚未实现，将在后续版本中提供。");
                await dialog.ShowDialog(_parentWindow);
            });
        }

        private async Task ShowErrorDialog(string title, string message)
        {
            var dialog = new ErrorDialog(title, message);
            await dialog.ShowDialog(_parentWindow);
        }

        private void LoadDemoData()
        {
            // 加载概览数据
            TotalEmployees = 128;
            TotalDepartments = 8;
            TotalPositions = 24;
            AbsenceRate = 0.032; // 3.2%

            // 加载公告
            Announcements = new ObservableCollection<AnnouncementItem>
            {
                new AnnouncementItem
                {
                    Title = "公司年度评优活动",
                    Date = DateTime.Now.AddDays(-2),
                    Content = "公司将于下个月开展年度评优活动，请各部门做好准备工作。优秀员工将获得额外奖金和带薪假期。"
                },
                new AnnouncementItem
                {
                    Title = "端午节放假通知",
                    Date = DateTime.Now.AddDays(-5),
                    Content = "根据国家法定节假日安排，公司将于6月10日至12日放假三天，请各部门安排好工作。"
                },
                new AnnouncementItem
                {
                    Title = "新版人力资源系统上线",
                    Date = DateTime.Now.AddDays(-10),
                    Content = "公司新版人力资源管理系统已正式上线，所有人事相关流程将在新系统中进行。请各位员工熟悉新系统的使用。"
                }
            };

            // 加载待办事项
            Todos = new ObservableCollection<TodoItem>
            {
                new TodoItem { Task = "审核本月员工考勤记录", IsCompleted = true },
                new TodoItem { Task = "准备下月部门预算报告", IsCompleted = false },
                new TodoItem { Task = "与IT部门讨论系统升级事宜", IsCompleted = false },
                new TodoItem { Task = "安排新员工入职培训", IsCompleted = false },
                new TodoItem { Task = "更新公司员工手册", IsCompleted = false }
            };

            // 加载部门分布数据
            DepartmentDistribution = new ObservableCollection<DepartmentDistributionItem>
            {
                new DepartmentDistributionItem { Name = "技术部", Count = 42, Color = SolidColorBrush.Parse("#2196F3") },
                new DepartmentDistributionItem { Name = "市场部", Count = 28, Color = SolidColorBrush.Parse("#4CAF50") },
                new DepartmentDistributionItem { Name = "销售部", Count = 18, Color = SolidColorBrush.Parse("#FFC107") },
                new DepartmentDistributionItem { Name = "人事部", Count = 12, Color = SolidColorBrush.Parse("#9C27B0") },
                new DepartmentDistributionItem { Name = "财务部", Count = 10, Color = SolidColorBrush.Parse("#F44336") },
                new DepartmentDistributionItem { Name = "行政部", Count = 8, Color = SolidColorBrush.Parse("#FF9800") },
                new DepartmentDistributionItem { Name = "客服部", Count = 6, Color = SolidColorBrush.Parse("#607D8B") },
                new DepartmentDistributionItem { Name = "其他", Count = 4, Color = SolidColorBrush.Parse("#9E9E9E") }
            };
        }
    }

    // 公告项模型
    public class AnnouncementItem
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    // 待办事项模型
    public class TodoItem
    {
        public string Task { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    // 部门分布项模型
    public class DepartmentDistributionItem
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
        public IBrush Color { get; set; } = new SolidColorBrush(Colors.Gray);
    }
}