using CommunityToolkit.Mvvm.ComponentModel;
using FangJia.Common;
using FangJia.Helpers;
using System.Collections.ObjectModel;

namespace FangJia.ViewModel
{
    public partial class DataViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Category> _data =
        [
            (Helper.CategoryList["Formulation"] as Category)!,
            (Helper.CategoryList["Drug"] as Category)!,
            (Helper.CategoryList["Classic"] as Category)!,
            (Helper.CategoryList["Case"] as Category)!
        ];
    }
}
