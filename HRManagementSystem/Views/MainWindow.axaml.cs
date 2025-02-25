using System;
using Avalonia.Controls;
using HRManagementSystem.ViewModels;

namespace HRManagementSystem.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // 确保窗口可以调整大小
        CanResize = true;
        
        var viewModel = new MainWindowViewModel();
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