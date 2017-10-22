using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DietPlanner.ViewModels.Common;
using Prism.Commands;

namespace DietPlanner.ViewModels.Recipies
{
    public class EditRecipeViewModel : RecipeViewModel
    {
        private MainViewModel mainViewModel;
        private Action<EditRecipeViewModel> saveMealUpdate;
        
        public EditRecipeViewModel(MainViewModel mainViewModel, RecipeViewModel editing, Action<RecipeViewModel> saveMealUpdate)
        {
            this.mainViewModel = mainViewModel;
            this.saveMealUpdate = saveMealUpdate;

            if (editing != null)
            {
                this.Clone(editing);
            }

            this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Name")
                {
                    OnPropertyChanged("CanSave");
                }
            };
        }

        public ObservableCollection<ConsumableViewModel> Consumables
        {
            get { return mainViewModel.Consumables.Consumables; }
        }

        private ConsumableViewModel _selectedAddFood;
        public ConsumableViewModel SelectedAddFood
        {
            get { return _selectedAddFood; }
            set {
                SetProperty(ref _selectedAddFood, value);
                OnPropertyChanged(nameof(CanAddFood));
            }
        }

        public bool CanAddFood
        {
            get {
                return SelectedAddFood != null && !
                    FoodItems.Any(x => x.Consumable.Id == SelectedAddFood.Id);
            }
        }
        
        private RecipeItemViewModel _selectedFood;
        public RecipeItemViewModel SelectedFood
        {
            get { return _selectedFood; }
            set
            {
                SetProperty(ref _selectedFood, value);
                OnPropertyChanged(nameof(CanRemoveFood));
            }
        }

        public bool CanRemoveFood
        {
            get { return SelectedFood != null; }
        }

        private ICommand _addFood;
        public ICommand AddFood
        {
            get
            {
                if (_addFood == null)
                {
                    _addFood = new DelegateCommand(() =>
                    {
                        // This food type has already been added
                        if (this.FoodItems.Any(x => x.Consumable.Id == SelectedAddFood.Id))
                            return;

                        this.FoodItems.Add(new RecipeItemViewModel()
                        {
                            Consumable = SelectedAddFood,
                            Quantity = 1
                        });

                        OnPropertyChanged(nameof(CanAddFood));
                        OnPropertyChanged(nameof(CanSave));
                    });
                }
                return _addFood;
            }
        }

        private ICommand _removeFood;
        public ICommand RemoveFood
        {
            get
            {
                if (_removeFood == null)
                {
                    _removeFood = new DelegateCommand(() =>
                    {
                        // This food type has already been added
                        var food = this.FoodItems.FirstOrDefault(x => x.Consumable.Id == SelectedAddFood.Id);
                        if (food == null)
                            return;

                        FoodItems.Remove(food);

                        OnPropertyChanged(nameof(CanAddFood));
                        OnPropertyChanged(nameof(CanSave));
                    });
                }
                return _removeFood;
            }
        }

        private ICommand _saveMeal;
        public ICommand SaveMeal
        {
            get
            {
                if (_saveMeal == null)
                {
                    _saveMeal = new DelegateCommand(() =>
                    {
                        this.saveMealUpdate(this);
                    });
                }
                return _saveMeal;
            }
        }

        public bool CanSave
        {
            get {
                return !String.IsNullOrWhiteSpace(Name) &&
                        FoodItems.Any(x => x.Quantity > 0);
            }
        }
    }
}
