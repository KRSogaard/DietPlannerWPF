using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DietPlanner.ViewModels.Common;
using DietPlanner.ViewModels.Plan.Rules;
using DietPlanner.ViewModels.Settings;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan
{
    public class PlanDayViewModel : BindableBase, IConsumableViewModel
    {
        private MainViewModel mainViewModel;
        private PlanViewModel planViewModel;
        public ObservableCollection<PlanMealViewModel> Meals { get; set; }
        public ObservableCollection<IRule> Rules { get; set; }

        public PlanDayViewModel(MainViewModel mainViewModel, PlanViewModel planViewModel)
        {
            this.mainViewModel = mainViewModel;
            this.planViewModel = planViewModel;

            Meals = new ObservableCollection<PlanMealViewModel>();
            Meals.CollectionChanged += (sender, args) => {
                if (args.OldItems != null)
                {
                    foreach (PlanMealViewModel viewModel in args.OldItems)
                    {
                        viewModel.PropertyChanged -= DayViewModelUpdated;
                    }
                }

                if (args.NewItems != null)
                {
                    foreach (PlanMealViewModel viewModel in args.NewItems)
                    {
                        viewModel.PropertyChanged += DayViewModelUpdated;
                    }
                }

                planViewModel.SavePlan();
            };

            Rules = new ObservableCollection<IRule>();
            Rules.Add(new FatGoalRule(this, mainViewModel.Settings));
            Rules.Add(new ProteinGoalRule(this, mainViewModel.Settings));
        }

        private void DayViewModelUpdated(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // Keep if any of it's properties update we need to update
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(Calories):
                case nameof(FatTotal):
                case nameof(Cholesterol):
                case nameof(Sodium):
                case nameof(CarbohydrateTotal):
                case nameof(Protein):
                    CallUpdate();
                    return;
                case "Hour":
                case "Min":
                    planViewModel.SavePlan();
                    return;
            }
        }

        private void CallUpdate()
        {
            OnPropertyChanged(nameof(Calories));
            OnPropertyChanged(nameof(FatTotal));
            OnPropertyChanged(nameof(Cholesterol));
            OnPropertyChanged(nameof(Sodium));
            OnPropertyChanged(nameof(CarbohydrateTotal));
            OnPropertyChanged(nameof(Protein));
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public string Unit
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double Calories => Meals.Sum(x => x.Calories);
        public double FatTotal => Meals.Sum(x => x.FatTotal);
        public double Cholesterol => Meals.Sum(x => x.Cholesterol);
        public double Sodium => Meals.Sum(x => x.Sodium);
        public double CarbohydrateTotal => Meals.Sum(x => x.CarbohydrateTotal);
        public double Protein => Meals.Sum(x => x.Protein);

        #region New
        private string _newMealName;
        public string NewMealName
        {
            get { return _newMealName; }
            set { SetProperty(ref _newMealName, value);
                OnPropertyChanged(nameof(CanAddNewMeal));
            }
        }

        private int _newMealHours;
        public int NewMealHours
        {
            get { return _newMealHours; }
            set { SetProperty(ref _newMealHours, value);
                OnPropertyChanged(nameof(CanAddNewMeal));
            }
        }

        private int _newMealMin;
        public int NewMealMin
        {
            get { return _newMealMin; }
            set
            {
                SetProperty(ref _newMealMin, value);
                OnPropertyChanged(nameof(CanAddNewMeal));
            }
        }

        public bool CanAddNewMeal
        {
            get
            {
                return !String.IsNullOrWhiteSpace(NewMealName) &&
                       NewMealHours >= 00 && NewMealHours <= 23 &&
                       NewMealMin >= 00 && NewMealMin <= 59;
            }
        }

        private ICommand _addNewMeal;
        public ICommand AddNewMeal
        {
            get
            {
                if (_addNewMeal == null)
                {
                    _addNewMeal = new DelegateCommand(() =>
                    {
                        if (!CanAddNewMeal)
                            return;

                        Meals.Add(new PlanMealViewModel(mainViewModel, planViewModel, this)
                        {
                            Name = NewMealName,
                            Hour = NewMealHours,
                            Min = NewMealMin
                        });

                        NewMealName = "";
                        NewMealHours = 0;
                        NewMealMin = 0;
                    });
                }
                return _addNewMeal;
            }
        }
        #endregion

        public void RemoveMeal(PlanMealViewModel planMealViewModel)
        {
            Meals.Remove(planMealViewModel);
        }
    }
}