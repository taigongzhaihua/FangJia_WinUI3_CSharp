using CommunityToolkit.Mvvm.ComponentModel;
using FangJia.Pages;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Graphics;
#pragma warning disable CA1416



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FangJia;

/// <summary>
/// 一个可以单独使用或在 Frame 中导航到的空窗口。
/// </summary>

public sealed partial class MainWindow
{
    private readonly AppWindow _appWindow;
    internal MainPageViewModel ViewModel;

    public MainWindow()
    {
        InitializeComponent();

        // 假设 "this" 是一个 XAML 窗口。在不使用 WinUI 3 1.3 或更高版本的项目中，使用互操作 API 获取 AppWindow。
        // WinUI 3 1.3 或更高版本，使用互操作 API 获取 AppWindow。

        ViewModel = new MainPageViewModel();
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
#if WINDOWS10_0_19041_0_OR_GREATER
        TitleBarTextBlock.Text = AppInfo.Current.DisplayInfo.DisplayName;
#endif
        // 设置窗口图标
        _appWindow.SetIcon("Assets/StoreLogo.ico");

        // 设置窗口标题栏按钮颜色
        _appWindow.TitleBar.ButtonForegroundColor =

            ((SolidColorBrush)Application.Current.Resources["WindowCaptionForeground"]).Color;

        _appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
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
                ViewModel.IsFullScreen = false;
                break;

            case AppWindowPresenterKind.FullScreen:
                // 全屏 - 隐藏自定义标题栏 和 默认的系统标题栏。
                // AppTitleBar.Visibility = Visibility.Collapsed;

                sender.TitleBar.ExtendsContentIntoTitleBar = true;
                _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;
                ViewModel.IsFullScreen = true;
                break;

            case AppWindowPresenterKind.Overlapped:
                // 正常 - 隐藏系统标题栏 并 使用自定义标题栏。
                AppTitleBar.Visibility = Visibility.Visible;
                sender.TitleBar.ExtendsContentIntoTitleBar = true;
                _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
                ViewModel.IsFullScreen = false;
                break;

            case AppWindowPresenterKind.Default:
            default:
                // 使用默认的系统标题栏。
                sender.TitleBar.ResetToDefault();
                sender.TitleBar.ExtendsContentIntoTitleBar = true;
                _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
                ViewModel.IsFullScreen = false;
                break;
        }
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
            ViewModel.PageHead.Clear();
            ViewModel.PageHead.Add(new Folder { Name = "设置" });
            return;
        }
        var selectedTag = args.SelectedItemContainer.Tag as string;
        if (string.IsNullOrEmpty(selectedTag)) return;
        switch (selectedTag)
        {
            case "HomePage":
                ContentFrame.Navigate(typeof(HomePage));
                if (ViewModel.PageHead.Count > 0) ViewModel.PageHead.Clear();
                ViewModel.PageHead.Add(new Folder { Name = "首页" });
                break;
            case "DataPage":
                ContentFrame.Navigate(typeof(DataPage));
                if (ViewModel.PageHead.Count > 0) ViewModel.PageHead.Clear();
                ViewModel.PageHead.Add(new Folder { Name = "数据" });
                break;
            case "AboutPage":
                ContentFrame.Navigate(typeof(AboutPage));
                if (ViewModel.PageHead.Count > 0) ViewModel.PageHead.Clear();
                ViewModel.PageHead.Add(new Folder { Name = "关于" });
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
        _appWindow.SetPresenter(ViewModel.IsFullScreen
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
    private void BreadcrumbBar2_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        var items = PageTitleBreadcrumbBar.ItemsSource as ObservableCollection<Folder>;
        for (var i = items!.Count - 1; i >= args.Index + 1; i--)
        {
            items.RemoveAt(i);
        }
    }

    private void NavigationViewControl_OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        // 当 NavigationView 的返回按钮被点击时，执行 Frame 的返回操作
        if (ContentFrame.CanGoBack)
        {
            ContentFrame.GoBack();
        }
    }
}
#pragma warning disable MVVMTK0045
internal partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty] private bool _isFullScreen;
    [ObservableProperty] private ObservableCollection<Folder> _pageHead = [];
}
#pragma warning restore MVVMTK0045

public class Folder
{
    public string? Name { get; set; }
}


