using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FangJia.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DataPage : Page
    {
        public DataPage()
        {
            this.InitializeComponent();
            items = ["方剂", "药物", "经典", "医案"];
        }
        private List<string> items { get; set; }
    }
}
