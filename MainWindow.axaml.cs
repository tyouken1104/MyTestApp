using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Timers;
using Avalonia.Platform;
using MyTestApp.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Avalonia.Media;
using System;
using Avalonia.Animation;

namespace MyTestApp;

public partial class MainWindow : Window
{
        private DateTime _lastClickTime;
        private const double DoubleClickTime = 500; // 毫秒;

        public MainWindow()
            {
                InitializeComponent();
                // DataContext现在由依赖注入自动设置
                
                // 设置窗口样式
                this.SystemDecorations = SystemDecorations.None;
                this.TransparencyLevelHint = new[] { WindowTransparencyLevel.Transparent };
                
                // 设置初始边框
                var windowBorder = this.FindControl<Border>("WindowBorder");
                if (windowBorder != null)
                {
                    windowBorder.BorderThickness = WindowState == WindowState.Maximized ? new Thickness(0) : new Thickness(2);
                }
                
                // 查找并绑定窗口控制按钮
                var minimizeButton = this.FindControl<Button>("MinimizeButton");
                var maximizeButton = this.FindControl<Button>("MaximizeButton");
                var closeButton = this.FindControl<Button>("CloseButton");

                if (minimizeButton != null)
                    minimizeButton.Click += OnMinimizeClick;
                
                if (maximizeButton != null)
                    maximizeButton.Click += OnMaximizeClick;
                
                if (closeButton != null)
                    closeButton.Click += OnCloseClick;

                // 设置菜单交互
                SetupMenuInteractions();
                
                // 设置窗口大小调整
                SetupWindowResize();
            }

