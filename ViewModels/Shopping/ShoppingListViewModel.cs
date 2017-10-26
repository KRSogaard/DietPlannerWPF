using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DietPlanner.Printing;
using DietPlanner.ViewModels.Common;
using DietPlanner.ViewModels.Plan;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Shopping
{
    public class ShoppingListViewModel : BindableBase
    {
        private MainViewModel mainViewModel;
        private PlanViewModel planViewModel;
        private Dictionary<string, ShoppingListItemViewModel> consumableToShoppingListMap = new Dictionary<string, ShoppingListItemViewModel>();
        
        public ObservableCollection<ShoppingListItemViewModel> Items { get; set; }

        private bool _includeMonday = true;
        public bool IncludeMonday
        {
            get { return _includeMonday; }
            set
            {
                SetProperty(ref _includeMonday, value);
                GenerateShoppingList();
            }
        }

        private bool _includeTuesday = true;
        public bool IncludeTuesday
        {
            get { return _includeTuesday; }
            set
            {
                SetProperty(ref _includeTuesday, value);
                GenerateShoppingList();
            }
        }

        private bool _includeWednesday = true;
        public bool IncludeWednesday
        {
            get { return _includeWednesday; }
            set
            {
                SetProperty(ref _includeWednesday, value);
                GenerateShoppingList();
            }
        }

        private bool _includeThursday = true;
        public bool IncludeThursday
        {
            get { return _includeThursday; }
            set
            {
                SetProperty(ref _includeThursday, value);
                GenerateShoppingList();
            }
        }

        private bool _includeFriday = true;
        public bool IncludeFriday
        {
            get { return _includeFriday; }
            set
            {
                SetProperty(ref _includeFriday, value);
                GenerateShoppingList();
            }
        }

        private bool _includeSaturday = true;
        public bool IncludeSaturday
        {
            get { return _includeSaturday; }
            set
            {
                SetProperty(ref _includeSaturday, value);
                GenerateShoppingList();
            }
        }

        private bool _includeSunday = true;
        public bool IncludeSunday
        {
            get { return _includeSunday; }
            set
            {
                SetProperty(ref _includeSunday, value);
                GenerateShoppingList();
            }
        }

        public ShoppingListViewModel(MainViewModel mainViewModel, PlanViewModel planViewModel)
        {
            this.planViewModel = planViewModel;
            this.mainViewModel = mainViewModel;

            Items = new ObservableCollection<ShoppingListItemViewModel>();
            GenerateShoppingList();
        }
        
        private void GenerateShoppingList()
        {
            Items.Clear();
            consumableToShoppingListMap.Clear();
            if (IncludeMonday)
                GenerateDay(planViewModel.Monday);
            if (IncludeTuesday)
                GenerateDay(planViewModel.Tuesday);
            if (IncludeWednesday)
                GenerateDay(planViewModel.Wednesday);
            if (IncludeThursday)
                GenerateDay(planViewModel.Thursday);
            if (IncludeFriday)
                GenerateDay(planViewModel.Friday);
            if (IncludeSaturday)
                GenerateDay(planViewModel.Saturday);
            if (IncludeSunday)
                GenerateDay(planViewModel.Sunday);
        }

        private void GenerateDay(PlanDayViewModel day)
        {
            if (day == null || day.Meals == null) 
                return;
            
            foreach (var meal in day.Meals)
            {
                if (meal.Consumables == null)
                    continue;

                foreach (PlanConsumableViewModel consumable in meal.Consumables)
                {
                    if (consumable.IsRecipe)
                    {
                        foreach (RecipeItemViewModel foodItem in ((RecipeViewModel) consumable.Consumable).FoodItems)
                        {
                            AddItem(foodItem.Consumable, foodItem.Quantity);
                        }
                    }
                    else
                    {
                        AddItem((ConsumableViewModel)consumable.Consumable, consumable.Quantity);
                    }
                }
            }
        }

        private void AddItem(ConsumableViewModel consumable, double quantity)
        {
            if (!consumableToShoppingListMap.ContainsKey(consumable.Id))
            {
                var newItem = new ShoppingListItemViewModel(consumable);
                consumableToShoppingListMap.Add(consumable.Id, newItem);
                Items.Add(newItem);
            }
            consumableToShoppingListMap[consumable.Id].Quantity += quantity;
        }

        private ICommand _generateList;
        public ICommand GenerateList
        {
            get
            {
                if (_generateList == null)
                {
                    _generateList = new DelegateCommand(() =>
                    {
                        GenerateShoppingList();
                    });
                }
                return _generateList;
            }
        }

        private ICommand _print;
        public ICommand Print
        {
            get
            {
                if (_print == null)
                {
                    _print = new DelegateCommand(() =>
                    {
                        new ShoppingListPrinting(Items.ToList()).Print();
                    });
                }
                return _print;
            }
        }

        public class ShoppingListItemViewModel : BindableBase
        {
            public ConsumableViewModel Consumable { get; private set; }
            
            public ShoppingListItemViewModel(ConsumableViewModel consumable)
            {
                Consumable = consumable;
                Quantity = 0;
            }

            private double _quantity;
            public double Quantity
            {
                get { return _quantity; }
                set
                {
                    SetProperty(ref _quantity, value);
                    OnPropertyChanged(nameof(Note));
                }
            }

            public string Note
            {
                get
                {
                    if ("g".Equals(Consumable.Unit, StringComparison.CurrentCultureIgnoreCase))
                    {
                        double lb = Quantity * 0.0022;
                        double oz = Quantity * 0.03527396;
                        return $" {lb.ToString("##.##")} lb, {oz.ToString("##.##")} oz";
                    }

                    return "";
                }
            }
        }
    }
}
