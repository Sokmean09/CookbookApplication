using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookbookApplication.Models
{
    public class Ingredient : INotifyPropertyChanged
    {
        private string? name;

        private string? quantity;

        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string? Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
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
