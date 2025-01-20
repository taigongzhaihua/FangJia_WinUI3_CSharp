using CommunityToolkit.Mvvm.ComponentModel;
using FangJia.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FangJia;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty] private bool _isFullScreen;
    [ObservableProperty] private ObservableCollection<Category> _pageHeader = [];
    [ObservableProperty] private ObservableCollection<CategoryBase> _menuFolders = [];
    [ObservableProperty] private ObservableCollection<CategoryBase> _footFolders = [];
    [ObservableProperty] private Dictionary<string, CategoryBase> _categoryList = [];

    public MainPageViewModel()
    {
        CategoryList = new Dictionary<string, CategoryBase>()
        {
            { "Home", new Category { Name = "首页", Glyph = "\uE80F", Path = "HomePage", Tooltip = "首页" } },
            { "Data", new Category { Name = "数据", Glyph = "\uE8F1", Path = "DataPage", Tooltip = "数据：维护数据库内容，包括方剂、药物、经典、医案等。", } },
            { "About", new Category { Name = "关于", Glyph = "\uE946", Path = "AboutPage", Tooltip = "关于：软件版权及版本信息。" } },
            // 数据子项
            { "Formulation", new Category { Name = "方剂", Glyph = "\uE8A1", Path = "FormulationPage", Tooltip = "方剂：维护方剂信息。" } },
            { "Drug", new Category { Name = "药物", Glyph = "\uE8A1", Path = "DrugPage", Tooltip = "药物：维护药物信息。" } },
            { "Classic", new Category { Name = "经典", Glyph = "\uE8A1", Path = "ClassicPage", Tooltip = "经典：维护经典信息。" } },
            { "Case", new Category { Name = "医案", Glyph = "\uE8A1", Path = "CasePage", Tooltip = "医案：维护医案信息。" } },
            // 设置
            { "Settings", new Category { Glyph = "\uE713", Name = "设置", Path = "SettingsPage", Tooltip = "设置" } }
        };
        var dataCategory = CategoryList["Data"] as Category;
        dataCategory!.Children =
        [
            CategoryList["Formulation"],
            CategoryList["Drug"],
            CategoryList["Classic"],
            CategoryList["Case"]
        ];
        MenuFolders =
        [
            CategoryList["Home"],
            dataCategory
        ];
        FootFolders =
        [
            CategoryList["About"],
            new Separator()
        ];
    }
}