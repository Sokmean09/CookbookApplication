using static CookbookApplication.Services.DefaultDialogService;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using CookbookApplication.Services;
using CookbookApplication.Models;
using CookbookApplication.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CookbookApplication.ViewModels
{
    internal partial class RecipeViewModel : ObservableObject
    {
        public string FilePath { get; set; }

        [ObservableProperty]
        private Recipe? selectedRecipe;

        [ObservableProperty]
        private string? searchText;

        public ObservableCollection<Recipe> Recipes { get; set; }

        public ObservableCollection<Recipe> FilteredRecipes { get; set; }

        private readonly IDialogService dialogService;
        private readonly IFileService pdfFileService;
        private readonly IFileService docxFileService;
        private readonly IFileService jsonFileService;

        partial void OnSelectedRecipeChanged(Recipe? value)
        {
            RemoveRecipeCommand.NotifyCanExecuteChanged();
            EditRecipeCommand.NotifyCanExecuteChanged();
            ExportRecipeCommand.NotifyCanExecuteChanged();
        }

        partial void OnSearchTextChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                SearchParameter(new object());
            }
        }

        public List<string> RecipeTypeNames { get; set; } =
        [
            "Main-dish", "Side-dish", "Dessert", 
            "Breakfast", "Lunch", "Dinner",
            "Salad", "Soup", "Stew", "Snack", "Baked-goods", "Others"
        ];

        public List<string> RecipeCuisineNames { get; set; } =
        [
            "American", "British", "Chinese", "French", "Italian", "Japanese", "Khmer", 
            "Mexican", "Spanish", "Thai", "Vietnamese", "Others"
        ];

        public RecipeViewModel(IDialogService dialogService, IFileService docxFileService, IFileService pdfFileService, IFileService jsonFileService)
        {
            this.dialogService = dialogService;
            this.pdfFileService = pdfFileService;
            this.docxFileService = docxFileService;
            this.jsonFileService = jsonFileService;

            FilePath = Directory.GetCurrentDirectory() + "..\\..\\..\\..\\Recipes.json";
            Recipes =
            [
                new Recipe
                {
                    Name = "Fried Rice", 
                    Type = "Main-dish", 
                    Cuisine = "Khmer", 
                    Ingredients = [
                        new Ingredient { Name = "rice", Quantity = "100g"}, 
                        new Ingredient { Name = "egg" , Quantity = "1 whole"}
                    ],
                    Instructions = [
                        new Instruction { Name = "crack egg"},
                        new Instruction { Name = "stir"}
                    ],
                    Imagepath = @"\Resources\fried_rice.jpg" }
            ];

            if (!LoadRecipesFromFile())
            {
                SaveRecipesToFile();
            }
            FilteredRecipes = new(Recipes);
        }

        [RelayCommand]
        private void AddRecipe(object parameter)
        {
            Recipes.Add(new Recipe
            {
                Name = "New Name",
                Type = "New Type",
                Cuisine = "New Cuisine",
                Imagepath = "..\\Resources\\image.png",
                Ingredients = [new Ingredient { Name = "New Ingredient", Quantity = "1" }],
                Instructions = [new Instruction { Name = "New Instruction" }],
                About_detail = "Description"
            });
            SearchParameter(new object());
        }

        [RelayCommand(CanExecute = nameof(CanRemoveRecipe))]
        private void RemoveRecipe(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Delete the recipe?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                Recipes.Remove(SelectedRecipe);
                SearchParameter(new object());
            }
        }
        private bool CanRemoveRecipe(object parameter)
        {
            return IsRecipeSelected();
        }

        [RelayCommand(CanExecute = nameof(CanEditRecipe))]
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
            return IsRecipeSelected();
        }

        [RelayCommand(CanExecute = nameof(CanExportRecipe))]
        private void ExportRecipe(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(DefaultDialogService.FileType.All))
                {
                    List<Recipe> list = [SelectedRecipe];

                    string filePath = dialogService.FilePath;
                    string fileExtension = Path.GetExtension(filePath).ToLower();

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
            return IsRecipeSelected();
        }

        [RelayCommand]
        private void AddIngredient(object parameter)
        {
            SelectedRecipe?.Ingredients?.Add(new Ingredient { Name = "New Ingredient", Quantity = "1" });
        }

        [RelayCommand(CanExecute = nameof(CanRemoveIngredient))]
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

        [RelayCommand]
        private void AddInstruction(object parameter)
        {
            SelectedRecipe?.Instructions?.Add(new Instruction { Name = "New Instruction" });
        }

        [RelayCommand(CanExecute = nameof(CanRemoveInstruction))]
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

        [RelayCommand]
        private void EditImage(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedRecipe.Imagepath = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }

        [RelayCommand]
        private void SearchParameter(object parameter)
        {
            List<string>? searchParameters = SearchText?
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(term => term.ToLower())
                .ToList();

            if (searchParameters == null || searchParameters.Count == 0)
            {
                FilteredRecipes.Clear();
                foreach (Recipe recipe in Recipes)
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

        [RelayCommand]
        private void SaveDocDocxFile(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(FileType.Word))
                {
                    docxFileService.Save(dialogService.FilePath, ReturnRecipesToList());
                    dialogService.ShowMessage("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        [RelayCommand]
        private void SavePdfFile(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(FileType.Pdf))
                {
                    pdfFileService.Save(dialogService.FilePath, ReturnRecipesToList());
                    dialogService.ShowMessage("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        [RelayCommand]
        private void SaveJsonFile(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(FileType.Json))
                {
                    jsonFileService.Save(dialogService.FilePath, ReturnRecipesToList());
                    dialogService.ShowMessage("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        [RelayCommand]
        private void OpenJsonFile(object parameter)
        {
            if (dialogService.OpenFileDialog())
            {
                List<Recipe> recipeList = jsonFileService.Open(dialogService.FilePath);
                Recipes = new(recipeList);
                FilteredRecipes.Clear();
                foreach (Recipe recipe in Recipes)
                {
                    FilteredRecipes.Add(recipe);
                }
                return;
            }
        }

        [RelayCommand]
        private void DataGridTips(object parameter)
        {
            MessageBox.Show("Alt + Right Arrow : Expand Grid\nAlt + Left Arrow : Shrink Grid", "Tips", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshRecipes()
        {

        }

        private List<Recipe> ReturnRecipesToList()
        {
            return Recipes.Select(recipe => new Recipe
            {
                Name = recipe?.Name,
                Type = recipe?.Type,
                Cuisine = recipe?.Cuisine,
                Imagepath = recipe?.Imagepath,
                Ingredients = recipe?.Ingredients,
                Instructions = recipe?.Instructions,
                About_detail = recipe?.About_detail,
            }).ToList();
        }

        private bool IsRecipeSelected()
        {
            return SelectedRecipe != null;
        }

        public bool LoadRecipesFromFile()
        {
            if (!File.Exists(FilePath))
            {
                return false;
            }
            List<Recipe> recipeList = jsonFileService.Open(FilePath);
            Recipes = new(recipeList);
            FilteredRecipes = new(Recipes);
            return true;
        }

        public void SaveRecipesToFile()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            if (Recipes != null)
            {
                jsonFileService.Save(FilePath, [.. Recipes]);
            }
        }
    }
}
