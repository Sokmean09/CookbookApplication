using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Windows;
using CookbookApplication.Services;
using CookbookApplication.Models;
using CookbookApplication.Views;
using static CookbookApplication.Services.DefaultDialogService;
using System.Windows.Markup;

namespace CookbookApplication.ViewModels
{
    internal class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe selectedRecipe;
        public ObservableCollection<Recipe> Recipes { get; set; }
        public ObservableCollection<Recipe> FilteredRecipes { get; set; }
        private Visibility searchBarVisibility = Visibility.Collapsed;
        private Visibility sortBarVisibility = Visibility.Collapsed;
        private string? searchText;
        private string? sortType;
        private string? sortCuisine;
        private readonly IDialogService dialogService;
        private readonly IFileService pdfFileService;
        private readonly IFileService docxFileService;

        public Recipe SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                selectedRecipe = value;
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
                if (SearchText == string.Empty)
                {
                    SearchParameters(new object());
                }
            }
        }

        public string? SortType
        {
            get => sortType;
            set
            {
                sortType = value;
                OnPropertyChanged();
            }
        }

        public string? SortCuisine
        {
            get => sortCuisine;
            set
            {
                sortCuisine = value;
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
            FilteredRecipes = new(Recipes);

            AddRecipeCommand = new(AddRecipe);
            RemoveRecipeCommand = new(RemoveRecipe, CanRemoveRecipe);
            EditRecipeCommand = new(EditRecipe, CanEditRecipe);
            ExportRecipeCommand = new(ExportRecipe, CanExportRecipe);

            AddIngredientCommand = new(AddIngredient);
            AddInstructionCommand = new(AddInstruction);
            RemoveIngredientCommand = new(RemoveIngredient, CanRemoveIngredient);
            RemoveInstructionCommand = new(RemoveInstruction, CanRemoveInstruction);
            EditImageCommand = new(EditImage);

            ToggleSearchBarCommand = new(ToggleSearchBar);
            SearchParameterCommand = new(SearchParameters);

            SaveDocDocxFileCommand = new(SaveDocDocxFile);
            SavePdfFileCommand = new(SavePdfFile);
        }

        public RelayCommand AddRecipeCommand { get; }
        public RelayCommand RemoveRecipeCommand { get; }
        public RelayCommand EditRecipeCommand { get; }
        public RelayCommand ExportRecipeCommand { get; }

        public RelayCommand AddIngredientCommand { get; }
        public RelayCommand RemoveIngredientCommand { get; }
        public RelayCommand AddInstructionCommand { get; }
        public RelayCommand RemoveInstructionCommand { get; }
        public RelayCommand EditImageCommand { get; }

        public RelayCommand ToggleSearchBarCommand { get; }
        public RelayCommand SearchParameterCommand { get; }

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
            SearchParameters(new object());
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
                SearchParameters(new object());
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

        private void ExportRecipe(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(DefaultDialogService.FileType.All))
                {
                    List<Recipe> list = [SelectedRecipe];

                    string filePath = dialogService.FilePath;
                    string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                    if (fileExtension == ".doc" || fileExtension == ".docx")
                    {
                        docxFileService.Save(filePath, list);
                    }
                    else if (fileExtension == ".pdf")
                    {
                        pdfFileService.Save(filePath, list);
                    }
                    else
                    {
                        dialogService.ShowMessage("Unsupported file type.");
                        return;
                    }

                    dialogService.ShowMessage("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        private bool CanExportRecipe(object parameter)
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

        private void SearchParameters(object parameter)
        {
            List<string>? searchParameters = SearchText?
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(term => term.ToLower())
                .ToList();

            // If the search text is empty or contains only whitespace, show all recipes
            if (searchParameters == null || searchParameters.Count == 0)
            {
                FilteredRecipes.Clear();
                foreach (var recipe in Recipes)
                {
                    FilteredRecipes.Add(recipe);
                }
                return;
            }

            FilteredRecipes.Clear();

            foreach (Recipe recipe in Recipes)
            {
                if (searchParameters.Any(searchString =>
                    recipe.Name?.ToLower().Contains(searchString) == true ||
                    recipe.Type?.ToLower().Contains(searchString) == true ||
                    recipe.Cuisine?.ToLower().Contains(searchString) == true))
                {
                    if (!FilteredRecipes.Contains(recipe))
                    {
                        FilteredRecipes.Add(recipe);
                    }
                }
            }
        }

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
                    dialogService.ShowMessage("File saved successfully.");
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
                    dialogService.ShowMessage("File saved successfully.");
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
