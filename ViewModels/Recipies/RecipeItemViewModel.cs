using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Common
{
    public class RecipeItemViewModel : BindableBase, IConsumableViewModel
    {
        private ConsumableViewModel _consumable;

        public ConsumableViewModel Consumable
        {
            get { return _consumable; }
            set { SetProperty(ref _consumable, value); }
        }

        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                SetProperty(ref _quantity, value);
                _quantityString = value.ToString();
                OnPropertyChanged(nameof(QuantityString));
            }
        }

        private string _quantityString;
        public string QuantityString
        {
            get { return _quantityString; }
            set
            {
                double tryParse;
                if (!double.TryParse(value, out tryParse))
                {
                    QuantityString = QuantityString;
                    return;
                }
                if (Consumable.IsItemized && tryParse % 1 != 0)
                {
                    QuantityString = ((int) tryParse).ToString();
                    return;
                }

                Quantity = tryParse;
                _quantityString = value;
            }
        }

        public string Name => Consumable.Name;

        public string Unit => Consumable.Unit;

        public double Calories => Consumable.Calories * Quantity;

        public double Cholesterol => Consumable.Cholesterol * Quantity;

        public double Sodium => Consumable.Sodium * Quantity;

        public double Protein => Consumable.Protein * Quantity;

        public double FatTotal => Consumable.FatTotal * Quantity;

        public double CarbohydrateTotal => Consumable.CarbohydrateTotal * Quantity;
    }
}
