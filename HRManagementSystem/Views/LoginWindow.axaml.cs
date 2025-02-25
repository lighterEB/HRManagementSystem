using Avalonia.Controls;
using HRManagementSystem.ViewModels;

namespace HRManagementSystem.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var viewModel = new LoginViewModel();
            viewModel.LoginSuccessful += OnLoginSuccessful;
            DataContext = viewModel;
        }

        private void OnLoginSuccessful(object? sender, System.EventArgs e)
        {
            // 创建并显示主窗口
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // 关闭登录窗口
            Close();
        }
    }
}