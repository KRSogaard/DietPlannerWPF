using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietPlanner.ViewModels.Common;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan
{
    public class ShoppingListViewModel : BindableBase
    {
        public ObservableCollection<ShoppingListItemViewModel> Items { get; set; }

        public ShoppingListViewModel(MainViewModel mainViewModel, PlanViewModel planViewModel)
        {
            Items = new ObservableCollection<ShoppingListItemViewModel>();
        }


        private void GenerateShoppingList()
        {
            Items.Clear();
            Dictionary<string, ConsumableViewModel> map = new Dictionary<string, ConsumableViewModel>();
        }

        public class ShoppingListItemViewModel : BindableBase
        {
            public ConsumableViewModel Consumable { get; set; }

            private double _quantity;
            public double Quantity
            {
                get { return _quantity; }
                set { SetProperty(ref _quantity, value); }
            }
        }
    }
}
