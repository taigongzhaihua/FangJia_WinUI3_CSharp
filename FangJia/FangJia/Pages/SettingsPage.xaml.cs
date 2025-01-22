// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using FangJia.Helpers;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace FangJia.Pages;

/// <summary>
/// 设置页面类，可单独使用或在 Frame 中导航使用。
/// </summary>
public sealed partial class SettingsPage
{

    public SettingsPage()
    {
        InitializeComponent();
    }

    private void themeMode_SelectionChanged(object sender, RoutedEventArgs e)
    {
        var selectedTheme = ((ComboBoxItem)ThemeMode.SelectedItem)?.Tag?.ToString();
        var window = WindowHelper.GetWindowForElement(this);
        string color;
        if (selectedTheme != null)
        {
            ThemeHelper.RootTheme = App.GetEnum<ElementTheme>(selectedTheme);
            if (selectedTheme == "Dark")
            {
                TitleBarHelper.SetCaptionButtonColors(window, Colors.White);
                color = selectedTheme;
            }
            else if (selectedTheme == "Light")
            {
                TitleBarHelper.SetCaptionButtonColors(window, Colors.Black);
                color = selectedTheme;
            }
            else
            {
                color = TitleBarHelper.ApplySystemThemeToCaptionButtons(window) == Colors.White ? "Dark" : "Light";
            }
            // announce visual change to automation
            UIHelper.AnnounceActionForAccessibility(sender as UIElement, $"Theme changed to {color}",
                "ThemeChangedNotificationActivityId");
        }
    }
}