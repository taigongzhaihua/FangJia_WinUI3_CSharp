﻿using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace FangJia.Helpers
{

    internal class TitleBarHelper
    {
        // workaround as Appwindow titlebar doesn't update caption button colors correctly when changed while app is running
        // https://task.ms/44172495
        public static Windows.UI.Color ApplySystemThemeToCaptionButtons(Window window)
        {
            var mainWindow = (Application.Current as App)?.Window as MainWindow;
            var frame = (mainWindow?.Content as FrameworkElement)?.FindName("MainGrid") as FrameworkElement;
            var color = frame?.ActualTheme == ElementTheme.Dark ? Colors.White : Colors.Black;
            SetCaptionButtonColors(window, color);
            return color;
        }

        public static void SetCaptionButtonColors(Window window, Windows.UI.Color color)
        {
            var res = Application.Current.Resources;
            res["WindowCaptionForeground"] = new SolidColorBrush() { Color = color };
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
