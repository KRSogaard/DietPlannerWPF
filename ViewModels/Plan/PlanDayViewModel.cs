using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan
{
    public class PlanDayViewModel : BindableBase
    {
        private MainViewModel mainViewModel;
        private PlanViewModel planViewModel;
        public ObservableCollection<PlanMealViewModel> Meals { get; set; }

        public PlanDayViewModel(MainViewModel mainViewModel, PlanViewModel planViewModel)
        {
            this.mainViewModel = mainViewModel;
            this.planViewModel = planViewModel;

            Meals = new ObservableCollection<PlanMealViewModel>();
            Meals.CollectionChanged += (sender, args) => {
                OnPropertyChanged("MealCount");
            };
        }
    }
}