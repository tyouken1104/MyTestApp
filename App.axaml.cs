using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using MyTestApp.Services;
using MyTestApp.ViewModels;
using System;

namespace MyTestApp;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        // 注册ViewLocator
        DataTemplates.Add(new ViewLocator());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();
            var fileDialogService = new FileDialogService(mainWindow);
            
            // 创建MainWindowViewModel时注入文件对话框服务
            var mainWindowViewModel = ActivatorUtilities.CreateInstance<MainWindowViewModel>(_serviceProvider, fileDialogService);
            mainWindow.DataContext = mainWindowViewModel;
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        // 注册ViewModels
        services.AddTransient<MainWindowViewModel>();
        
        // 注册Services
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<IMessenger, WeakReferenceMessenger>();
        services.AddSingleton<IMessageService, MessageService>();
    }
}