using Microsoft.Win32;
using System.Windows;

namespace CookbookApplication.Services
{
    internal class DefaultDialogService : IDialogService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public string FilePath { get; set; }
        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }

            return false;
        }

        public enum FileType
        {
            Word,
            Pdf,
            Json,
            All
        }

        public bool SaveFileDialog(FileType fileType)
        {
            SaveFileDialog saveFileDialog = new();

            SetSaveFileDialogExt(fileType, saveFileDialog);
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }

            return false;
        }

        private void SetSaveFileDialogExt(FileType fileType, SaveFileDialog saveFileDialog)
        {
            if (fileType == FileType.Word)
            {
                saveFileDialog.Filter = "Word Documents (*.doc)|*.doc|Word Documents (*.docx)|*.docx";
                saveFileDialog.DefaultExt = "docx"; // Default file extension
                saveFileDialog.AddExtension = true; // Add the selected extension to the file name
            }
            else if (fileType == FileType.Pdf)
            {
                saveFileDialog.Filter = "PDF Documents (*.pdf)|*.pdf";
                saveFileDialog.DefaultExt = "pdf"; // Default file extension for PDF
                saveFileDialog.AddExtension = true; // Add the selected extension to the file name
            }
            else if (fileType == FileType.Json)
            {
                saveFileDialog.Filter = "JSON Source File (*.json)|*.json";
                saveFileDialog.DefaultExt = "json"; // Default file extension for PDF
                saveFileDialog.AddExtension = true; // Add the selected extension to the file name
            }
            else
            {
                saveFileDialog.Filter = "PDF Documents (*.pdf)|*.pdf|Word Documents (*.doc)|*.doc|Word Documents (*.docx)|*.docx";
                saveFileDialog.DefaultExt = "pdf"; // Default file extension
                saveFileDialog.AddExtension = true; // Add the selected extension to the file name
            }
        }
    }
}
