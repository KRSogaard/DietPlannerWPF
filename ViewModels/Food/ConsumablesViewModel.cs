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
        private string saveFileName = "Foods.json";
        private string filePathFood;
        private MainViewModel mainViewModel;

        public ObservableCollection<ConsumableViewModel> Consumables { get; set; }

        public ConsumablesViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            filePathFood = Path.Combine(mainViewModel.Settings.DataPath, saveFileName);

            Consumables = new ObservableCollection<ConsumableViewModel>();
            LoadConsumables();
            Consumables.CollectionChanged += (sender, args) =>
            {
                SaveConsumables();
            };
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

        private AddServingViewModel editingServingsConsumable;
        public AddServingViewModel EditingServingsConsumable
        {
            get { return editingServingsConsumable; }
            set
            {
                SetProperty(ref editingServingsConsumable, value);
                ShowServingsEditing = value != null;
            }
        }

        private bool showServingsEditing;
        public bool ShowServingsEditing
        {
            get { return showServingsEditing; }
            set
            {
                SetProperty(ref showServingsEditing, value);
            }
        }

        private ConsumableViewModel selectedConsumable;
        public ConsumableViewModel SelectedConsumable
        {
            get { return selectedConsumable; }
            set { SetProperty(ref selectedConsumable, value); }
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

        private ICommand _createServings;
        public ICommand CreateServings
        {
            get
            {
                if (_createServings == null)
                {
                    _createServings = new DelegateCommand(() =>
                    {
                        EditingServingsConsumable = new AddServingViewModel(SaveConsumableUpdate);
                    });
                }
                return _createServings;
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
            EditingServingsConsumable = null;
            // new food item
            if (x.Id == null)
            {
                var newModel = new ConsumableViewModel();
                newModel.Clone(x);
                newModel.Id = Guid.NewGuid().ToString();
                Consumables.Add(newModel);
            }
            else
            {
                // Update existing food item
                var food = Consumables.FirstOrDefault(y => y.Id == x.Id);
                if (food == null)
                {
                    var newModel = new ConsumableViewModel();
                    newModel.Clone(x);
                    Consumables.Add(newModel);
                }
                else
                {
                    food.Clone(x);
                    // We save when we change the collection, so this is the only place we need to call save
                    SaveConsumables();
                }
            }
        }

        private void SaveConsumables()
        {
            var json = JsonConvert.SerializeObject(Consumables);
            File.WriteAllText(filePathFood, json, Encoding.UTF8);
        }

        private void LoadConsumables()
        {
            if (!File.Exists(filePathFood))
            {
                return;
            }

            var conten = File.ReadAllText(filePathFood, Encoding.UTF8);
            var list = JsonConvert.DeserializeObject<List<ConsumableViewModel>>(conten);
            Consumables.Clear();
            Consumables.AddRange(list);
        }
    }
}
