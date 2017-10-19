using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class EditMealViewModel : BindableBase
    {

        #region Meal
        private Meal _meal;
        public Meal Meal
        {
            get { return _meal; }
            set
            {
                SetProperty(ref _meal, value);
                OnPropertyChanged(nameof(Meal_Calories));
                OnPropertyChanged(nameof(Meal_Cholesterol));
                OnPropertyChanged(nameof(Meal_Sodium));
                OnPropertyChanged(nameof(Meal_Protein));
                OnPropertyChanged(nameof(Meal_Fat));
                OnPropertyChanged(nameof(Meal_Carbohydrate));
            }
        }

        public double Meal_Calories
        {
            get { return Meal.Calories; }
        }

        public double Meal_Cholesterol
        {
            get { return Meal.Cholesterol; }
        }

        public double Meal_Sodium
        {
            get { return Meal.Sodium; }
        }

        public double Meal_Protein
        {
            get { return Meal.Protein; }
        }

        public double Meal_Fat
        {
            get { return Meal.Fat.Total; }
        }

        public double Meal_Carbohydrate
        {
            get { return Meal.Carbohydrate.Total; }
        }

        #endregion

        public Meal Updating { get; set; }
        private MainViewModel mainViewModel;

        public EditMealViewModel(MainViewModel mainViewModel, Meal meal)
        {
            this.Meal = new Meal();
            this.Updating = meal;
            this.mainViewModel = mainViewModel;
            FoodItems = new ObservableCollection<EditMealItemViewModel>();

            if (meal != null)
            {
                Name = meal.Name;
                foreach (Meal.MealItem item in meal.MealItems)
                {
                    FoodItems.Add(new EditMealItemViewModel(this)
                    {
                        Food = item.Food,
                        Units = item.Units.ToString()
                    });
                }
            }
        }
        public EditMealViewModel(MainViewModel mainViewModel) :
            this(mainViewModel, null)
        {
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(CanSaveMeal));
            }
        }

        private Food _selectedAddFood;
        public Food SelectedAddFood
        {
            get { return _selectedAddFood; }
            set
            {
                SetProperty(ref _selectedAddFood, value); 
                OnPropertyChanged(nameof(CanAddFood));
            }
        }

        private EditMealItemViewModel _selectedFood;
        public EditMealItemViewModel SelectedFood
        {
            get { return _selectedFood; }
            set
            {
                SetProperty(ref _selectedFood, value);
                OnPropertyChanged(nameof(CanRemoveFood));
            }
        }

        public bool CanAddFood
        {
            get { return SelectedAddFood != null; }
        }

        public bool CanRemoveFood
        {
            get { return SelectedFood != null; }
        }

        public bool CanSaveMeal
        {
            get { return !String.IsNullOrWhiteSpace(Name) && FoodItems.Count > 0 && !FoodItems.Any(x => double.Parse(x.Units) <= 0); }
        }
        private ObservableCollection<EditMealItemViewModel> _foodItems;
        public ObservableCollection<EditMealItemViewModel> FoodItems
        {
            get { return _foodItems; }
            set { SetProperty(ref _foodItems, value); }
        }

        public ObservableCollection<Food> AllFoods
        {
            get
            {
                return new ObservableCollection<Food>();
                //return mainViewModel.AllFoods;
            }
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
                        if (SelectedAddFood == null)
                        {
                            return;
                        }
                        foreach (var fi in FoodItems)
                        {
                            if (fi.Food == SelectedAddFood)
                            {
                                return;
                            }
                        }

                        FoodItems.Add(new EditMealItemViewModel(this)
                        {
                            Food = SelectedAddFood,
                            Units = "1"
                        });

                        SelectedAddFood = null;
                        UpdateMeal();
                        OnPropertyChanged(nameof(CanSaveMeal));
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
                        if (SelectedFood == null)
                        {
                            return;
                        }

                        FoodItems.Remove(SelectedFood);
                        SelectedFood = null;
                        UpdateMeal();
                        OnPropertyChanged(nameof(CanSaveMeal));
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
                        //if (!CanSaveMeal)
                        //    return;

                        //mainViewModel.SaveMeal(Meal);
                    });
                }
                return _saveMeal;
            }
        }

        protected void UpdateMeal()
        {
            Meal meal = new Meal();
            meal.Name = Name;
            meal.MealItems = FoodItems.Select(x => new Meal.MealItem()
            {
                Food = x.Food,
                Units = double.Parse(x.Units)
            }).ToList();
            if (Updating != null)
            {
                meal.Id = Updating.Id;
            }

            this.Meal = meal;
        }

        public class EditMealItemViewModel : BindableBase
        {
            private EditMealViewModel parent;

            public EditMealItemViewModel(EditMealViewModel editMealViewModel)
            {
                parent = editMealViewModel;
                Units = "0";
            }

            public Food Food { get; set; }
            private double _units;
            public string Units
            {
                get { return _units.ToString(); }
                set
                {
                    double dvalue;
                    if (!double.TryParse(value, out dvalue))
                    {
                        Units = _units.ToString();
                        return;
                    }
                    SetProperty(ref _units, dvalue);
                    parent.UpdateMeal();
                    parent.OnPropertyChanged(nameof(EditMealViewModel.CanSaveMeal));
                }
            }
        }
    }
}
