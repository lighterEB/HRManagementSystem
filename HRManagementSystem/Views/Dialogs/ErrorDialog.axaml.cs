using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace HRManagementSystem.Views.Dialogs;

public partial class ErrorDialog : Window
{
    public ErrorDialog()
    {
        InitializeComponent();
    }

    public ErrorDialog(string title, string message) : this()
    {
        Title = title;

        // 安全地设置控件文本
        var titleBlock = this.FindControl<TextBlock>("ErrorTitle");
        var messageBlock = this.FindControl<TextBlock>("ErrorMessage");

        if (titleBlock != null)
            titleBlock.Text = title;

        if (messageBlock != null)
            messageBlock.Text = message;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    // 公开Close方法
    public new void Close()
    {
        if (Dispatcher.UIThread.CheckAccess())
            base.Close();
        else
            Dispatcher.UIThread.Post(base.Close);
    }
}