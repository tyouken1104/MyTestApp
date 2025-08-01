using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyTestApp.Services;
using System.Collections.Generic;

namespace MyTestApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IThemeService _themeService;
        private readonly IMessageService _messageService;
        private readonly IFileDialogService _fileDialogService;
        
        [ObservableProperty]
        private bool _isDarkTheme;

        [ObservableProperty]
        private List<string> _themes = new() { "系统默认", "亮色主题", "暗色主题" };

        [ObservableProperty]
        private string _selectedTheme;

        [ObservableProperty]
        private MessageViewModel _messageViewModel;

        [ObservableProperty]
        private object _currentContent;

        public MainWindowViewModel(IThemeService themeService, IMessageService messageService, IFileDialogService fileDialogService)
        {
            _themeService = themeService;
            _messageService = messageService;
            _fileDialogService = fileDialogService;
            IsDarkTheme = _themeService.IsDarkTheme();
            
            // 根据当前主题初始化选择
            if (_themeService.IsSystemTheme())
                SelectedTheme = "系统默认";
            else if (IsDarkTheme)
                SelectedTheme = "暗色主题";
            else
                SelectedTheme = "亮色主题";

            MessageViewModel = new MessageViewModel(null);
            
            // 默认显示欢迎页面
            CurrentContent = new WelcomeViewModel();
        }

        [RelayCommand]
        private void ToggleTheme()
        {
            if (IsDarkTheme)
            {
                _themeService.SetLightTheme();
                IsDarkTheme = false;
            }
            else
            {
                _themeService.SetDarkTheme();
                IsDarkTheme = true;
            }
        }

        [RelayCommand]
        private void SetDarkTheme()
        {
            _themeService.SetDarkTheme();
            IsDarkTheme = true;
        }

        [RelayCommand]
        private void SetLightTheme()
        {
            _themeService.SetLightTheme();
            IsDarkTheme = false;
        }

        partial void OnSelectedThemeChanged(string value)
        {
            switch (value)
            {
                case "系统默认":
                    _themeService.SetSystemTheme();
                    IsDarkTheme = _themeService.IsDarkTheme();
                    break;
                case "亮色主题":
                    _themeService.SetLightTheme();
                    IsDarkTheme = false;
                    break;
                case "暗色主题":
                    _themeService.SetDarkTheme();
                    IsDarkTheme = true;
                    break;
            }
        }

        [RelayCommand]
        private void ShowMessageTest()
        {
            CurrentContent = new MessageTestViewModel(_messageService, _fileDialogService);
        }

        [RelayCommand]
        private void ShowWelcome()
        {
            CurrentContent = new WelcomeViewModel();
        }
    }
}