using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;

namespace HRManagementSystem.Views;

public partial class SplashWindow : Window
{
    private readonly Border _mainBorder;
    private TaskCompletionSource<bool> _fadeOutComplete;

    public SplashWindow()
    {
        InitializeComponent();
        _mainBorder = this.FindControl<Border>("MainBorder");
        _fadeOutComplete = new TaskCompletionSource<bool>();
    }

    public async Task ShowSplashScreenAsync()
    {
        // 等待一段时间以显示加载效果
        await Task.Delay(2000);
    }

    public async Task FadeOutAsync()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _mainBorder.Classes.Remove("fadeIn");
            _mainBorder.Classes.Add("fadeOut");
        });

        // 等待淡出动画完成
        await Task.Delay(600); // 稍微多等一下，确保动画完成
    }
}