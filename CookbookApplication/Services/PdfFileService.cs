using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using CookbookApplication.Models;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Media.Media3D;

namespace CookbookApplication.Services
{
    internal class PdfFileService : IFileService
    {
        public List<Recipe> Open(string fileName)
        {
            throw new System.NotImplementedException("PDF reading is not implemented.");
        }

        public void Save(string fileName, List<Recipe> recipes)
        {
            PdfDocument document = new();
            document.Info.Title = "Food List";

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new("Verdana", 12);
            double yPoint = 50;
            double imageSize = 100; // Size of the image
            double pageHeight = page.Height.Point;
            double marginBottom = 50; // Margin at the bottom of the page

            for (int i = 0; i < recipes.Count; i++)
            {
                Recipe recipe = recipes[i];
                // Image
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Resources", recipe.ImagePath);
                if (File.Exists(imagePath))
                {
                    XImage image = XImage.FromFile(imagePath);
                    Debug.WriteLine(image.Size.ToString());
                    if (yPoint + imageSize + 10 > pageHeight - marginBottom)
                    {
                        // Add a new page if there's not enough space for the image
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPoint = 50;
                    }
                    gfx.DrawImage(image, 40, yPoint, imageSize, imageSize);
                    yPoint += imageSize + 10;
                }
                else
                {
                    Debug.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());
                    Debug.WriteLine("Image file not found at: " + imagePath);
                }

                // Information
                gfx.DrawString($"Name: {recipe.Name}", font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                yPoint += 20;
                gfx.DrawString($"Type: {recipe.Type}", font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                yPoint += 20;
                gfx.DrawString($"Cuisine: {recipe.Cuisine}", font, XBrushes.Black, new XRect(40, yPoint, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
                yPoint += 20;

                // Ingredients
                if (recipe.Ingredients != null && recipe.Ingredients.Count > 0)
                {
                    yPoint = DrawText(gfx, font, "Ingredients:", yPoint, pageHeight, marginBottom);
                    foreach (Ingredient ingredient in recipe.Ingredients)
                    {
                        yPoint = DrawText(gfx, font, $"{ingredient.Name}: {ingredient.Quantity}", yPoint, pageHeight, marginBottom);
                    }
                }

                // Instructions
                if (recipe.Instructions != null && recipe.Instructions.Count > 0)
                {
                    yPoint = DrawText(gfx, font, "Instructions:", yPoint, pageHeight, marginBottom);
                    foreach (Instruction instruction in recipe.Instructions)
                    {
                        yPoint = DrawText(gfx, font, instruction.Name, yPoint, pageHeight, marginBottom);
                    }
                }

                if (i < recipes.Count - 1)
                {
                    // Add a page break between recipes
                    yPoint += 20; // Add some space before the page xbreak
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = 50; // Reset yPoint after the page break
                }
            }

            // Save to a file
            using (FileStream fileStream = new(fileName, FileMode.Create, FileAccess.Write))
            {
                document.Save(fileStream);
            }
        }
        private double DrawText(XGraphics gfx, XFont font, string text, double yPoint, double pageHeight, double marginBottom)
        {
            if (yPoint + 20 > pageHeight - marginBottom) // Check if there's enough space for the text
            {
                // Add a new page if there's not enough space for the text
                PdfPage newPage = gfx.PdfPage.Owner.AddPage();
                gfx = XGraphics.FromPdfPage(newPage);
                yPoint = 50; // Reset yPoint to the top of the new page
            }
            gfx.DrawString(text, font, XBrushes.Black, new XRect(40, yPoint, gfx.PdfPage.Width.Point, gfx.PdfPage.Height.Point), XStringFormats.TopLeft);
            return yPoint + 20; // Return the updated yPoint after drawing the text
        }
    }
}
