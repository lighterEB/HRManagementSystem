using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

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

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}