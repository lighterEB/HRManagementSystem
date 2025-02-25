using System;
using ReactiveUI;

namespace HRManagementSystem.ViewModels
{
    public class NewTodoViewModel : ViewModelBase
    {
        private string _task = string.Empty;
        private bool _isCompleted = false;

        public string Task
        {
            get => _task;
            set => this.RaiseAndSetIfChanged(ref _task, value);
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set => this.RaiseAndSetIfChanged(ref _isCompleted, value);
        }
    }
}