using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HRManagementSystem.Views.Dialogs
{
    public partial class LogoutDialog : Window
    {
        public bool ShouldLogout { get; private set; }

        public LogoutDialog()
        {
            InitializeComponent();
        }

        private void OnCancelClick(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }

        private void OnConfirmClick(object? sender, RoutedEventArgs e)
        {
            Close(true);
        }
    }
}