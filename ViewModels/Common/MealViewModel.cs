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
    public class MealViewModel : BindableBase
    {
        public ObservableCollection<MealItemViewModel> FoodItems { get; set; }

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

        public double Fat
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

        public double Carbohydrate
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

        public MealViewModel()
        {
            FoodItems = new ObservableCollection<MealItemViewModel>();
            FoodItems.CollectionChanged += (sender, args) =>
            {
                CallUpdate();

                // Register watching items
                foreach (var oldItem in args.OldItems)
                {
                    var item = oldItem as MealItemViewModel;
                    item.PropertyChanged -= SubItemPropertyChanged;
                    item.Consumable.PropertyChanged -= SubItemPropertyChanged;
                    item.Consumable.Fat.PropertyChanged -= SubItemPropertyChanged;
                    item.Consumable.Carbohydrates.PropertyChanged -= SubItemPropertyChanged;
                }

                foreach (var newItem in args.NewItems)
                {
                    var item = newItem as MealItemViewModel;
                    item.PropertyChanged += SubItemPropertyChanged;
                    item.Consumable.PropertyChanged += SubItemPropertyChanged;
                    item.Consumable.Fat.PropertyChanged += SubItemPropertyChanged;
                    item.Consumable.Carbohydrates.PropertyChanged += SubItemPropertyChanged;
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
            OnPropertyChanged(nameof(Fat));
            OnPropertyChanged(nameof(Cholesterol));
            OnPropertyChanged(nameof(Sodium));
            OnPropertyChanged(nameof(Carbohydrate));
            OnPropertyChanged(nameof(Protein));
        }
    }
}
