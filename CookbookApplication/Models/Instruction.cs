using CommunityToolkit.Mvvm.ComponentModel;

namespace CookbookApplication.Models
{
    public partial class Instruction : ObservableObject
    {
        [ObservableProperty]
        private string? name;
    }
}
