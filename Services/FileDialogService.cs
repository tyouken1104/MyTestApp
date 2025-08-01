using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyTestApp.Services
{
    public class FileDialogService : IFileDialogService
    {
        private readonly Window _mainWindow;

        public FileDialogService(Window mainWindow)
    {
        _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
    }

        public async Task<FileDialogResult> OpenFileDialogAsync(
            string title = "选择文件",
            string? initialDirectory = null,
            bool allowMultiple = false,
            FileDialogFilter[]? filters = null)
        {
            var options = new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = allowMultiple,
                FileTypeFilter = ConvertFilters(filters)
            };

            if (!string.IsNullOrEmpty(initialDirectory))
            {
                options.SuggestedStartLocation = await GetFolderFromPathAsync(initialDirectory);
            }

            var result = await _mainWindow.StorageProvider.OpenFilePickerAsync(options);

            return new FileDialogResult
            {
                Success = result.Count > 0,
                FilePaths = result.Select(f => f.TryGetLocalPath()).Where(p => p != null).ToArray()!,
                SelectedPath = result.Count > 0 ? result[0].TryGetLocalPath() : null
            };
        }

        public async Task<FileDialogResult> SaveFileDialogAsync(
            string title = "保存文件",
            string? initialDirectory = null,
            string? defaultFileName = null,
            FileDialogFilter[]? filters = null)
        {
            var options = new FilePickerSaveOptions
            {
                Title = title,
                FileTypeChoices = ConvertFilters(filters),
                DefaultExtension = filters?.FirstOrDefault()?.Extensions.FirstOrDefault(),
                SuggestedFileName = defaultFileName
            };

            if (!string.IsNullOrEmpty(initialDirectory))
            {
                options.SuggestedStartLocation = await GetFolderFromPathAsync(initialDirectory);
            }

            var result = await _mainWindow.StorageProvider.SaveFilePickerAsync(options);

            return new FileDialogResult
            {
                Success = result != null,
                FilePaths = result != null ? new[] { result.TryGetLocalPath()! } : null,
                SelectedPath = result?.TryGetLocalPath()
            };
        }

        public async Task<FileDialogResult> OpenFolderDialogAsync(
            string title = "选择文件夹",
            string? initialDirectory = null)
        {
            var options = new FolderPickerOpenOptions
            {
                Title = title,
                AllowMultiple = false
            };

            if (!string.IsNullOrEmpty(initialDirectory))
            {
                options.SuggestedStartLocation = await GetFolderFromPathAsync(initialDirectory);
            }

            var result = await _mainWindow.StorageProvider.OpenFolderPickerAsync(options);

            return new FileDialogResult
            {
                Success = result.Count > 0,
                FilePaths = result.Select(f => f.TryGetLocalPath()).Where(p => p != null).ToArray()!,
                SelectedPath = result.Count > 0 ? result[0].TryGetLocalPath() : null
            };
        }

        private FilePickerFileType[]? ConvertFilters(FileDialogFilter[]? filters)
        {
            if (filters == null || filters.Length == 0)
                return null;

            return filters.Select(f => new FilePickerFileType(f.Name)
            {
                Patterns = f.Extensions.Select(ext => $"*.{ext}").ToArray()
            }).ToArray();
        }

        private async Task<IStorageFolder?> GetFolderFromPathAsync(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            try
            {
                return await _mainWindow.StorageProvider.TryGetFolderFromPathAsync(path);
            }
            catch
            {
                return null;
            }
        }
    }
}