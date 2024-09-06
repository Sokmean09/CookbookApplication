using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace CookbookApplication.Models
{
    public partial class Recipe : ObservableObject
    {
        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? type;

        [ObservableProperty]
        private string? cuisine;

        [ObservableProperty]
        private string? imagepath;

        [ObservableProperty]
        private ObservableCollection<Ingredient>? ingredients;

        [ObservableProperty]
        private ObservableCollection<Instruction>? instructions;

        [ObservableProperty]
        private string? about_detail;
    }
}
