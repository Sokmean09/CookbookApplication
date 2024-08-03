using System.Windows;
using CookbookApplication.ViewModels;

namespace CookbookApplication.Views
{
    /// <summary>
    /// Interaction logic for RecipeView.xaml
    /// </summary>
    public partial class RecipeView : Window
    {
        public RecipeView()
        {
            InitializeComponent();

            DataContext = new RecipeViewModel();
        }
    }
}
