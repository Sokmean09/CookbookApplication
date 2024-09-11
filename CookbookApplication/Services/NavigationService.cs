using CookbookApplication.Views;
using System.Windows.Controls;

namespace CookbookApplication.Services
{
    class NavigationService : INavigationService
    {
        private readonly Frame mainFrame;

        public NavigationService(Frame mainFrame)
        {
            this.mainFrame = mainFrame;
        }

        public void NavigateTo(string pageKey, object viewModel)
        {
            Page? page;
            switch (pageKey)
            {
                case "Page1":
                    page = new Page1();
                    break;
                case "Page2":
                    page = new Page2();
                    break;
                default:
                    throw new ArgumentException("Unknown page key", nameof(pageKey));
            }

            if (page != null)
            {
                page.DataContext = viewModel;
                mainFrame.Content = page;
            }
        }
    }
}
