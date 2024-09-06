using CommunityToolkit.Mvvm.ComponentModel;

namespace CookbookApplication.Models
{
    public partial class Ingredient : ObservableObject
    {
        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? quantity;
    }
}
