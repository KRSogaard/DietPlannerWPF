using DietPlanner.ViewModels.Common;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan
{
    public class PlanConsumableViewModel : BindableBase, IConsumableViewModel
    {
        public IConsumableViewModel Consumable { get; set; }

        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                SetProperty(ref _quantity, value);
                _quantityString = value.ToString();
                OnPropertyChanged(nameof(QuantityString));
                CallUpdateOnAll();
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

                bool isItemized = (Consumable is RecipeItemViewModel || 
                                   (Consumable is ConsumableViewModel && ((ConsumableViewModel)Consumable).IsItemized));
                if (isItemized && tryParse % 1 != 0)
                {
                    QuantityString = ((int)tryParse).ToString();
                    return;
                }

                Quantity = tryParse;
                _quantityString = value;
            }
        }

        public PlanConsumableViewModel(IConsumableViewModel consumable)
        {
            Consumable = consumable;
            Quantity = 1;

            consumable.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "Calories":
                        OnPropertyChanged("Calories");
                        return;
                    case "Cholesterol":
                        OnPropertyChanged("Cholesterol");
                        return;
                    case "Sodium":
                        OnPropertyChanged("Sodium");
                        return;
                    case "Protein":
                        OnPropertyChanged("Protein");
                        return;
                    case "FatTotal":
                        OnPropertyChanged("FatTotal");
                        return;
                    case "CarbohydrateTotal":
                        OnPropertyChanged("CarbohydrateTotal");
                        return;
                }
            };
        }

        private void CallUpdateOnAll()
        {
            OnPropertyChanged(nameof(Calories));
            OnPropertyChanged(nameof(Cholesterol));
            OnPropertyChanged(nameof(Sodium));
            OnPropertyChanged(nameof(Protein));
            OnPropertyChanged(nameof(FatTotal));
            OnPropertyChanged(nameof(CarbohydrateTotal));
        }

        public string Name
        {
            get { return Consumable.Name; }
        }
        public string Unit
        {
            get { return Consumable.Unit; }
        }
        public double Calories
        {
            get { return Consumable.Calories * Quantity; }
        }
        public double Cholesterol
        {
            get { return Consumable.Cholesterol * Quantity; }
        }
        public double Sodium
        {
            get { return Consumable.Sodium * Quantity; }
        }
        public double Protein
        {
            get { return Consumable.Protein * Quantity; }
        }
        public double FatTotal
        {
            get { return Consumable.FatTotal * Quantity; }
        }
        public double CarbohydrateTotal
        {
            get { return Consumable.CarbohydrateTotal * Quantity; }
        }

        public bool IsRecipe
        {
            get { return Consumable is RecipeViewModel; }
        }
    }
}