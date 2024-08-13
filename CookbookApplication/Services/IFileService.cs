using CookbookApplication.Models;

namespace CookbookApplication.Services
{
    internal interface IFileService
    {
        List<Recipe> Open(string fileName);
        void Save(string fileName, List<Recipe> recipeList);
    }
}
