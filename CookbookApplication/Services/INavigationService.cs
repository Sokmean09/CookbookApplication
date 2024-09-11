namespace CookbookApplication.Services
{
    interface INavigationService
    {
        void NavigateTo(string pageKey, object viewModel);
    }
}
