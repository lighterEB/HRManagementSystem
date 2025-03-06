using System;
using Avalonia.Controls;
using HRManagementSystem.Models.Identity;
using HRManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HRManagementSystem.Views;

public partial class MainWindow : Window
{
    public MainWindow(CustomSignInManager signInManager, UserManager<User> userManager)
    {
        InitializeComponent();

        // 确保窗口可以调整大小
        CanResize = true;

        var viewModel = new MainWindowViewModel(signInManager, userManager);
        DataContext = viewModel;
        viewModel.SetWindow(this);
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        // 确保打开后窗口可以调整大小
        CanResize = true;
    }
}