using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HRManagementSystem.Views.Dialogs;

public partial class ErrorDialog : Window
{
    public ErrorDialog(string title, string message)
    {
        InitializeComponent();

        var errorTitle = this.FindControl<TextBlock>("ErrorTitle");
        var errorMessage = this.FindControl<TextBlock>("ErrorMessage");

        if (errorTitle != null) errorTitle.Text = title;
        if (errorMessage != null) errorMessage.Text = message;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}