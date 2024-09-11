using System.Windows;
using CookbookApplication.Views;
using CookbookApplication.ViewModels;
using CookbookApplication.Services;

namespace CookbookApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NavigationService navigationService;

        public MainWindow()
        {
            InitializeComponent();

            RecipeViewModel recipeViewModel = new(new DefaultDialogService(),
                                                      new DocDocxFileService(),
                                                      new PdfFileService(),
                                                      new JsonFileService());

            // Pass the frame for navigation and set the DataContext
            navigationService = new NavigationService(MainFrame);

            DataContext = recipeViewModel; // Set the MainWindow's DataContext

            // Make sure to pass the navigation service to the view model
            recipeViewModel.NavigationService = navigationService;

            Page1 page1 = new()
            {
                DataContext = DataContext
            };

            MainFrame.Content = page1;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RecipeViewModel? viewModel = DataContext as RecipeViewModel;

            if (viewModel != null)
            {
                // Call the method in the ViewModel that handles the save prompt
                MessageBoxResult result = MessageBox.Show("Do you want to save your changes before exiting?",
                                                      "Save Changes",
                                                      MessageBoxButton.YesNoCancel,
                                                      MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {

                        viewModel.SaveRecipesToFile();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    // Cancel the closing event
                    e.Cancel = true;
                }
            }
        }
    }
}
