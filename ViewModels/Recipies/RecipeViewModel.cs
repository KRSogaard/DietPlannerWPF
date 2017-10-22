using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Common
{
    public class RecipeViewModel : BindableBase, IConsumableViewModel
    {
        public ObservableCollection<RecipeItemViewModel> FoodItems { get; private set; }

        public int Count
        {
            get { return FoodItems.Count; }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Unit
        {
            get { return "meals"; }
        }

        public double Calories
        {
            get
            {
                double total = 0;
                foreach (var item in FoodItems)
                {
                    total += item.Quantity * item.Consumable.Calories;
                }
                return total;
            }
        }

        public double FatTotal
        {
            get
            {
                double total = 0;
                foreach (var item in FoodItems)
                {
                    total += item.Quantity * item.Consumable.Fat.Total;
                }
                return total;
            }
        }

        public double Cholesterol
        {
            get
            {
                double total = 0;
                foreach (var item in FoodItems)
                {
                    total += item.Quantity * item.Consumable.Cholesterol;
                }
                return total;
            }
        }

        public double Sodium
        {
            get
            {
                double total = 0;
                foreach (var item in FoodItems)
                {
                    total += item.Quantity * item.Consumable.Sodium;
                }
                return total;
            }
        }

        public double CarbohydrateTotal
        {
            get
            {
                double total = 0;
                foreach (var item in FoodItems)
                {
                    total += item.Quantity * item.Consumable.Carbohydrates.Total;
                }
                return total;
            }
        }

        public double Protein
        {
            get
            {
                double total = 0;
                foreach (var item in FoodItems)
                {
                    total += item.Quantity * item.Consumable.Protein;
                }
                return total;
            }
        }

        public RecipeViewModel()
        {
            FoodItems = new ObservableCollection<RecipeItemViewModel>();
            FoodItems.CollectionChanged += (sender, args) =>
            {
                CallUpdate();

                // Register watching items
                if (args.OldItems != null)
                {
                    foreach (var oldItem in args.OldItems)
                    {
                        var item = oldItem as RecipeItemViewModel;
                        item.PropertyChanged -= SubItemPropertyChanged;
                        item.Consumable.PropertyChanged -= SubItemPropertyChanged;
                        item.Consumable.Fat.PropertyChanged -= SubItemPropertyChanged;
                        item.Consumable.Carbohydrates.PropertyChanged -= SubItemPropertyChanged;
                    }
                }

                if (args.NewItems != null)
                {
                    foreach (var newItem in args.NewItems)
                    {
                        var item = newItem as RecipeItemViewModel;
                        item.PropertyChanged += SubItemPropertyChanged;
                        item.Consumable.PropertyChanged += SubItemPropertyChanged;
                        item.Consumable.Fat.PropertyChanged += SubItemPropertyChanged;
                        item.Consumable.Carbohydrates.PropertyChanged += SubItemPropertyChanged;
                    }
                }
            };
        }

        private void SubItemPropertyChanged(object o, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            CallUpdate();
        }

        private void CallUpdate()
        {
            OnPropertyChanged(nameof(Calories));
            OnPropertyChanged(nameof(FatTotal));
            OnPropertyChanged(nameof(Cholesterol));
            OnPropertyChanged(nameof(Sodium));
            OnPropertyChanged(nameof(CarbohydrateTotal));
            OnPropertyChanged(nameof(Protein));
            OnPropertyChanged(nameof(Count));
        }

        public void Clone(RecipeViewModel mealViewModel)
        {
            Id = mealViewModel.Id;
            Name = mealViewModel.Name;

            FoodItems.Clear();
            foreach (RecipeItemViewModel x in mealViewModel.FoodItems)
            {
                FoodItems.Add(new RecipeItemViewModel()
                {
                    Consumable = x.Consumable,
                    Quantity = x.Quantity
                });
            }
        }

    }
}
