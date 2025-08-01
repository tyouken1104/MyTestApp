using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyTestApp.Services;

namespace MyTestApp.ViewModels
{
    public class RemoveToastMessage
    {
        public ToastMessage Toast { get; }

        public RemoveToastMessage(ToastMessage toast)
        {
            Toast = toast;
        }
    }

    public partial class ToastMessage : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _message = string.Empty;

        [ObservableProperty]
        private string _typeColor = "#0078D4";

        [ObservableProperty]
        private int _duration = 3000;

        public ToastMessage(string title, string message, string typeColor, int duration = 3000)
        {
            Title = title;
            Message = message;
            TypeColor = typeColor;
            Duration = duration;
        }

        [RelayCommand]
        public void CloseCommand()
        {
            WeakReferenceMessenger.Default.Send(new RemoveToastMessage(this));
        }
    }

    public partial class MessageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isMessageVisible;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _message = string.Empty;

        [ObservableProperty]
        private MessageType _currentMessageType;

        [ObservableProperty]
        private string _typeColor = "#0078D4";

        [ObservableProperty]
        private string _icon = "fa-info-circle";

        [ObservableProperty]
        private bool _isConfirmDialog;

        [ObservableProperty]
        private string _inputText = string.Empty;

        [ObservableProperty]
        private bool _isInputDialog;

        [ObservableProperty]
        private bool _showCloseButton = true;

        [ObservableProperty]
        private bool _showCancelButton = true;

        [ObservableProperty]
        private string _confirmButtonText = "确定";

        [ObservableProperty]
        private bool _showInput;

        public ObservableCollection<ToastMessage> ToastMessages { get; } = new ObservableCollection<ToastMessage>();

        private TaskCompletionSource<bool>? _currentConfirmTask;
        private TaskCompletionSource<string?>? _currentInputTask;

        public MessageViewModel(IMessenger? messenger)
        {
            // 使用WeakReferenceMessenger.Default确保与ToastMessage使用相同的实例
            var messengerInstance = WeakReferenceMessenger.Default;
            messengerInstance.Register<ShowMessageRequest>(this, OnShowMessage);
            messengerInstance.Register<ShowConfirmRequest>(this, OnShowConfirm);
            messengerInstance.Register<ShowInputRequest>(this, OnShowInput);
            messengerInstance.Register<ShowToastMessage>(this, OnShowToast);
            messengerInstance.Register<RemoveToastMessage>(this, OnRemoveToast);
        }

        private void OnShowMessage(object recipient, ShowMessageRequest message)
        {
            Title = message.Title;
            Message = message.Message;
            CurrentMessageType = message.Type;
            
            // 根据消息类型设置颜色和图标
            (TypeColor, Icon) = GetTypeColorAndIcon(message.Type);
            
            IsConfirmDialog = false;
            IsInputDialog = false;
            ShowCloseButton = true;
            ShowCancelButton = false;
            ShowInput = false;
            ConfirmButtonText = "确定";
            IsMessageVisible = true;
        }

        private void OnShowConfirm(object recipient, ShowConfirmRequest message)
        {
            Title = message.Title;
            Message = message.Message;
            CurrentMessageType = message.Type;
            
            // 根据消息类型设置颜色和图标
            (TypeColor, Icon) = GetTypeColorAndIcon(message.Type);
            
            IsConfirmDialog = true;
            IsInputDialog = false;
            ShowCloseButton = true;
            ShowCancelButton = true;
            ShowInput = false;
            ConfirmButtonText = "确定";
            IsMessageVisible = true;
            _currentConfirmTask = message.TaskCompletionSource;
        }

        private void OnShowInput(object recipient, ShowInputRequest message)
        {
            Title = message.Title;
            Message = message.Message;
            CurrentMessageType = MessageType.Information;
            
            // 根据消息类型设置颜色和图标
            (TypeColor, Icon) = GetTypeColorAndIcon(MessageType.Information);
            
            InputText = message.DefaultValue;
            IsInputDialog = true;
            IsConfirmDialog = false;
            ShowCloseButton = true;
            ShowCancelButton = true;
            ShowInput = true;
            ConfirmButtonText = "确定";
            IsMessageVisible = true;
            _currentInputTask = message.TaskCompletionSource;
        }

        private void OnShowToast(object recipient, ShowToastMessage message)
        {
            var title = message.Type switch
            {
                MessageType.Success => "成功",
                MessageType.Warning => "警告",
                MessageType.Error => "错误",
                _ => "提示"
            };
            
            var typeColor = message.Type switch
            {
                MessageType.Success => "#107C10",
                MessageType.Warning => "#FF8C00",
                MessageType.Error => "#D13438",
                _ => "#0078D4"
            };

            var toast = new ToastMessage(title, message.Message, typeColor, message.Duration);
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ToastMessages.Add(toast);
                
                // 只有成功和信息类型的Toast自动关闭，警告和错误类型需要手动关闭
                if (message.Type == MessageType.Success || message.Type == MessageType.Information)
                {
                    _ = Task.Run(async () =>
                    {
                        await Task.Delay(message.Duration);
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            if (ToastMessages.Contains(toast))
                            {
                                ToastMessages.Remove(toast);
                            }
                        });
                    });
                }
            });
        }

        private void OnRemoveToast(object recipient, RemoveToastMessage message)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (ToastMessages.Contains(message.Toast))
                {
                    ToastMessages.Remove(message.Toast);
                }
            });
        }

        public void ConfirmMessage()
        {
            if (IsConfirmDialog)
            {
                _currentConfirmTask?.TrySetResult(true);
            }
            else if (IsInputDialog)
            {
                _currentInputTask?.TrySetResult(InputText);
            }
            else
            {
                // 普通消息确认
                _currentConfirmTask?.TrySetResult(true);
            }
            IsMessageVisible = false;
            _currentConfirmTask = null;
            _currentInputTask = null;
        }

        public void CancelMessage()
        {
            if (IsConfirmDialog)
            {
                _currentConfirmTask?.TrySetResult(false);
            }
            else if (IsInputDialog)
            {
                _currentInputTask?.TrySetResult(null);
            }
            IsMessageVisible = false;
            _currentConfirmTask = null;
            _currentInputTask = null;
        }

        public void CloseMessage()
        {
            IsMessageVisible = false;
            _currentConfirmTask = null;
            _currentInputTask = null;
        }



        [RelayCommand]
        public void CloseCommand()
        {
            CloseMessage();
        }



        [RelayCommand]
        public void ConfirmCommand()
        {
            ConfirmMessage();
        }

        private (string Color, string Icon) GetTypeColorAndIcon(MessageType type)
        {
            return type switch
            {
                MessageType.Success => ("#107C10", "fa-check-circle"),
                MessageType.Warning => ("#FF8C00", "fa-exclamation-triangle"),
                MessageType.Error => ("#D13438", "fa-times-circle"),
                MessageType.Information => ("#0078D4", "fa-info-circle"),
                _ => ("#0078D4", "fa-info-circle")
            };
        }

        [RelayCommand]
        public void CancelCommand()
        {
            CancelMessage();
        }
    }
}