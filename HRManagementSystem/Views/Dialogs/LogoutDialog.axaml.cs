using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HRManagementSystem.Views.Dialogs;

public partial class LogoutDialog : Window
{
    public LogoutDialog()
    {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
    }

    private void ConfirmClick(object sender, RoutedEventArgs e)
    {
        Close(true);
    }

    private void CancelClick(object sender, RoutedEventArgs e)
    {
        Close(false);
    }
}