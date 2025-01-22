using FangJia.Common;
using FangJia.Helpers;
using FangJia.ViewModel;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FangJia.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DataPage
    {
        public DataPage()
        {
            InitializeComponent();
            ViewModel = Locator.GetService<DataViewModel>();
        }

        internal DataViewModel ViewModel;

        private void Collection_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is not Category item) return;
            Frame.Navigate(Helper.GetType(item.Path));
        }
    }
}
