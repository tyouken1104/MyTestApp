using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyTestApp.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessenger _messenger;

        public MessageService()
        {
            _messenger = WeakReferenceMessenger.Default;
        }

        public async Task ShowMessage(string message, string title = "提示", MessageType type = MessageType.Information)
        {
            var messageRequest = new ShowMessageRequest(message, title, type);
            _messenger.Send(messageRequest);
            await messageRequest.TaskCompletionSource.Task;
        }

        public async Task<bool> ShowConfirm(string message, string title = "确认", MessageType type = MessageType.Warning)
        {
            var confirmRequest = new ShowConfirmRequest(message, title, type);
            _messenger.Send(confirmRequest);
            return await confirmRequest.TaskCompletionSource.Task;
        }

        public async Task<string?> ShowInput(string message, string title = "输入", string defaultValue = "")
        {
            var inputRequest = new ShowInputRequest(message, title, defaultValue);
            _messenger.Send(inputRequest);
            return await inputRequest.TaskCompletionSource.Task;
        }

        public void ShowToast(string message, MessageType type = MessageType.Information, int duration = 3000)
        {
            _messenger.Send(new ShowToastMessage(message, type, duration));
        }
    }

    #region Message Request Classes

    public class ShowMessageRequest
    {
        public string Message { get; }
        public string Title { get; }
        public MessageType Type { get; }
        public TaskCompletionSource<bool> TaskCompletionSource { get; }

        public ShowMessageRequest(string message, string title, MessageType type)
        {
            Message = message;
            Title = title;
            Type = type;
            TaskCompletionSource = new TaskCompletionSource<bool>();
        }
    }

    public class ShowConfirmRequest
    {
        public string Message { get; }
        public string Title { get; }
        public MessageType Type { get; }
        public TaskCompletionSource<bool> TaskCompletionSource { get; }

        public ShowConfirmRequest(string message, string title, MessageType type)
        {
            Message = message;
            Title = title;
            Type = type;
            TaskCompletionSource = new TaskCompletionSource<bool>();
        }
    }

    public class ShowInputRequest
    {
        public string Message { get; }
        public string Title { get; }
        public string DefaultValue { get; }
        public TaskCompletionSource<string?> TaskCompletionSource { get; }

        public ShowInputRequest(string message, string title, string defaultValue)
        {
            Message = message;
            Title = title;
            DefaultValue = defaultValue;
            TaskCompletionSource = new TaskCompletionSource<string?>();
        }
    }

    public class ShowToastMessage
    {
        public string Message { get; }
        public MessageType Type { get; }
        public int Duration { get; }

        public ShowToastMessage(string message, MessageType type, int duration)
        {
            Message = message;
            Type = type;
            Duration = duration;
        }
    }



    #endregion
}