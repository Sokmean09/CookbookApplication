using static CookbookApplication.Services.DefaultDialogService;

namespace CookbookApplication.Services
{
    internal interface IDialogService
    {
        void ShowMessage(string message);
        string FilePath { get; set; }
        bool OpenFileDialog();
        bool SaveFileDialog(FileType fileType);
    }
}
