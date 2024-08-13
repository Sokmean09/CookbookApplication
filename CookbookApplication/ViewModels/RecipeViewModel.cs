using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Windows;
using CookbookApplication.Services;
using CookbookApplication.Models;
using CookbookApplication.Views;
using static CookbookApplication.Services.DefaultDialogService;

namespace CookbookApplication.ViewModels
{
    internal class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe? selectedRecipe;
        public ObservableCollection<Recipe?> recipes = [];
        private Visibility searchBarVisibility = Visibility.Collapsed;
        private Visibility sortBarVisibility = Visibility.Collapsed;
        private string? searchText;
        private string? sortType;
        private string? sortCuisine;
        private readonly IDialogService dialogService;
        private readonly IFileService pdfFileService;
        private readonly IFileService docxFileService;

        public Recipe? SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Recipe?> Recipes
        {
            get => recipes;
            set
            {
                recipes = value;
                OnPropertyChanged();
            }
        }

        public Visibility SearchBarVisibility
        {
            get => searchBarVisibility;
            set
            {
                searchBarVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility SortBarVisibility
        {
            get => sortBarVisibility;
            set
            {
                sortBarVisibility = value;
                OnPropertyChanged();
            }
        }

        public string? SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }

        public string? SortType
        {
            get => sortType;
            set
            {
                sortType = value;
                OnPropertyChanged();
                //ApplySorting();
            }
        }

        public string? SortCuisine
        {
            get => sortCuisine;
            set
            {
                sortCuisine = value;
                OnPropertyChanged();
                //ApplySorting();
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
            "Soup",
            "Stew",
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

        public RecipeViewModel(IDialogService dialogService, IFileService docxFileService, IFileService pdfFileService)
        {
            this.dialogService = dialogService;
            this.pdfFileService = pdfFileService;
            this.docxFileService = docxFileService;

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
                    ImagePath = "..\\Resources\\fried_rice.jpg" },
                new Recipe 
                {
                    Name = "red curry",
                    Type = "Soup", 
                    Cuisine = "Thai",
                    Ingredients = [],
                    Instructions = [],
                    ImagePath = "..\\Resources\\thai-red-curry-with-chicken.jpg" }
            ];
            AddRecipeCommand = new(AddRecipe);
            RemoveRecipeCommand = new(RemoveRecipe, CanRemoveRecipe);
            EditRecipeCommand = new(EditRecipe, CanEditRecipe);
            AddIngredientCommand = new(AddIngredient);
            AddInstructionCommand = new(AddInstruction);
            RemoveIngredientCommand = new(RemoveIngredient, CanRemoveIngredient);
            RemoveInstructionCommand = new(RemoveInstruction, CanRemoveInstruction);
            EditImageCommand = new(EditImage);
            ToggleSearchBarCommand = new(ToggleSearchBar);
            /*ToggleSortBarCommand = new(ToggleSortBar);
            ResetSortBarCommand = new(ResetSortBar);*/
            SaveDocDocxFileCommand = new(SaveDocDocxFile);
            SavePdfFileCommand = new(SavePdfFile);
            /*Recipes.Add(new Recipe { Name = "Recipe1", Type = "Side-dish", Cuisine = "Chinese", ImagePath = "..\\Resources\\fried_rice.jpg" });
            Recipes.Add(new Recipe { Name = "Recipe2", Type = "Main", Cuisine = "Italian", ImagePath = "..\\Resources\\fried_rice.jpg" });
            Recipes.Add(new Recipe { Name = "Recipe3", Type = "Dessert", Cuisine = "French", ImagePath = "..\\Resources\\fried_rice.jpg" })*/;
        }

        public RelayCommand AddRecipeCommand { get; }
        public RelayCommand RemoveRecipeCommand { get; }
        public RelayCommand EditRecipeCommand { get; }
        public RelayCommand AddIngredientCommand { get; }
        public RelayCommand RemoveIngredientCommand { get; }
        public RelayCommand AddInstructionCommand { get; }
        public RelayCommand RemoveInstructionCommand { get; }
        public RelayCommand EditImageCommand { get; }
        public RelayCommand ToggleSearchBarCommand { get; }
        /* public RelayCommand ToggleSortBarCommand { get; }
         public RelayCommand ResetSortBarCommand { get; }*/
        public RelayCommand SaveDocDocxFileCommand { get; }
        public RelayCommand SavePdfFileCommand { get; }

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
                SelectedRecipe.Ingredients?.Remove(ingredient);
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
                SelectedRecipe.ImagePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }

        private void ToggleSearchBar(object parameter)
        {
            SearchBarVisibility = SearchBarVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        /*private void ToggleSortBar(object parameter)
        {
            SortBarVisibility = SortBarVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ResetSortBar(object parameter)
        {
            SortType = null;
            SortCuisine = null;
            ApplySorting();
        }
        private void ApplySorting()
        {
            var collectionView = CollectionViewSource.GetDefaultView(Recipes);
            if (collectionView == null) return;

            collectionView.SortDescriptions.Clear();

            if (!string.IsNullOrEmpty(SortType))
            {
                collectionView.SortDescriptions.Add(new SortDescription(nameof(Recipe.Type), ListSortDirection.Ascending));
            }

            if (!string.IsNullOrEmpty(SortCuisine))
            {
                collectionView.SortDescriptions.Add(new SortDescription(nameof(Recipe.Cuisine), ListSortDirection.Ascending));
            }

            collectionView.Refresh();
        }*/

        private void SaveDocDocxFile(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(FileType.Word))
                {
                    docxFileService.Save(dialogService.FilePath,
                        Recipes.Select(recipe => new Recipe
                        { Name = recipe?.Name, Type = recipe?.Type, Cuisine = recipe?.Cuisine, ImagePath = recipe?.ImagePath,
                            Ingredients = recipe?.Ingredients, Instructions = recipe?.Instructions }).ToList());
                    dialogService.ShowMessage("File saved");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        private void SavePdfFile(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(FileType.Pdf))
                {
                    pdfFileService.Save(dialogService.FilePath,
                        Recipes.Select(recipe => new Recipe
                        { Name = recipe?.Name, Type = recipe?.Type, Cuisine = recipe?.Cuisine, ImagePath = recipe?.ImagePath,
                            Ingredients = recipe?.Ingredients, Instructions = recipe?.Instructions }).ToList());
                    dialogService.ShowMessage("File saved");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
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
