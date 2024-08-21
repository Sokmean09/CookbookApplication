using System.Diagnostics;
using System.IO;
using CookbookApplication.Models;
using NPOI.XWPF.UserModel;

namespace CookbookApplication.Services
{
    internal class DocDocxFileService : IFileService
    {
        public List<Recipe> Open(string fileName)
        {
            throw new NotImplementedException("DOCX reading is not implemented.");
        }

        public void Save(string fileName, List<Recipe> recipeList)
        {
            XWPFDocument document = new();

            foreach (Recipe recipe in recipeList)
            {
                XWPFParagraph recipeParagraph = document.CreateParagraph();
                XWPFRun recipeRun = recipeParagraph.CreateRun();

                //string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Resources", Path.GetFileName(recipe.ImagePath));
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources", recipe.ImagePath);

                if (File.Exists(imagePath))
                {
                    using (FileStream imageStream = new(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        recipeRun.AddPicture(imageStream, (int)PictureType.JPEG, Path.GetFileName(imagePath), 1000000, 1000000);
                    }
                }
                else
                {
                    Debug.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());
                    Debug.WriteLine("Image file not found at: " + imagePath);
                }

                recipeParagraph = document.CreateParagraph();
                recipeRun = recipeParagraph.CreateRun();
                recipeRun.IsBold = true;
                recipeRun.SetText("Name: ");
                recipeRun = recipeParagraph.CreateRun();
                recipeRun.SetText($"{recipe.Name}");
                recipeRun.AddCarriageReturn();

                recipeParagraph = document.CreateParagraph();
                recipeRun = recipeParagraph.CreateRun();
                recipeRun.IsBold = true;
                recipeRun.SetText("Type: ");
                recipeRun = recipeParagraph.CreateRun();
                recipeRun.SetText($"{recipe.Type}");
                recipeRun.AddCarriageReturn();

                recipeParagraph = document.CreateParagraph();
                recipeRun = recipeParagraph.CreateRun();
                recipeRun.IsBold = true;
                recipeRun.SetText("Cuisine: ");
                recipeRun = recipeParagraph.CreateRun();
                recipeRun.SetText($"{recipe.Cuisine}");

                if (recipe.Ingredients != null && recipe.Ingredients.Count > 0)
                {
                    XWPFParagraph ingredientsHeader = document.CreateParagraph();
                    XWPFRun ingredientsHeaderRun = ingredientsHeader.CreateRun();
                    ingredientsHeaderRun.AddCarriageReturn();
                    ingredientsHeaderRun.IsBold = true;
                    ingredientsHeaderRun.SetText("Ingredients:");

                    foreach (Ingredient ingredient in recipe.Ingredients)
                    {
                        XWPFParagraph ingredientParagraph = document.CreateParagraph();
                        XWPFRun ingredientRun = ingredientParagraph.CreateRun();
                        ingredientRun.SetText($"{ingredient.Name}: {ingredient.Quantity}");
                    }
                }

                if (recipe.Instructions != null && recipe.Instructions.Count > 0)
                {
                    XWPFParagraph instructionsHeader = document.CreateParagraph();
                    XWPFRun instructionsHeaderRun = instructionsHeader.CreateRun();
                    instructionsHeaderRun.AddCarriageReturn();
                    instructionsHeaderRun.IsBold = true;
                    instructionsHeaderRun.SetText("Instructions:");

                    foreach (Instruction instruction in recipe.Instructions)
                    {
                        XWPFParagraph instructionParagraph = document.CreateParagraph();
                        XWPFRun instructionRun = instructionParagraph.CreateRun();
                        instructionRun.SetText(instruction.Name);
                    }
                }

                // Add a page break between recipes
                XWPFParagraph breakParagraph = document.CreateParagraph();
                XWPFRun breakRun = breakParagraph.CreateRun();
                breakRun.AddBreak(BreakType.PAGE);
            }

            // Save to a file
            using (FileStream fileStream = new(fileName, FileMode.Create, FileAccess.Write))
            {
                document.Write(fileStream);
            }
        }
    }
}
