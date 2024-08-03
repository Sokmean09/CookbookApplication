using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookbookApplication.Models
{
    public class Instruction : INotifyPropertyChanged
    {
        private string? name;

        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
