using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HRManagementSystem.ViewModels;

namespace HRManagementSystem.Views;

public partial class RegisterView : UserControl
{
    public RegisterView()
    {
        InitializeComponent();

        AttachedToVisualTree += RegisterView_AttachedToVisualTree;
    }

    private void RegisterView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        var backToLoginLink = this.FindControl<TextBlock>("BackToLoginLink");

        if (backToLoginLink != null)
            backToLoginLink.PointerPressed += BackToLoginLink_PointerPressed;
    }

    private void BackToLoginLink_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is RegisterViewModel viewModel) viewModel.OnBackToLogin();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}