using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Graphics;
using AboutPage = FangJia.Pages.AboutPage;
using HomePage = FangJia.Pages.HomePage;
using SettingsPage = FangJia.Pages.SettingsPage;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FangJia;

/// <summary>
/// 一个可以单独使用或在 Frame 中导航到的空窗口。
/// </summary>

public sealed partial class MainWindow
{
    private readonly AppWindow _appWindow;
    private bool _isFullScreen;
    private ObservableCollection<string> _pageHead;

    [SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
    public MainWindow()
    {
        InitializeComponent();

        // 假设 "this" 是一个 XAML 窗口。在不使用 WinUI 3 1.3 或更高版本的项目中，使用互操作 API 获取 AppWindow。
        // WinUI 3 1.3 或更高版本，使用互操作 API 获取 AppWindow。

        _appWindow = AppWindow;
        _appWindow.Changed += AppWindow_Changed;
        Activated += MainWindow_Activated;
        AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        AppTitleBar.Loaded += AppTitleBar_Loaded;

        ExtendsContentIntoTitleBar = true;
        if (ExtendsContentIntoTitleBar)
        {
            _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
        }
        SetTitleBar(AppTitleBar);
        TitleBarTextBlock.Text = AppInfo.Current.DisplayInfo.DisplayName;
        // 设置窗口图标
        _appWindow.SetIcon("Assets/StoreLogo.ico");
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        if (ExtendsContentIntoTitleBar)
        {
            // Set the initial interactive regions.
            // 设置初始交互区域。
            SetRegionsForCustomTitleBar();
        }
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (ExtendsContentIntoTitleBar)
        {
            // Update interactive regions if the size of the window changes.
            // 如果窗口大小发生变化，更新交互区域。

            SetRegionsForCustomTitleBar();
        }
    }

    private void SetRegionsForCustomTitleBar()
    {
        // 指定标题栏的交互区域。

        var scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

        RightPaddingColumn.Width = new GridLength(_appWindow.TitleBar.RightInset / scaleAdjustment);
        LeftPaddingColumn.Width = new GridLength(_appWindow.TitleBar.LeftInset / scaleAdjustment);



        // 获取 PersonPicture 控件周围的矩形。
        var transform = PersonPic.TransformToVisual(null);
        var bounds = transform.TransformBounds(new Rect(0, 0,
                                                    PersonPic.ActualWidth,
                                                    PersonPic.ActualHeight));
        var personPicRect = GetRect(bounds, scaleAdjustment);

        // 获取全屏按钮周围的矩形。
        transform = FullScreenButton.TransformToVisual(null);
        bounds = transform.TransformBounds(new Rect(0, 0,
                                                    FullScreenButton.ActualWidth,
                                                    FullScreenButton.ActualHeight));
        var fullScreenRect = GetRect(bounds, scaleAdjustment);

        var rectArray = new[] { personPicRect, fullScreenRect };

        var nonClientInputSrc =
            InputNonClientPointerSource.GetForWindowId(AppWindow.Id);
        nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
    }

    private static RectInt32 GetRect(Rect bounds, double scale) => new(
            _X: (int)Math.Round(bounds.X * scale),
            _Y: (int)Math.Round(bounds.Y * scale),
            _Width: (int)Math.Round(bounds.Width * scale),
            _Height: (int)Math.Round(bounds.Height * scale)
        );

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState == WindowActivationState.Deactivated)
        {
            TitleBarTextBlock.Foreground =
                (SolidColorBrush)Application.Current.Resources["WindowCaptionForegroundDisabled"];
        }
        else
        {
            TitleBarTextBlock.Foreground =
                (SolidColorBrush)Application.Current.Resources["WindowCaptionForeground"];
        }
    }

    private void AppWindow_Changed(AppWindow sender, AppWindowChangedEventArgs args)
    {
        if (!args.DidPresenterChange) return;
        switch (sender.Presenter.Kind)
        {
            case AppWindowPresenterKind.CompactOverlay:
                // 紧凑覆盖 - 隐藏自定义标题栏 并 使用默认的系统标题栏。
                AppTitleBar.Visibility = Visibility.Collapsed;
                sender.TitleBar.ResetToDefault();
                sender.TitleBar.ExtendsContentIntoTitleBar = true;
                _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
                _isFullScreen = false;
                break;

            case AppWindowPresenterKind.FullScreen:
                // 全屏 - 隐藏自定义标题栏 和 默认的系统标题栏。
                // AppTitleBar.Visibility = Visibility.Collapsed;

                sender.TitleBar.ExtendsContentIntoTitleBar = true;
                _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
                _isFullScreen = true;
                break;

            case AppWindowPresenterKind.Overlapped:
                // 正常 - 隐藏系统标题栏 并 使用自定义标题栏。
                AppTitleBar.Visibility = Visibility.Visible;
                sender.TitleBar.ExtendsContentIntoTitleBar = true;
                _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
                _isFullScreen = false;
                break;

            case AppWindowPresenterKind.Default:
            default:
                // 使用默认的系统标题栏。

                sender.TitleBar.ResetToDefault();
                sender.TitleBar.ExtendsContentIntoTitleBar = true;
                _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
                _isFullScreen = false;
                break;
        }
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
            return;
        }
        var selectedTag = args.SelectedItemContainer.Tag as string;
        if (string.IsNullOrEmpty(selectedTag)) return;
        switch (selectedTag)
        {
            case "HomePage":
                ContentFrame.Navigate(typeof(HomePage));
                break;
            case "AboutPage":
                ContentFrame.Navigate(typeof(AboutPage));
                break;
        }
    }

    private void NavigationView_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is NavigationView navigationView)
        {
            navigationView.SelectedItem = navigationView.MenuItems[0];
        }
    }
    // 切换窗口显示模式
    private void FullScreen(object sender, RoutedEventArgs e)
    {
        _appWindow.SetPresenter(_isFullScreen
            ? AppWindowPresenterKind.Default
            : AppWindowPresenterKind.FullScreen);
    }
    private void OnPaneDisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = sender.PaneDisplayMode switch
        {
            NavigationViewPaneDisplayMode.Top => new Thickness(16, 0, 0, 0),
            _ => sender.DisplayMode == NavigationViewDisplayMode.Minimal
                ? new Thickness(96, 0, 0, 0)
                : new Thickness(48, 0, 0, 0)
        };
    }
}

