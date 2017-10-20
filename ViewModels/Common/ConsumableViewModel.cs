using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietPlanner.Models;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Common
{
    public class ConsumableViewModel : BindableBase
    {
        public ConsumableViewModel()
        {
            Fat = new FatViewModel();
            Carbohydrates = new CarbohydratesViewModel();
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set
            {
                SetProperty(ref _unit, value);
            }
        }

        private bool _isItemized;
        public bool IsItemized
        {
            get { return _isItemized; }
            set { SetProperty(ref _isItemized, value); }
        }

        private FoodType _type;
        public FoodType Type
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type, value);
            }
        }

        private double _calories;
        public double Calories
        {
            get { return _calories; }
            set
            {
                SetProperty(ref _calories, value);
            }
        }

        private double _cholesterol;
        public double Cholesterol
        {
            get { return _cholesterol; }
            set { SetProperty(ref _cholesterol, value); }
        }

        private double _sodium;
        public double Sodium
        {
            get { return _sodium; }
            set { SetProperty(ref _sodium, value); }
        }

        private double _protein;
        public double Protein
        {
            get { return _protein; }
            set { SetProperty(ref _protein, value); }
        }

        private FatViewModel _fat;
        public FatViewModel Fat
        {
            get { return _fat; }
            set
            {
                SetProperty(ref _fat, value);
            }
        }

        private CarbohydratesViewModel _carohydrates;
        public CarbohydratesViewModel Carbohydrates
        {
            get { return _carohydrates; }
            set { SetProperty(ref _carohydrates, value); }
        }

        public void clone(ConsumableViewModel o)
        {
            this.Id = o.Id;
            this.Name = o.Name;
            this.Unit = o.Unit;
            this.IsItemized = o.IsItemized;
            this.Calories = o.Calories;
            this.Type = o.Type;
            this.Cholesterol = o.Cholesterol;
            this.Sodium = o.Sodium;
            this.Protein = o.Protein;
            this.Fat.clone(o.Fat);
            this.Carbohydrates.clone(o.Carbohydrates);
        }
    }
}
