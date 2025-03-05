using Avalonia.Controls;
using HRManagementSystem.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Views;

public partial class LoginWindow : Window
{
    public LoginWindow(CustomSignInManager signInManager, UserManager<User> userManager)
    {
        InitializeComponent();
        
        // 使用新的LoginWindowViewModel作为数据上下文
        DataContext = new ViewModels.LoginWindowViewModel(this, signInManager, userManager);
    }
}