using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietPlanner.Models;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class MealPlanViewModel : BindableBase
    {
        public MainViewModel mainViewModel;

        public MealPlanDayViewModel Monday { get; set; }
        public MealPlanDayViewModel Tuesday { get; set; }
        public MealPlanDayViewModel Wednesday { get; set; }
        public MealPlanDayViewModel Thursday { get; set; }
        public MealPlanDayViewModel Friday { get; set; }
        public MealPlanDayViewModel Saturday { get; set; }
        public MealPlanDayViewModel Sunday { get; set; }

        public MealPlanViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;

            Monday = new MealPlanDayViewModel(this);
            Tuesday = new MealPlanDayViewModel(this);
            Wednesday = new MealPlanDayViewModel(this);
            Thursday = new MealPlanDayViewModel(this);
            Friday = new MealPlanDayViewModel(this);
            Saturday = new MealPlanDayViewModel(this);
            Sunday = new MealPlanDayViewModel(this);
        }

        public class MealPlanDayViewModel : BindableBase
        {
            private MealPlanViewModel _parent;

            private ObservableCollection<MealPlanDayMealViewModel> _meals;
            public ObservableCollection<MealPlanDayMealViewModel> Meals {
                get { return _meals; }
                set
                {
                    SetProperty(ref _meals, value);
                    OnPropertyChanged(nameof(MealCount));
                }
            }

            public int MealCount
            {
                get { return Meals.Count; }
            }

            public MealPlanDayViewModel(MealPlanViewModel parent)
            {
                _parent = parent;
                Meals = new ObservableCollection<MealPlanDayMealViewModel>();
                Meals.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(MealCount));

                Meals.Add(new MealPlanDayMealViewModel(this, "Meal 1"));
                Meals.Add(new MealPlanDayMealViewModel(this, "Meal 2"));
                Meals.Add(new MealPlanDayMealViewModel(this, "Meal 3"));
                Meals.Add(new MealPlanDayMealViewModel(this, "Meal 4"));
            }

            public class MealPlanDayMealViewModel : BindableBase
            {
                private string _name;
                public string Name
                {
                    get { return _name; }
                    set { SetProperty(ref _name, value); }
                }

                private ObservableCollection<MealItem> _items;
                public ObservableCollection<MealItem> Items
                {
                    get { return _items; }
                    set { SetProperty(ref _items, value); }
                }

                public MealPlanDayMealViewModel(MealPlanDayViewModel mealPlanDayViewModel, string meal)
                {
                    Name = meal;
                    Items = new ObservableCollection<MealItem>();

                    //Items.Add(new MealItem(mealPlanDayViewModel._parent.mainViewModel.AllFoods.First())
                    //{
                    //    Units = "150"
                    //});
                    //Items.Add(new MealItem(mealPlanDayViewModel._parent.mainViewModel.AllFoods.Last())
                    //{
                    //    Units = "1"
                    //});
                }

                public class MealItem : BindableBase
                {
                    public MealItem(IConsumable food)
                    {
                        _food = food;
                        Units = "1";
                    }

                    private IConsumable _food;
                    public IConsumable Food
                    {
                        get { return _food; }
                        
                    }

                    private double _units;
                    public string Units
                    {
                        get { return _units.ToString("##.###"); }
                        set
                        {
                            double tryParse;
                            if (!double.TryParse(value, out tryParse))
                            {
                                Units = _units.ToString();
                                return;
                            }
                            // If it is a meal or itemized can we only add whole numbers
                            if (_food is Meal || (_food is Food && ((Food) _food).IsItemized))
                            {
                                if (tryParse % 1 != 0)
                                {
                                    Units = ((int) tryParse).ToString();
                                    return;
                                }
                            }

                            _units = tryParse;

                            SetProperty(ref _units, tryParse);
                            OnPropertyChanged(nameof(Calories));
                            OnPropertyChanged(nameof(Fat));
                            OnPropertyChanged(nameof(Cholesterol));
                            OnPropertyChanged(nameof(Sodium));
                            OnPropertyChanged(nameof(Carbohydrate));
                            OnPropertyChanged(nameof(Protein));
                        }
                    }

                    public double Calories
                    {
                        get { return _food.Calories * _units; }
                    }

                    public double Fat
                    {
                        get { return _food.Fat.Total * _units; }
                    }

                    public double Cholesterol
                    {
                        get { return _food.Cholesterol * _units; }
                    }

                    public double Sodium
                    {
                        get { return _food.Sodium * _units; }
                    }

                    public double Carbohydrate
                    {
                        get { return _food.Carbohydrate.Total * _units; }
                    }

                    public double Protein
                    {
                        get { return _food.Protein * _units; }
                    }
                }
            }
        }
    }
}