    private void OnMinimizeClick(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void OnMaximizeClick(object? sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            if (sender is Button button)
            {
                var icon = new Projektanker.Icons.Avalonia.Icon
                {
                    Value = "fa-window-maximize",
                    FontSize = 14
                };
                button.Content = icon;
            }
        }
        else
        {
            WindowState = WindowState.Maximized;
            if (sender is Button button)
            {
                var icon = new Projektanker.Icons.Avalonia.Icon
                {
                    Value = "fa-window-restore",
                    FontSize = 14
                };
                button.Content = icon;
            }
        }
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SetupMenuInteractions()
    {
        var toggleButton = this.FindControl<Button>("ToggleMenuButton");
        var systemIcon = this.FindControl<Button>("SystemIcon");
        var businessIcon = this.FindControl<Button>("BusinessIcon");
        var themeIcon = this.FindControl<Button>("ThemeIcon");
        var messageTestIcon = this.FindControl<Button>("MessageTestIcon");
        
        var systemSubMenu = this.FindControl<StackPanel>("SystemSubMenu");
        var businessSubMenu = this.FindControl<StackPanel>("BusinessSubMenu");
        var themeSubMenu = this.FindControl<StackPanel>("ThemeSubMenu");
        var messageTestSubMenu = this.FindControl<StackPanel>("MessageTestSubMenu");

        if (toggleButton != null)
            toggleButton.Click += (s, e) => ToggleMenu();
        
        if (systemIcon != null && systemSubMenu != null)
            systemIcon.Click += (s, e) => ShowSubMenu(systemSubMenu);
        
        if (businessIcon != null && businessSubMenu != null)
            businessIcon.Click += (s, e) => ShowSubMenu(businessSubMenu);
        
        if (themeIcon != null && themeSubMenu != null)
            themeIcon.Click += (s, e) => ShowSubMenu(themeSubMenu);

        if (messageTestIcon != null && messageTestSubMenu != null)
            messageTestIcon.Click += (s, e) => ShowSubMenu(messageTestSubMenu);
    }
    
    private void ToggleMenu()
    {
        var subMenuPanel = this.FindControl<Border>("SubMenuPanel");
        var menuPanel = this.FindControl<Border>("MenuPanel");
        var toggleButton = this.FindControl<Button>("ToggleMenuButton");

        if (subMenuPanel == null || menuPanel == null || toggleButton == null)
            return;

        var isExpanded = subMenuPanel.IsVisible;
        
        if (isExpanded)
        {
            subMenuPanel.IsVisible = false;
            menuPanel.Width = 50;
            
            var icon = toggleButton.FindControl<Projektanker.Icons.Avalonia.Icon>("ToggleIcon");
            if (icon != null)
                icon.Value = "fa-bars";
            ToolTip.SetTip(toggleButton, "展开菜单");
        }
        else
        {
            subMenuPanel.IsVisible = true;
            menuPanel.Width = 250;
            
            var icon = toggleButton.FindControl<Projektanker.Icons.Avalonia.Icon>("ToggleIcon");
            if (icon != null)
                icon.Value = "fa-angle-double-left";
            ToolTip.SetTip(toggleButton, "收缩菜单");
        }
    }

    private void ShowSubMenu(StackPanel subMenu)
    {
        var subMenuPanel = this.FindControl<Border>("SubMenuPanel");
        if (subMenuPanel != null && !subMenuPanel.IsVisible)
        {
            ToggleMenu();
        }

        HideAllSubMenus();
        subMenu.IsVisible = true;
    }
    
    private void HideAllSubMenus()
    {
        var systemSubMenu = this.FindControl<StackPanel>("SystemSubMenu");
        var businessSubMenu = this.FindControl<StackPanel>("BusinessSubMenu");
        var themeSubMenu = this.FindControl<StackPanel>("ThemeSubMenu");
        var messageTestSubMenu = this.FindControl<StackPanel>("MessageTestSubMenu");
        
        if (systemSubMenu != null) systemSubMenu.IsVisible = false;
        if (businessSubMenu != null) businessSubMenu.IsVisible = false;
        if (themeSubMenu != null) themeSubMenu.IsVisible = false;
        if (messageTestSubMenu != null) messageTestSubMenu.IsVisible = false;
    }

    private void InputTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            // 获取ViewModel并触发取消命令
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.MessageViewModel.CancelMessage();
            }
            e.Handled = true;
        }
    }

    private void MessageDialogHost_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            // 只有当对话框打开时才处理回车键
            if (DataContext is MainWindowViewModel viewModel && 
                viewModel.MessageViewModel.IsMessageVisible)
            {
                viewModel.MessageViewModel.CancelMessage();
            }
            e.Handled = true;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        
        if (change.Property == WindowStateProperty)
        {
            var maximizeButton = this.FindControl<Button>("MaximizeButton");
            if (maximizeButton != null)
            {
                var icon = new Projektanker.Icons.Avalonia.Icon
                {
                    Value = WindowState == WindowState.Maximized ? "fa-window-restore" : "fa-window-maximize",
                    FontSize = 14
                };
                maximizeButton.Content = icon;
            }

            var titleBar = this.FindControl<Grid>("TitleBar");
            var windowBorder = this.FindControl<Border>("WindowBorder");
            
            if (titleBar != null)
            {
                if (WindowState == WindowState.Maximized)
                {
                    titleBar.Margin = new Thickness(0);
                    if (windowBorder != null)
                        windowBorder.BorderThickness = new Thickness(0);
                }
                else
                {
                    titleBar.Margin = new Thickness(5, 5, 5, 0);
                    if (windowBorder != null)
                        windowBorder.BorderThickness = new Thickness(2);
                }
            }
        }
    }

    private void OnHeaderPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            // 检查是否双击
            var currentTime = DateTime.Now;
            var timeDiff = (currentTime - _lastClickTime).TotalMilliseconds;
            _lastClickTime = currentTime;

            if (timeDiff < DoubleClickTime)
            {
                // 双击事件 - 切换最大化/恢复
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                else
                {
                    WindowState = WindowState.Maximized;
                }
            }
            else
            {
                // 单击事件 - 开始拖拽
                BeginMoveDrag(e);
            }
        }
    }

    private void SetupWindowResize()
    {
        // 直接在窗口级别处理大小调整事件
        this.PointerMoved += OnResizePointerMoved;
        this.PointerPressed += OnResizePointerPressed;
        this.PointerReleased += OnResizePointerReleased;
    }

    private void OnResizePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
            return;

        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsLeftButtonPressed)
        {
            var position = e.GetPosition(this);
            var edge = GetResizeEdge(position);
            
            if (edge.HasValue)
            {
                BeginResizeDrag(edge.Value, e);
                e.Handled = true;
            }
            // 不在边缘区域时不处理事件，让事件传递给下层控件
        }
    }

    private void OnResizePointerMoved(object? sender, PointerEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
            return;

        var position = e.GetPosition(this);
        var edge = GetResizeEdge(position);
        
        if (edge.HasValue)
        {
            Cursor = GetCursorForEdge(edge.Value);
        }
        else
        {
            Cursor = Cursor.Default;
        }
    }

    private void OnResizePointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        Cursor = Cursor.Default;
    }

    private WindowEdge? GetResizeEdge(Point position)
    {
        const double edgeSize = 8;
        
        var width = Bounds.Width;
        var height = Bounds.Height;
        
        if (position.X < 0 || position.X > width || position.Y < 0 || position.Y > height)
            return null;
            
        var leftEdge = position.X <= edgeSize;
        var rightEdge = position.X >= width - edgeSize;
        var topEdge = position.Y <= edgeSize;
        var bottomEdge = position.Y >= height - edgeSize;
        
        if (leftEdge && topEdge) return WindowEdge.NorthWest;
        if (rightEdge && topEdge) return WindowEdge.NorthEast;
        if (leftEdge && bottomEdge) return WindowEdge.SouthWest;
        if (rightEdge && bottomEdge) return WindowEdge.SouthEast;
        if (leftEdge) return WindowEdge.West;
        if (rightEdge) return WindowEdge.East;
        if (topEdge) return WindowEdge.North;
        if (bottomEdge) return WindowEdge.South;
        
        return null;
    }

    private Cursor GetCursorForEdge(WindowEdge edge)
    {
        return edge switch
        {
            WindowEdge.North or WindowEdge.South => new Cursor(StandardCursorType.SizeNorthSouth),
            WindowEdge.East or WindowEdge.West => new Cursor(StandardCursorType.SizeWestEast),
            WindowEdge.NorthEast or WindowEdge.SouthWest => new Cursor(StandardCursorType.BottomLeftCorner),
            WindowEdge.NorthWest or WindowEdge.SouthEast => new Cursor(StandardCursorType.BottomRightCorner),
            _ => Cursor.Default
        };
    }
    
    private void ToastBorder_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // 获取DataContext中的ViewModel并调用CloseCommand
        if (DataContext is MainWindowViewModel viewModel)
        {
            // 由于这是在ItemsControl中的DataTemplate，我们需要找到对应的DataContext
            if (sender is Border border && border.DataContext is ToastMessage toastMessage)
            {
                // 创建RemoveToastMessage并发送给MessageViewModel处理
                var removeMessage = new RemoveToastMessage(toastMessage);
                WeakReferenceMessenger.Default.Send(removeMessage);
            }
        }
    }
    
    private void ToastBorder_PointerEntered(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            // 创建轻微放大的动画效果
            var scaleTransform = border.RenderTransform as ScaleTransform ?? new ScaleTransform(1, 1);
            scaleTransform.ScaleX = 1.05;
            scaleTransform.ScaleY = 1.05;
            border.RenderTransform = scaleTransform;
        }
    }

    private void ToastBorder_PointerExited(object? sender, PointerEventArgs e)
    {
        if (sender is Border border)
        {
            // 恢复正常大小
            var scaleTransform = border.RenderTransform as ScaleTransform ?? new ScaleTransform(1, 1);
            scaleTransform.ScaleX = 1;
            scaleTransform.ScaleY = 1;
            border.RenderTransform = scaleTransform;
        }
    }
}