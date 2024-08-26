using System.Windows;
using CookbookApplication.Services;
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

            DataContext = new RecipeViewModel(new DefaultDialogService(), new DocDocxFileService(), new PdfFileService(), new JsonFileService());
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
