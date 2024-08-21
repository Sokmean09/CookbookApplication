using CookbookApplication.Models;
using System.IO;
using Newtonsoft.Json;

namespace CookbookApplication.Services
{
    class JsonFileService : IFileService
    {
        public List<Recipe> Open(string fileName)
        {
            List<Recipe> recipes = [];
            string jsonString = File.ReadAllText(fileName);
            recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonString);
            return recipes;
        }

        public void Save(string fileName, List<Recipe> recipes)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            string jsonString = JsonConvert.SerializeObject(recipes, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
