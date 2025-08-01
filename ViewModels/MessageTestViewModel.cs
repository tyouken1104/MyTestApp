using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyTestApp.Services;
using System.Threading.Tasks;

namespace MyTestApp.ViewModels
{
    public partial class MessageTestViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        private readonly IFileDialogService _fileDialogService;

        public MessageTestViewModel(IMessageService messageService, IFileDialogService fileDialogService)
        {
            _messageService = messageService;
            _fileDialogService = fileDialogService;
        }

        [RelayCommand]
        private void ShowToast()
        {
            _messageService.ShowToast("这是一个Toast消息！", MessageType.Success);
        }

        [RelayCommand]
        private void ShowToastWarning()
        {
            _messageService.ShowToast("这是一个警告消息！请注意查看", MessageType.Warning);
        }

        [RelayCommand]
        private void ShowToastError()
        {
            _messageService.ShowToast("这是一个错误消息！操作失败", MessageType.Error);
        }

        [RelayCommand]
        private void ShowToastInfo()
        {
            _messageService.ShowToast("这是一个普通信息消息", MessageType.Information);
        }

        [RelayCommand]
        private void ShowMultipleToasts()
        {
            _messageService.ShowToast("第一条消息：操作开始", MessageType.Information);
            _messageService.ShowToast("第二条消息：处理中...", MessageType.Warning);
            _messageService.ShowToast("第三条消息：操作完成", MessageType.Success);
            _messageService.ShowToast("第四条消息：出现错误", MessageType.Error);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task ShowMessage()
        {
            await _messageService.ShowMessage("这是一条包含大量内容的测试消息，用于演示消息弹窗的自动扩大和滚动功能。\n\n第一部分：项目介绍\n这是一个基于Avalonia的跨平台桌面应用程序，采用MVVM架构模式，提供了现代化的用户界面和丰富的交互体验。\n\n第二部分：功能特性\n- 支持多种消息类型：普通消息、确认对话框、输入对话框\n- 提供Toast通知系统，支持成功、警告、错误、信息四种类型\n- 响应式设计，支持不同屏幕尺寸\n- 主题切换功能，支持明暗两种主题\n- 动画效果，提供流畅的用户体验\n\n第三部分：技术栈\n- 前端框架：Avalonia UI\n- 架构模式：MVVM (Model-View-ViewModel)\n- 依赖注入：Microsoft.Extensions.DependencyInjection\n- 消息通信：CommunityToolkit.Mvvm\n- 图标库：Avalonia.Fonts.FontAwesome\n\n第四部分：使用说明\n本应用程序提供了直观的用户界面，通过左侧导航菜单可以访问不同的功能模块。每个功能都经过精心设计，确保用户能够轻松完成各种操作任务。\n\n第五部分：注意事项\n- 请确保您的系统满足最低配置要求\n- 建议在高分辨率显示器上使用以获得最佳体验\n- 定期更新应用程序以获得最新功能和安全补丁\n\n第六部分：反馈建议\n如果您在使用过程中遇到任何问题或有改进建议，欢迎通过应用内的反馈功能与我们联系。我们将持续优化产品体验，为您提供更好的服务。\n\n这是一条超长的测试消息，用于验证消息弹窗的自动扩大和滚动功能是否正常工作。当消息内容超过一定长度时，应该自动出现纵向滚动条，同时保持弹窗的美观和易用性。","欢迎使用消息服务");
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task ShowConfirm()
        {
            var result = await _messageService.ShowConfirm("您确定要执行此操作吗？\n\n此操作将演示确认对话框的使用方式，包含大量内容测试：\n\n操作详情：\n1. 这将影响系统的多个组件\n2. 操作完成后无法撤销\n3. 可能需要几分钟时间完成\n4. 建议在操作前保存当前工作\n\n注意事项：\n- 请确保网络连接稳定\n- 关闭其他可能干扰的程序\n- 操作过程中不要关闭应用程序\n\n风险提示：\n此操作涉及数据变更，虽然我们会尽力确保数据安全，但仍建议您在操作前备份重要数据。\n\n确认要继续执行此操作吗？\n\n这是一个超长的确认消息，用于测试确认对话框的自动扩大和滚动功能。当消息内容超过一定长度时，应该自动出现纵向滚动条，同时保持弹窗的美观和易用性。","确认操作");
            if (result)
            {
                _messageService.ShowToast("操作已确认", MessageType.Success);
            }
            else
            {
                _messageService.ShowToast("操作已取消", MessageType.Information);
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        private async Task ShowInput()
        {
            var result = await _messageService.ShowInput("请输入您的详细信息：\n\n输入要求：\n1. 请提供您的完整姓名\n2. 联系方式（电话或邮箱）\n3. 简短的个人介绍\n4. 您对产品的使用反馈\n\n示例格式：\n姓名：张三\n邮箱：zhangsan@example.com\n简介：我是一名软件开发者，对用户体验设计很感兴趣\n反馈：界面很美观，但希望能增加更多快捷键支持\n\n注意事项：\n- 请确保提供的信息准确无误\n- 我们会严格保护您的个人信息\n- 您的反馈将帮助我们改进产品\n- 输入内容将在提交后进行审核\n\n隐私声明：\n您提供的信息将仅用于产品改进和用户体验优化，不会用于任何商业目的或与第三方分享。我们遵循严格的隐私保护政策。\n\n这是一个超长的输入提示消息，用于测试输入对话框的自动扩大和滚动功能。当消息内容超过一定长度时，应该自动出现纵向滚动条，同时保持弹窗的美观和易用性。\n\n请输入您的信息：","用户输入", "在此输入您的详细信息...");
            if (!string.IsNullOrEmpty(result))
            {
                _messageService.ShowToast($"收到输入：{result}", MessageType.Success);
            }
        }

        [RelayCommand]
        private async Task OpenFileDialog()
        {
            var filters = new[]
            {
                new FileDialogFilter("文本文件", "txt", "md", "log"),
                new FileDialogFilter("图片文件", "png", "jpg", "jpeg", "gif", "bmp"),
                new FileDialogFilter("所有文件", "*")
            };

            var result = await _fileDialogService.OpenFileDialogAsync(
                "选择要打开的文件",
                null,
                false,
                filters);

            if (result.Success && result.SelectedPath != null)
            {
                _messageService.ShowToast($"已选择文件：{result.SelectedPath}", MessageType.Success);
            }
            else
            {
                _messageService.ShowToast("取消选择文件", MessageType.Information);
            }
        }

        [RelayCommand]
        private async Task OpenMultipleFilesDialog()
        {
            var filters = new[]
            {
                new FileDialogFilter("图片文件", "png", "jpg", "jpeg", "gif", "bmp"),
                new FileDialogFilter("文档文件", "doc", "docx", "pdf", "txt"),
                new FileDialogFilter("所有文件", "*")
            };

            var result = await _fileDialogService.OpenFileDialogAsync(
                "选择多个文件",
                null,
                true,
                filters);

            if (result.Success && result.FilePaths?.Length > 0)
            {
                var fileList = string.Join("\n", result.FilePaths);
                await _messageService.ShowMessage("选择的文件", $"已选择 {result.FilePaths.Length} 个文件：\n\n{fileList}", MessageType.Success);
            }
            else
            {
                _messageService.ShowToast("取消选择文件", MessageType.Information);
            }
        }

        [RelayCommand]
        private async Task SaveFileDialog()
        {
            var filters = new[]
            {
                new FileDialogFilter("文本文件", "txt"),
                new FileDialogFilter("Markdown文件", "md"),
                new FileDialogFilter("JSON文件", "json"),
                new FileDialogFilter("所有文件", "*")
            };

            var result = await _fileDialogService.SaveFileDialogAsync(
                "保存文件",
                null,
                "新建文件.txt",
                filters);

            if (result.Success && result.SelectedPath != null)
            {
                _messageService.ShowToast($"文件将保存到：{result.SelectedPath}", MessageType.Success);
            }
            else
            {
                _messageService.ShowToast("取消保存文件", MessageType.Information);
            }
        }

        [RelayCommand]
        private async Task OpenFolderDialog()
        {
            var result = await _fileDialogService.OpenFolderDialogAsync(
                "选择文件夹",
                null);

            if (result.Success && result.SelectedPath != null)
            {
                _messageService.ShowToast($"已选择文件夹：{result.SelectedPath}", MessageType.Success);
            }
            else
            {
                _messageService.ShowToast("取消选择文件夹", MessageType.Information);
            }
        }
    }
}