using Microsoft.UI.Xaml;
#pragma warning disable CA1416

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FangJia;

/// <summary>
/// 提供特定于应用程序的行为以补充默认的 Application 类。
/// </summary>
public partial class App
{
    /// <summary>
    /// 初始化单例应用程序对象。这是执行的第一行编写代码，因此是 main() 或 WinMain() 的逻辑等效项。
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 在启动应用程序时调用。
    /// </summary>
    /// <param name="args">有关启动请求和过程的详细信息。</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();

    }

    private Window? _window;
}
