using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HRManagementSystem.ViewModels;

namespace HRManagementSystem.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();

        // 初始化后获取控件并添加事件处理
        AttachedToVisualTree += LoginView_AttachedToVisualTree;
    }

    private void LoginView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        // 控件完全加载后再获取引用，确保能找到
        var registerLink = this.FindControl<TextBlock>("RegisterLink");
        var forgotPasswordLink = this.FindControl<TextBlock>("ForgotPasswordLink");

        if (registerLink != null)
            registerLink.PointerPressed += RegisterLink_PointerPressed;

        if (forgotPasswordLink != null)
            forgotPasswordLink.PointerPressed += ForgotPasswordLink_PointerPressed;
    }

    private void RegisterLink_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
            // 直接调用ViewModel的方法
            viewModel.OnNavigateToRegister();
    }

    private void ForgotPasswordLink_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
            // 直接调用ViewModel的方法
            viewModel.OnForgotPassword();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}