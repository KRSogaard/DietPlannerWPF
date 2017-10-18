using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Common
{
    public class MealItemViewModel : BindableBase
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
            }
        }
        public string QuantityString
        {
            get { return Quantity.ToString(); }
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
            }
        }
    }
}
