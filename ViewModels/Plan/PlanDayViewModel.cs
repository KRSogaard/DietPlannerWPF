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


            OnPropertyChanged(nameof(ProteinGoal));
            OnPropertyChanged(nameof(ProteinGoalText));
            OnPropertyChanged(nameof(FatGoal));
            OnPropertyChanged(nameof(FatGoalText));
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

        public bool ProteinGoal
        {
            get { return Protein / Settings.Weight < Settings.ProteinGoal; }
        }

        public string ProteinGoalText
        {
            get
            {
                var required = (Settings.ProteinGoal * Settings.Weight);
                return "Protein gotal of " + required.ToString("F1") + " not meet, " + (required - Protein).ToString("F1") + " missing.";
            }
        }

        public bool FatGoal
        {
            get { return FatTotal / Settings.Weight < Settings.FatGoal; }
        }

        public string FatGoalText
        {
            get
            {
                var required = (Settings.FatGoal * Settings.Weight);
                return "Fat gotal of " + required.ToString("F1") + " not meet, " + (required - FatTotal).ToString("F1") + " missing.";
            }
        }


        public SettingsViewModel Settings
        {
            get { return mainViewModel.Settings; }
        }
    }
}