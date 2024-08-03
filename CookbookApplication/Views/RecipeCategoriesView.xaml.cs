using CookbookApplication.ViewModels;
using System.Windows.Controls;

namespace CookbookApplication.Views
{
    /// <summary>
    /// Interaction logic for RecipeCategoriesView.xaml
    /// </summary>
    public partial class RecipeCategoriesView : UserControl
    {
        public RecipeCategoriesView()
        {
            InitializeComponent();

            DataContext = new RecipeViewModel();
            
        }
    }
}
