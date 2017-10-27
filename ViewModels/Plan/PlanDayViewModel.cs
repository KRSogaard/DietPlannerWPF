using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DietPlanner.Printing;
using DietPlanner.ViewModels.Common;
using DietPlanner.ViewModels.Plan.Rules;
using DietPlanner.ViewModels.Settings;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan
{
    public class PlanDayViewModel : BindableBase, IConsumableViewModel
    {
        public MainViewModel mainViewModel;
        private PlanViewModel planViewModel;
        public ObservableCollection<PlanMealViewModel> Meals { get; set; }
        public ObservableCollection<IRule> Rules { get; set; }

        public string DayName { get; set; }

        public PlanDayViewModel(string dayName, MainViewModel mainViewModel, PlanViewModel planViewModel)
        {
            this.DayName = dayName;
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

            OtherDays = new ObservableCollection<PlanDayViewModel>();
            foreach (PlanDayViewModel planDayViewModel in planViewModel.Days)
            {
                if (planDayViewModel != this)
                {
                    OtherDays.Add(planDayViewModel);
                }
            }
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

            OnPropertyChanged(nameof(FatProcent));
            OnPropertyChanged(nameof(ProteinProcent));
            OnPropertyChanged(nameof(CarbohydrateProcent));
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

        public double FatProcent
        {
            get
            {
                var total = FatTotal * 9 + Protein * 4 + CarbohydrateTotal * 4;
                var cals = FatTotal * 9;
                return (cals / total) * 100;
            }
        }
        public double ProteinProcent
        {
            get
            {
                var total = FatTotal * 9 + Protein * 4 + CarbohydrateTotal * 4;
                var cals = Protein * 4;
                return (cals / total) * 100;
            }
        }
        public double CarbohydrateProcent
        {
            get
            {
                var total = FatTotal * 9 + Protein * 4 + CarbohydrateTotal * 4;
                var cals = CarbohydrateTotal * 4;
                return (cals / total) * 100;
            }
        }

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
        
        private ICommand _print;
        public ICommand Print
        {
            get
            {
                if (_print == null)
                {
                    _print = new DelegateCommand(() =>
                    {
                        new PlanDayPrinting(mainViewModel, this).Print();
                    });
                }
                return _print;
            }
        }

        #region Clone
        public ObservableCollection<PlanDayViewModel> OtherDays { get; set; }

        private PlanDayViewModel _selectedCloneDay;
        public PlanDayViewModel SelectedCloneDay
        {
            get { return _selectedCloneDay; }
            set
            {
                SetProperty(ref _selectedCloneDay, value);
                OnPropertyChanged(nameof(CanClone));
            }
        }

        public bool CanClone
        {
            get { return SelectedCloneDay != null; }
        }

        private ICommand _clone;
        public ICommand Clone
        {
            get
            {
                if (_clone == null)
                {
                    _clone = new DelegateCommand(() =>
                    {
                        Meals.Clear();
                        foreach (PlanMealViewModel pMeal in SelectedCloneDay.Meals)
                        {
                            var newMeal = new PlanMealViewModel(mainViewModel, planViewModel, this)
                            {
                                Name = pMeal.Name,
                                Hour = pMeal.Hour,
                                Min = pMeal.Min
                            };
                            foreach (var consumable in pMeal.Consumables)
                            {
                                newMeal.Consumables.Add(new PlanConsumableViewModel(consumable.Consumable)
                                {
                                    Quantity = consumable.Quantity
                                });
                            }
                            Meals.Add(newMeal);
                        }
                        CallUpdate();
                    });
                }
                return _clone;
            }
        }

        #endregion
    }
}