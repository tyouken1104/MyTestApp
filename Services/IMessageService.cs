using System.Threading.Tasks;

namespace MyTestApp.Services
{
    public enum MessageType
    {
        Information,
        Success,
        Warning,
        Error,
        Question
    }

    public class MessageResult
    {
        public bool IsConfirmed { get; set; }
        public string InputText { get; set; } = string.Empty;
    }

    public interface IMessageService
    {
        /// <summary>
        /// 显示普通消息
        /// </summary>
        Task ShowMessage(string message, string title = "提示", MessageType type = MessageType.Information);
        
        /// <summary>
        /// 显示确认对话框
        /// </summary>
        Task<bool> ShowConfirm(string message, string title = "确认", MessageType type = MessageType.Question);
        
        /// <summary>
        /// 显示输入对话框
        /// </summary>
        Task<string?> ShowInput(string message, string title = "输入", string defaultValue = "");
        
        /// <summary>
        /// 显示带回调的消息
        /// </summary>
        void ShowToast(string message, MessageType type = MessageType.Information, int duration = 3000);
    }
}