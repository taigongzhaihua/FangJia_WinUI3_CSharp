using CommunityToolkit.Mvvm.ComponentModel;
using FangJia.Common;
using FangJia.Helpers;
using System.Collections.ObjectModel;

namespace FangJia.ViewModel;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty] private bool _isFullScreen;
    [ObservableProperty] private ObservableCollection<Category> _pageHeader = [];
    [ObservableProperty] private ObservableCollection<CategoryBase> _menuFolders = [];
    [ObservableProperty] private ObservableCollection<CategoryBase> _footFolders = [];

    public MainPageViewModel()
    {
        var dataCategory = Helper.CategoryList["Data"] as Category;
        dataCategory!.Children =
        [
            Helper.CategoryList["Formulation"],
            Helper.CategoryList["Drug"],
            Helper.CategoryList["Classic"],
            Helper.CategoryList["Case"]
        ];
        MenuFolders =
        [
            Helper.CategoryList["Home"],
            dataCategory
        ];
        FootFolders =
        [
            Helper.CategoryList["About"],
            new Separator()
        ];
    }

}