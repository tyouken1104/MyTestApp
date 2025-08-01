using System;
using System.Threading.Tasks;

namespace MyTestApp.Services
{
    public class FileDialogResult
    {
        public bool Success { get; set; }
        public string[]? FilePaths { get; set; }
        public string? SelectedPath { get; set; }
    }

    public interface IFileDialogService
    {
        Task<FileDialogResult> OpenFileDialogAsync(
            string title = "选择文件",
            string? initialDirectory = null,
            bool allowMultiple = false,
            FileDialogFilter[]? filters = null);

        Task<FileDialogResult> SaveFileDialogAsync(
            string title = "保存文件",
            string? initialDirectory = null,
            string? defaultFileName = null,
            FileDialogFilter[]? filters = null);

        Task<FileDialogResult> OpenFolderDialogAsync(
            string title = "选择文件夹",
            string? initialDirectory = null);
    }

    public class FileDialogFilter
    {
        public string Name { get; set; } = string.Empty;
        public string[] Extensions { get; set; } = Array.Empty<string>();

        public FileDialogFilter() { }

        public FileDialogFilter(string name, params string[] extensions)
        {
            Name = name;
            Extensions = extensions;
        }
    }
}