using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using HRManagementSystem.ViewModels;

namespace HRManagementSystem.Views.Dialogs;

public partial class NewTodoDialog : Window
{
    private readonly NewTodoViewModel _viewModel;

    public NewTodoDialog()
    {
        InitializeComponent();
        _viewModel = new NewTodoViewModel();
        DataContext = _viewModel;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close(null);
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_viewModel.Task))
        {
            ShowError("待办事项内容不能为空");
            return;
        }

        var todoItem = new TodoItem
        {
            Task = _viewModel.Task.Trim(),
            IsCompleted = _viewModel.IsCompleted
        };

        Close(todoItem);
    }

    private async void ShowError(string message)
    {
        var dialog = new ErrorDialog("输入错误", message);
        await dialog.ShowDialog(this);
    }
}