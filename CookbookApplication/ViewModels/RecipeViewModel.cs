using CookbookApplication.Services;
using CookbookApplication.Models;
using CookbookApplication.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;

namespace CookbookApplication.ViewModels
{
    internal class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe? selectedRecipe;
        public ObservableCollection<Recipe?> Recipes { get; set; }

        public Recipe? SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        public List<string> RecipeTypeNames { get; set; } =
        [
            "Main-dish",
            "Side-dish",
            "Dessert",
            "Breakfast",
            "Lunch",
            "Dinner",
            "Salad",
            "Soup/Stew",
            "Snack",
            "Baked-goods",
            "Others"
        ];

        public List<string> RecipeCuisineNames { get; set; } =
        [
            "American",
            "British",
            "Chinese",
            "French",
            "Japanese",
            "Khmer",
            "Mexican",
            "Spanish",
            "Thai",
            "Vietnamese",
            "Others"
        ];

        public RecipeViewModel()
        {
            Recipes =
            [
                new Recipe
                {
                    Name = "fry rice", 
                    Type = "Main-dish", 
                    Cuisine = "Khmer", 
                    Ingredients = [new Ingredient { Name = "rice", Quantity = "100g"}, 
                                   new Ingredient { Name = "egg" , Quantity = "1 whole"}],
                    Instructions = [new Instruction { Name = "crack egg"},
                                    new Instruction { Name = "stir"}],
                    Image = "..\\Resources\\fried_rice.jpg" },
                new Recipe 
                {
                    Name = "red curry",
                    Type = "Soup", 
                    Cuisine = "Thai",
                    Ingredients = [],
                    Instructions = [],
                    Image = "..\\Resources\\thai-red-curry-with-chicken.jpg" }
            ];
            AddRecipeCommand = new RelayCommand(AddRecipe);
            RemoveRecipeCommand = new RelayCommand(RemoveRecipe, CanRemoveRecipe);
            EditRecipeCommand = new RelayCommand(EditRecipe, CanEditRecipe);
            AddIngredientCommand = new RelayCommand(AddIngredient);
            AddInstructionCommand = new RelayCommand(AddInstruction);
            RemoveIngredientCommand = new RelayCommand(RemoveIngredient, CanRemoveIngredient);
            RemoveInstructionCommand = new RelayCommand(RemoveInstruction, CanRemoveInstruction);
            EditImageCommand = new RelayCommand(EditImage);
        }

        public RelayCommand AddRecipeCommand { get; }
        public RelayCommand RemoveRecipeCommand { get; }
        public RelayCommand EditRecipeCommand { get; }
        public RelayCommand AddIngredientCommand { get; }
        public RelayCommand RemoveIngredientCommand { get; }
        public RelayCommand AddInstructionCommand { get; }
        public RelayCommand RemoveInstructionCommand { get; }
        public RelayCommand EditImageCommand { get; }

        private void AddRecipe(object parameter)
        {
            Recipes.Add(new Recipe
            {
                Name = "New Name",
                Type = "New Type",
                Cuisine = "New Cuisine",
                Ingredients = [new Ingredient { Name = "New Ingredient", Quantity = "1" }],
                Instructions = [new Instruction { Name = "New Instruction" }]
            }) ;
        }

        private bool CanRemoveRecipe(object parameter)
        {
            return SelectedRecipe != null;
        }

        private void RemoveRecipe(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Delete the recipe?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                Recipes.Remove(SelectedRecipe);
            }
        }


        private void EditRecipe(object parameter)
        {
            RecipeEditView recipeEditView = new()
            {
                DataContext = this
            };
            recipeEditView.ShowDialog();
        }

        private bool CanEditRecipe(object parameter)
        {
            return SelectedRecipe != null;
        }

        private void AddIngredient(object parameter)
        {
            SelectedRecipe?.Ingredients?.Add(new Ingredient { Name = "New Ingredient", Quantity = "1" });
        }

        private void RemoveIngredient(object parameter)
        {
            if (parameter is Ingredient ingredient && SelectedRecipe != null)
            {
                SelectedRecipe?.Ingredients?.Remove(ingredient);
            }
        }

        private bool CanRemoveIngredient(object parameter)
        {
            return parameter is Ingredient ingredient && SelectedRecipe?.Ingredients?.Contains(ingredient) == true;
        }

        private void AddInstruction(object parameter)
        {
            SelectedRecipe?.Instructions?.Add(new Instruction { Name = "New Instruction" });
        }

        private void RemoveInstruction(object parameter)
        {
            if (parameter is Instruction instruction && SelectedRecipe != null)
            {
                SelectedRecipe?.Instructions?.Remove(instruction);
            }
        }

        private bool CanRemoveInstruction(object parameter)
        {
            return parameter is Instruction instruction && SelectedRecipe?.Instructions?.Contains(instruction) == true;
        }

        private void EditImage(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedRecipe.Image = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
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
