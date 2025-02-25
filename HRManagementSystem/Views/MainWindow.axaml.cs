using System;
using Avalonia.Controls;
using HRManagementSystem.ViewModels;

namespace HRManagementSystem.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainWindowViewModel();
        viewModel.SetWindow(this);
        DataContext = viewModel;
    }
}