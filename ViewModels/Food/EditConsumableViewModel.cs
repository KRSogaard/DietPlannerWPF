using System;
using System.Windows.Input;
using DietPlanner.ViewModels.Common;
using Prism.Commands;

namespace DietPlanner.ViewModels.Food
{
    public class EditConsumableViewModel : ConsumableViewModel
    {
        private Action<ConsumableViewModel> _saveCallback;
        public EditConsumableViewModel(ConsumableViewModel editing, Action<ConsumableViewModel> saveCallback) 
            : base()
        {
            _saveCallback = saveCallback;
            if (editing != null)
            {
                Clone(editing);
            }
            else
            {
                // Default values
                Unit = "g";
            }

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
                        _saveCallback(this);
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
                       !string.IsNullOrWhiteSpace(Unit);
            }
        }
    }
}
