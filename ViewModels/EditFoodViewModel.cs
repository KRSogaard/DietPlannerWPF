using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.Models;
using DietPlanner.ViewModels.Common;
using DietPlanner.WPF;
using Prism.Commands;
using Prism.Common;
using Prism.Mvvm;

namespace DietPlanner.ViewModels
{
    public class EditFoodViewModel : ConsumableViewModel
    {
        private Action<EditFoodViewModel> _saveCallback;
        public EditFoodViewModel(ConsumableViewModel editing, Action<EditFoodViewModel> saveCallback) 
            : base()
        {
            _saveCallback = saveCallback;
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
                        _saveCallback(this);
                    });
                }
                return _saveFood;
            }
        }
    }
}
