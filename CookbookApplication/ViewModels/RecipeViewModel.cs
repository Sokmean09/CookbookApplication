using CookbookApplication.Services;
using CookbookApplication.Models;
using CookbookApplication.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CookbookApplication.ViewModels
{
    internal class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe? _selectedRecipe;
        public ObservableCollection<Recipe?> Recipes { get; set; }

        public Recipe? SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddRecipeCommand { get; }
        public ICommand RemoveRecipeCommand { get; }
        public ICommand EditRecipeCommand { get; }

        public RecipeViewModel()
        {
            Recipes = new ObservableCollection<Recipe?>
            {
                new Recipe {Name = "fry rice", Type = "Main dishes", Cuisine = "Cambodia", 
                    Ingredients = [new Ingredient { Name = "rice"}, 
                                   new Ingredient { Name = "egg"}
                    ],
                    Image = "C:\\Users\\USER\\source\\repos\\CookbookApplication\\CookbookApplication\\Resources\\fried_rice.jpg" },
                new Recipe {Name = "red curry", Type = "Soup", Cuisine = "Thai",
                    Image = "C:\\Users\\USER\\source\\repos\\CookbookApplication\\CookbookApplication\\Resources\\thai-red-curry-with-chicken.jpg" }
            };
            AddRecipeCommand = new RelayCommand(AddRecipe);
            RemoveRecipeCommand = new RelayCommand(RemoveRecipe, CanRemoveRecipe);
            EditRecipeCommand = new RelayCommand(EditRecipe, CanEditRecipe);
        }

        private void AddRecipe(object parameter)
        {
            Recipes.Add(new Recipe { Name = "New recipe"});
        }

        private bool CanRemoveRecipe(object parameter)
        {
            return SelectedRecipe != null;
        }

        private void RemoveRecipe(object parameter)
        {
            Recipes.Remove(SelectedRecipe);
        }

        private void EditRecipe(object parameter)
        {
            RecipeEditView recipeEditView = new();
            recipeEditView.DataContext = SelectedRecipe;
            recipeEditView.ShowDialog();
        }

        private bool CanEditRecipe(object parameter)
        {
            return SelectedRecipe != null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
