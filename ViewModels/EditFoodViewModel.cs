using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.Models;
using DietPlanner.WPF;
using Prism.Commands;
using Prism.Common;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class EditFoodViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private FoodType _type = FoodType.RedMeat;
        public FoodType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }
        
        private List<string> _allFoodsType;
        public List<string> AllFoodsType
        {
            get { return _allFoodsType; }
            set { SetProperty(ref _allFoodsType, value); }
        }

        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set { SetProperty(ref _unit, value); }
        }

        private bool _IsItemized;
        public bool IsItemized
        {
            get { return _IsItemized; }
            set { SetProperty(ref _IsItemized, value); }
        }

        private double _calories;
        public string Calories
        {
            get { return _calories.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    Calories = _calories.ToString();
                    return;
                }
                SetProperty(ref _calories, dvalue);
            }
        }

        private double _cholesterol;
        public string Cholesterol
        {
            get { return _cholesterol.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _cholesterol.ToString();
                    return;
                }
                SetProperty(ref _cholesterol, dvalue);
            }
        }

        private double _sodium;
        public string Sodium
        {
            get { return _sodium.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _sodium.ToString();
                    return;
                }
                SetProperty(ref _sodium, dvalue);
            }
        }

        private double _protein;
        public string Protein
        {
            get { return _protein.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _protein.ToString();
                    return;
                }
                SetProperty(ref _protein, dvalue);
            }
        }

        #region Carbohydrates
        public double _carbohydratesTotal;
        public string CarbohydratesTotal
        {
            get { return _carbohydratesTotal.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _carbohydratesTotal.ToString();
                    return;
                }
                SetProperty(ref _carbohydratesTotal, dvalue);
            }
        }

        public double _carbohydratesDietaryFiber;
        public string CarbohydratesDietaryFiber
        {
            get { return _carbohydratesDietaryFiber.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _carbohydratesDietaryFiber.ToString();
                    return;
                }
                SetProperty(ref _carbohydratesDietaryFiber, dvalue);
            }
        }

        public double _carbohydratesSugar;
        public string CarbohydratesSugar
        {
            get { return _carbohydratesSugar.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _carbohydratesSugar.ToString();
                    return;
                }
                SetProperty(ref _carbohydratesSugar, dvalue);
            }
        }
        #endregion

        #region Fat
        private double _fatTotal;
        public string FatTotal
        {
            get { return _fatTotal.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _fatTotal.ToString();
                    return;
                }
                SetProperty(ref _fatTotal, dvalue);
            }
        }

        private double _fatSaturated;
        public string FatSaturated
        {
            get { return _fatSaturated.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _fatSaturated.ToString();
                    return;
                }
                SetProperty(ref _fatSaturated, dvalue);
            }
        }

        private double _fatPolyunsaturated;
        public string FatPolyunsaturated
        {
            get { return _fatPolyunsaturated.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _fatPolyunsaturated.ToString();
                    return;
                }
                SetProperty(ref _fatPolyunsaturated, dvalue);
            }
        }

        private double _fatMonounsaturated;
        public string FatMonounsaturated
        {
            get { return _fatMonounsaturated.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _fatMonounsaturated.ToString();
                    return;
                }
                SetProperty(ref _fatMonounsaturated, dvalue);
            }
        }

        private double _fatTrans;
        public string FatTrans
        {
            get { return _fatTrans.ToString(); }
            set
            {
                double dvalue;
                if (!double.TryParse(value, out dvalue))
                {
                    FatTotal = _fatTrans.ToString();
                    return;
                }
                SetProperty(ref _fatTrans, dvalue);
            }
        }
        #endregion

        private bool _canSave;
        public bool CanSave
        {
            get
            {
                return !String.IsNullOrWhiteSpace(Name) &&
                       !String.IsNullOrWhiteSpace(Unit);
            }
        }

        public bool IsEditing
        {
            get { return Updating != null; }
        }

        public string EditingName
        {
            get { return Updating?.Name; }
        }

        public Food Updating;
        private MainViewModel mainViewModel;

        public EditFoodViewModel(MainViewModel mainViewModel, Food updating)
        {
            this.mainViewModel = mainViewModel;

            this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != nameof(CanSave))
                {
                    OnPropertyChanged(nameof(CanSave));
                }
            };

            if (updating != null)
            {
                this.Updating = updating;
                Name = updating.Name;
                Unit = updating.Unit;
                Type = updating.Type;
                IsItemized = updating.IsItemized;

                Calories = updating.Calories.ToString();
                Protein = updating.Protein.ToString();
                Cholesterol = updating.Cholesterol.ToString();
                Sodium = updating.Sodium.ToString();

                FatTotal = updating.Fat.Total.ToString();
                FatSaturated = updating.Fat.Saturated.ToString();
                FatPolyunsaturated = updating.Fat.Polyunsaturated.ToString();
                FatMonounsaturated = updating.Fat.Monounsaturated.ToString();
                FatTrans = updating.Fat.Trans.ToString();

                CarbohydratesTotal = updating.Carbohydrate.Total.ToString();
                CarbohydratesDietaryFiber = updating.Carbohydrate.DietaryFiber.ToString();
                CarbohydratesSugar = updating.Carbohydrate.Sugar.ToString();
            }
            else
            {
                Name = "";
                Unit = "g";
                Type = FoodType.RedMeat;
            }
        }

        public EditFoodViewModel(MainViewModel mainViewModel)
            : this(mainViewModel, null)
        {

        }

        private ICommand _saveFood;
        public ICommand SaveFood
        {
            get
            {
                if (_saveFood == null)
                {
                    _saveFood = new DelegateCommand(() =>
                    {
                        Food newFood = new Food()
                        {
                            Id = Updating?.Id,
                            Name = _name,
                            Unit = _unit,
                            IsItemized = _IsItemized,
                            Type = _type,
                            Calories = _calories,
                            Protein = _protein,
                            Cholesterol = _cholesterol,
                            Sodium = _sodium,
                            Fat = new Fats()
                            {
                                Total = _fatTotal,
                                Saturated = _fatSaturated,
                                Polyunsaturated = _fatPolyunsaturated,
                                Monounsaturated = _fatMonounsaturated,
                                Trans = _fatTrans
                            },
                            Carbohydrate = new Carbohydrates()
                            {
                                Total = _carbohydratesTotal,
                                DietaryFiber = _carbohydratesDietaryFiber,
                                Sugar = _carbohydratesSugar
                            }
                        };
                        mainViewModel.SaveFood(newFood);
                    });
                }
                return _saveFood;
            }
        }
    }
}
