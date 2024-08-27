using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CookbookApplication.Models
{
    internal class Recipe : INotifyPropertyChanged
    {
        private string? name;
        private string? type;
        private string? cuisine;
        private string? imagepath;
        private ObservableCollection<Ingredient>? ingredients;
        private ObservableCollection<Instruction>? instructions;
        private string? about_detail;

        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string? Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged();
            }
        }

        public string? Cuisine
        {
            get => cuisine;
            set
            {
                cuisine = value;
                OnPropertyChanged();
            }
        }

        public string? ImagePath
        {
            get => imagepath;
            set
            {
                imagepath = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Ingredient>? Ingredients
        {
            get => ingredients;
            set
            {
                ingredients = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Instruction>? Instructions
        {
            get => instructions;
            set
            {
                instructions = value;
                OnPropertyChanged();
            }
        }

        public string? About_Detail
        {
            get => about_detail;
            set
            {
                about_detail = value;
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
