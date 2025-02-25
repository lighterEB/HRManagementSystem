using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using HRManagementSystem.ViewModels;

namespace HRManagementSystem.Views.Dialogs
{
    public partial class NewAnnouncementDialog : Window
    {
        private readonly NewAnnouncementViewModel _viewModel;

        public NewAnnouncementDialog()
        {
            InitializeComponent();
            _viewModel = new NewAnnouncementViewModel();
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
            if (string.IsNullOrWhiteSpace(_viewModel.Title))
            {
                ShowError("标题不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(_viewModel.Content))
            {
                ShowError("内容不能为空");
                return;
            }

            var announcement = new AnnouncementItem
            {
                Title = _viewModel.Title.Trim(),
                Content = _viewModel.Content.Trim(),
                Date = DateTime.Now
            };

            Close(announcement);
        }

        private async void ShowError(string message)
        {
            var dialog = new ErrorDialog("输入错误", message);
            await dialog.ShowDialog(this);
        }
    }
}