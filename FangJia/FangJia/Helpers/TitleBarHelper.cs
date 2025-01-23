﻿using Microsoft.UI;
using Microsoft.UI.Xaml;

namespace FangJia.Helpers
{

    internal class TitleBarHelper
    {
        // workaround as Appwindow titlebar doesn't update caption button colors correctly when changed while app is running
        // 解决方法，因为在应用程序运行时更改时，AppWindow 标题栏不会正确更新标题按钮颜色
        // https://task.ms/44172495

        public static Windows.UI.Color ApplySystemThemeToCaptionButtons(Window window)
        {
            var color = ThemeHelper.ActualTheme == ElementTheme.Dark ? Colors.White : Colors.Black;
            SetCaptionButtonColors(window, color);
            return color;
        }

        public static void SetCaptionButtonColors(Window window, Windows.UI.Color color)
        {
            window.AppWindow.TitleBar.ButtonForegroundColor = color;
            window.AppWindow.TitleBar.ForegroundColor = color;
        }

        public static void SetCaptionButtonBackgroundColors(Window window, Windows.UI.Color? color)
        {
            var titleBar = window.AppWindow.TitleBar;
            titleBar.ButtonBackgroundColor = color;
        }

        public static void SetForegroundColor(Window window, Windows.UI.Color? color)
        {
            var titleBar = window.AppWindow.TitleBar;
            titleBar.ForegroundColor = color;
        }

        public static void SetBackgroundColor(Window window, Windows.UI.Color? color)
        {
            var titleBar = window.AppWindow.TitleBar;
            titleBar.BackgroundColor = color;
        }
    }
}
