using Avalonia.Controls;

namespace HRManagementSystem.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        
        // 使用新的LoginWindowViewModel作为数据上下文
        DataContext = new ViewModels.LoginWindowViewModel(this);
    }
}