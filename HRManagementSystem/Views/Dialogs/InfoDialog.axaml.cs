using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace HRManagementSystem.Views.Dialogs
{
    public partial class InfoDialog : Window
    {
        public InfoDialog(string title, string message)
        {
            InitializeComponent();
            
            var titleTextBlock = this.FindControl<TextBlock>("TitleTextBlock");
            var messageTextBlock = this.FindControl<TextBlock>("MessageTextBlock");
            
            titleTextBlock.Text = title;
            messageTextBlock.Text = message;
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
}