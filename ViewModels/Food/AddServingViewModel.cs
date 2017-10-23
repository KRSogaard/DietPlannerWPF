using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.ViewModels.Common;
using Prism.Commands;

namespace DietPlanner.ViewModels.Food
{
    public class AddServingViewModel : ConsumableViewModel
    {
        private Action<ConsumableViewModel> _saveCallback;

        private double _servingSize;

        public double ServingSize
        {
            get { return _servingSize; }
            set { SetProperty(ref _servingSize, value); }
        }

        public AddServingViewModel(Action<ConsumableViewModel> saveCallback) 
            : base()
        {
            _saveCallback = saveCallback;
            Unit = "g";
            ServingSize = 100;

            this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != nameof(CanSave))
                {
                    OnPropertyChanged(nameof(CanSave));
                }

            };
        }

        private ICommand _save;
        public ICommand Save
        {
            get
            {
                if (_save == null)
                {
                    _save = new DelegateCommand(() =>
                    {
                        ConsumableViewModel newModel = new ConsumableViewModel();
                        newModel.Clone(this);

                        if (!IsItemized)
                        {
                            newModel.Calories /= ServingSize;
                            newModel.Cholesterol /= ServingSize;
                            newModel.Sodium /= ServingSize;
                            newModel.Protein /= ServingSize;
                            newModel.Fat.Total /= ServingSize;
                            newModel.Fat.Saturated /= ServingSize;
                            newModel.Fat.Trans /= ServingSize;
                            newModel.Fat.Polyunsaturated /= ServingSize;
                            newModel.Fat.Monounsaturated /= ServingSize;
                            newModel.Carbohydrates.Total /= ServingSize;
                            newModel.Carbohydrates.DietaryFiber /= ServingSize;
                            newModel.Carbohydrates.Sugar /= ServingSize;
                        }

                        _saveCallback(newModel);
                    });
                }
                return _save;
            }
        }

        public bool CanSave
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name) &&
                       !string.IsNullOrWhiteSpace(Unit) &&
                       ServingSize > 0;
            }
        }
    }
}
