using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan
{
    public class PlanMealViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private int _hour;
        public int Hour
        {
            get { return _hour; }
            set { SetProperty(ref _hour, value); }
        }

        private int _min;
        public int Min
        {
            get { return _min; }
            set { SetProperty(ref _min, value); }
        }

        public ObservableCollection<PlanConsumableViewModel> Consumables { get; set; }

        public PlanMealViewModel(MainViewModel mainViewModel, PlanViewModel planViewModel)
        {
            Consumables = new ObservableCollection<PlanConsumableViewModel>();
            Consumables.CollectionChanged += (sender, args) =>
            {
                planViewModel.SavePlan();
            };
        }
    }
}
