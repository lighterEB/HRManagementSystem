using Avalonia.Controls;
using HRManagementSystem.ViewModels;

namespace HRManagementSystem.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }
    }
}