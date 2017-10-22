using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.ViewModels.Common;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan
{
    public class PlanMealViewModel : BindableBase, IConsumableViewModel
    {
        private MainViewModel mainViewModel;

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
            this.mainViewModel = mainViewModel;

            Consumables = new ObservableCollection<PlanConsumableViewModel>();
            Consumables.CollectionChanged += (sender, args) =>
            {
                if (args.OldItems != null)
                {
                    foreach (PlanConsumableViewModel viewModel in args.OldItems)
                    {
                        viewModel.PropertyChanged -= ConsumableViewModelUpdated;
                    }
                }

                if (args.NewItems != null)
                {
                    foreach (PlanConsumableViewModel viewModel in args.NewItems)
                    {
                        viewModel.PropertyChanged += ConsumableViewModelUpdated;
                    }
                }

                CallUpdate();
            };
        }

        private void ConsumableViewModelUpdated(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                CallUpdate();
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
            OnPropertyChanged(nameof(CanAddRecipe));
            OnPropertyChanged(nameof(CanAddConsumable));
        }
        
        public string Unit
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public double Calories => Consumables.Sum(x => x.Calories);
        public double FatTotal => Consumables.Sum(x => x.FatTotal);
        public double Cholesterol => Consumables.Sum(x => x.Cholesterol);
        public double Sodium => Consumables.Sum(x => x.Sodium);
        public double CarbohydrateTotal => Consumables.Sum(x => x.CarbohydrateTotal);
        public double Protein => Consumables.Sum(x => x.Protein);


        public ObservableCollection<ConsumableViewModel> AllConsumables
        {
            get { return mainViewModel.Consumables.Consumables; }
        }

        private ConsumableViewModel _selectedConsumable;
        public ConsumableViewModel SelectedConsumable
        {
            get { return _selectedConsumable; }
            set
            {
                SetProperty(ref _selectedConsumable, value);
                OnPropertyChanged(nameof(CanAddConsumable));
            }
        }
        
        public bool CanAddConsumable
        {
            get { return SelectedConsumable != null && !Consumables.Any(x =>
                    !x.IsRecipe &&
                    ((ConsumableViewModel)x.Consumable).Id == SelectedConsumable.Id); ; }
        }

        private ICommand _addConsumable;
        public ICommand AddConsumable
        {
            get
            {
                if (_addConsumable == null)
                {
                    _addConsumable = new DelegateCommand(() =>
                    {
                        if (!CanAddConsumable)
                            return;
                        Consumables.Add(new PlanConsumableViewModel(SelectedConsumable)
                        {
                            Quantity = 1
                        });
                        OnPropertyChanged(nameof(CanAddConsumable));
                    });
                }
                return _addConsumable;
            }
        }

        public ObservableCollection<RecipeViewModel> Recipes
        {
            get { return mainViewModel.Recipies.Recipes; }
        }

        private RecipeViewModel _selectedRecipe;
        public RecipeViewModel SelectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                SetProperty(ref _selectedRecipe, value);
                OnPropertyChanged(nameof(CanAddRecipe));
            }
        }
        
        public bool CanAddRecipe
        {
            get
            {
                return SelectedRecipe != null && 
                    !Consumables.Any(x =>
                                   x.IsRecipe &&
                                   ((RecipeViewModel)x.Consumable).Id == SelectedRecipe.Id);
            }
        }

        private ICommand _addRecipe;
        public ICommand AddRecipe
        {
            get
            {
                if (_addRecipe == null)
                {
                    _addRecipe = new DelegateCommand(() =>
                    {
                        if (!CanAddRecipe) 
                            return;
                        Consumables.Add(new PlanConsumableViewModel(SelectedRecipe)
                        {
                            Quantity = 1
                        });
                        OnPropertyChanged(nameof(CanAddRecipe));
                    });
                }
                return _addRecipe;
            }
        }
    }
}
