using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.Models;
using DietPlanner.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private FoodService foodService;

        public ObservableCollection<Food> AllFoods { get; set; }
        public ObservableCollection<Meal> AllMeals { get; set; }

        #region Food
        private EditFoodViewModel editFood;
        public EditFoodViewModel EditFood
        {
            get { return editFood; }
            set
            {
                SetProperty(ref editFood, value);
                ShowNewFood = value != null;
                OnPropertyChanged(nameof(EditFoodTitle));
            }
        }

        private Food _selectedFood;
        public Food SelectedFood
        {
            get { return _selectedFood; }
            set
            {
                SetProperty(ref _selectedFood, value);
                EditFood = new EditFoodViewModel(this, value);
                OnPropertyChanged(nameof(ShowNewFood));
            }
        }

        private bool _showNewFood;
        public bool ShowNewFood
        {
            get { return _showNewFood; }
            set { SetProperty(ref _showNewFood, value); }
        }

        public string EditFoodTitle
        {
            get
            {
                if (EditFood.Updating == null)
                {
                    return "Create new food";
                }
                return "Edit " + EditFood.Name;
            }
        }
        #endregion


        #region Meals
        private EditMealViewModel _editMeal;
        public EditMealViewModel EditMeal
        {
            get { return _editMeal; }
            set
            {
                SetProperty(ref _editMeal, value);
                ShowEditMeal = value != null;
                OnPropertyChanged(nameof(EditMealTitle));
            }
        }

        private bool _showEditMeal;
        public bool ShowEditMeal
        {
            get { return _showEditMeal; }
            set { SetProperty(ref _showEditMeal, value); }
        }

        private Meal _selectedMeal;
        public Meal SelectedMeal
        {
            get { return _selectedMeal; }
            set
            {
                SetProperty(ref _selectedMeal, value);
                if (value != null)
                    EditMeal = new EditMealViewModel(this, value);
                OnPropertyChanged(nameof(ShowEditMeal));
            }
        }

        public string EditMealTitle
        {
            get
            {
                if (EditMeal.Updating == null)
                {
                    return "Create new meal";
                }
                return "Edit " + EditMeal.Name;
            }
        }
        #endregion

        private MealPlanViewModel _mealPlan;
        public MealPlanViewModel MealPlan
        {
            get { return _mealPlan; }
            set { SetProperty(ref _mealPlan, value); }
        }

        public MainViewModel()
        {
            foodService = new FoodService();
            AllFoods = new ObservableCollection<Food>();
            AllFoods.AddRange(foodService.GetAll());
            AllMeals = new ObservableCollection<Meal>();
            AllMeals.AddRange(foodService.GetAllMeals());
            MealPlan = new MealPlanViewModel(this);
        }

        private ICommand _createFood;
        public ICommand CreateFood
        {
            get
            {
                if (_createFood == null)
                {
                    _createFood = new DelegateCommand(() =>
                    {
                        EditFood = new EditFoodViewModel(this, null);
                    });
                }
                return _createFood;
            }
        }

        public void SaveFood(Food newFood)
        {
            // Hide the flyout
            EditFood = null;

            foodService.Save(newFood);
            AllFoods.Clear();
            AllFoods.AddRange(foodService.GetAll());
        }
        private ICommand _createMeal;
        public ICommand CreateMeal
        {
            get
            {
                if (_createMeal == null)
                {
                    _createMeal = new DelegateCommand(() =>
                    {
                        EditMeal = new EditMealViewModel(this, null);
                    });
                }
                return _createMeal;
            }
        }
        
        public void SaveMeal(Meal meal)
        {
            // Hide the flyout
            EditMeal = null;

            foodService.SaveMeal(meal);
            AllMeals.Clear();
            AllMeals.AddRange(foodService.GetAllMeals());
        }
    }
}
