using DietPlanner.ViewModels.Food;
using DietPlanner.ViewModels.Plan;
using DietPlanner.ViewModels.Recipies;
using DietPlanner.ViewModels.Settings;
using DietPlanner.ViewModels.Shopping;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public SettingsViewModel Settings { get; set; }
        public ConsumablesViewModel Consumables { get; set; }
        public RecipiesViewModel Recipies { get; set; }
        public PlanViewModel Plan { get; set; }
        public ShoppingListViewModel Shopping { get; set; }

        public MainViewModel()
        {
            Settings = new SettingsViewModel(this);
            Consumables = new ConsumablesViewModel(this);
            Recipies = new RecipiesViewModel(this);
            Plan = new PlanViewModel(this);
            Shopping = new ShoppingListViewModel(this, Plan);
        }
    }
}
