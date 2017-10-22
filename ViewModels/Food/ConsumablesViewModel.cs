using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DietPlanner.ViewModels.Common;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Food
{
    public class ConsumablesViewModel : BindableBase
    {
        private string filePathFood = "Data/Foods.json";
        private MainViewModel mainViewModel;

        public ObservableCollection<ConsumableViewModel> Consumables { get; set; }

        public ConsumablesViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            
            Consumables = new ObservableCollection<ConsumableViewModel>();
            LoadConsumables();
        }
        
        private EditConsumableViewModel editingConsumable;
        public EditConsumableViewModel EditingConsumable
        {
            get { return editingConsumable; }
            set
            {
                SetProperty(ref editingConsumable, value);
                ShowEditing = value != null;
            }
        }

        private ConsumableViewModel selectedConsumable;
        public ConsumableViewModel SelectedConsumable
        {
            get { return selectedConsumable; }
            set { SetProperty(ref selectedConsumable, value); }
        }

        private bool showEditing;
        public bool ShowEditing
        {
            get { return showEditing; }
            set
            {
                SetProperty(ref showEditing, value);
                // Update the title
                OnPropertyChanged(nameof(EditingTitle));
            }
        }

        public string EditingTitle
        {
            get
            {
                if (EditingConsumable == null)
                    return "";
                if (String.IsNullOrWhiteSpace(EditingConsumable.Name))
                    return "Create new food";
                return $"Edit {EditingConsumable.Name}";
            }
        }

        private ICommand _create;
        public ICommand Create
        {
            get
            {
                if (_create == null)
                {
                    _create = new DelegateCommand(() =>
                    {
                        EditingConsumable = new EditConsumableViewModel(null, SaveConsumableUpdate);
                    });
                }
                return _create;
            }
        }

        private ICommand _editSlectedConsumable;
        public ICommand EditSlectedConsumable
        {
            get
            {
                if (_editSlectedConsumable == null)
                {
                    _editSlectedConsumable = new DelegateCommand(() =>
                    {
                        if (SelectedConsumable == null)
                            return;
                        EditingConsumable = new EditConsumableViewModel(SelectedConsumable, SaveConsumableUpdate);
                    });
                }
                return _editSlectedConsumable;
            }
        }

        private void SaveConsumableUpdate(ConsumableViewModel x)
        {
            EditingConsumable = null;
            // new food item
            if (x.Id == null)
            {
                x.Id = Guid.NewGuid().ToString();
                Consumables.Add(x);
            }
            else
            {
                // Update existing food item
                var food = Consumables.FirstOrDefault(y => y.Id == x.Id);
                if (food == null)
                {
                    Consumables.Add(x);
                }
                else
                {
                    food.Clone(x);
                }
            }
            SaveConsumables();
        }

        private void SaveConsumables()
        {
            var json = JsonConvert.SerializeObject(Consumables);
            File.WriteAllText(filePathFood, json, Encoding.UTF8);
        }

        private void LoadConsumables()
        {
            var conten = File.ReadAllText(filePathFood, Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<ConsumableViewModel>>(conten);
            Consumables.Clear();
            Consumables.AddRange(list);
        }
    }
}
