using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyTestApp.Services;
using System.Threading.Tasks;

namespace MyTestApp.ViewModels
{
    public partial class TestMessageViewModel : ViewModelBase
    {
        private readonly IMessageService _messageService;
        private readonly IFileDialogService _fileDialogService;

        public TestMessageViewModel(IMessageService messageService, IFileDialogService fileDialogService)
        {
            _messageService = messageService;
            _fileDialogService = fileDialogService;
        }

        [RelayCommand]
        private async Task ShowInfoMessage()
        {
            await _messageService.ShowMessage("信息", "这是一个信息消息", MessageType.Information);
        }

        [RelayCommand]
        private async Task ShowSuccessMessage()
        {
            await _messageService.ShowMessage("成功", "操作已成功完成", MessageType.Success);
        }

        [RelayCommand]
        private async Task ShowWarningMessage()
        {
            await _messageService.ShowMessage("警告", "这是一个警告消息", MessageType.Warning);
        }

        [RelayCommand]
        private async Task ShowErrorMessage()
        {
            await _messageService.ShowMessage("错误", "发生了一个错误", MessageType.Error);
        }

        [RelayCommand]
        private async Task ShowConfirmDialog()
        {
            var result = await _messageService.ShowConfirm("确认", "确定要执行此操作吗？");
            if (result)
            {
                await _messageService.ShowMessage("结果", "用户点击了确认", MessageType.Success);
            }
            else
            {
                await _messageService.ShowMessage("结果", "用户点击了取消", MessageType.Information);
            }
        }

        [RelayCommand]
        private async Task ShowInputDialog()
        {
            var result = await _messageService.ShowInput("输入", "请输入您的姓名：", "张三");
            if (!string.IsNullOrEmpty(result))
            {
                await _messageService.ShowMessage("输入结果", $"您输入的是：{result}", MessageType.Success);
            }
        }

        [RelayCommand]
        private void ShowToast()
        {
            _messageService.ShowToast("这是一个Toast消息", MessageType.Success);
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
                await _messageService.ShowMessage("文件选择", $"已选择文件：{result.SelectedPath}", MessageType.Success);
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
                await _messageService.ShowMessage("保存文件", $"文件将保存到：{result.SelectedPath}", MessageType.Success);
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
                await _messageService.ShowMessage("文件夹选择", $"已选择文件夹：{result.SelectedPath}", MessageType.Success);
            }
            else
            {
                _messageService.ShowToast("取消选择文件夹", MessageType.Information);
            }
        }
    }
}