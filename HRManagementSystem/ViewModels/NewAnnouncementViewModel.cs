using ReactiveUI;

namespace HRManagementSystem.ViewModels;

public class NewAnnouncementViewModel : ViewModelBase
{
    private string _content = string.Empty;
    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public string Content
    {
        get => _content;
        set => this.RaiseAndSetIfChanged(ref _content, value);
    }
}