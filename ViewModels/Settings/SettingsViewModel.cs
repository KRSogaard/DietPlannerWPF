using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietPlanner.ViewModels.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Settings
{
    public class SettingsViewModel : BindableBase
    {
        private string filePathSettings = "Data/Settings.json";
        private Settings settings;
        private MainViewModel mainViewModel;

        public SettingsViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (!File.Exists(filePathSettings))
            {
                settings = new Settings()
                {
                    Weight = 258,
                    Age = 31,
                    FatGoal = 0.5,
                    Height = 180,
                    ProteinGoal = 1
                };
                SaveSettings();
                return;
            }

            var conten = File.ReadAllText(filePathSettings, Encoding.UTF8);
            settings = JsonConvert.DeserializeObject<Settings>(conten);

            OnPropertyChanged(nameof(Weight));
            OnPropertyChanged(nameof(Age));
            OnPropertyChanged(nameof(Height));
            OnPropertyChanged(nameof(ProteinGoal));
            OnPropertyChanged(nameof(FatGoal));

        }

        private void SaveSettings()
        {
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(filePathSettings, json, Encoding.UTF8);
        }

        public double Weight
        {
            get { return settings.Weight; }
            set
            {
                settings.Weight = value;
                OnPropertyChanged(nameof(Weight));
                SaveSettings();
            }
        }

        public int Age
        {
            get { return settings.Age; }
            set
            {
                settings.Age = value;
                OnPropertyChanged(nameof(Age));
                SaveSettings();
            }
        }

        public int Height
        {
            get { return settings.Height; }
            set
            {
                settings.Height = value;
                OnPropertyChanged(nameof(Height));
                SaveSettings();
            }
        }

        public double ProteinGoal
        {
            get { return settings.ProteinGoal; }
            set
            {
                settings.ProteinGoal = value;
                OnPropertyChanged(nameof(ProteinGoal));
                SaveSettings();
            }
        }

        public double FatGoal
        {
            get { return settings.FatGoal; }
            set
            {
                settings.FatGoal = value;
                OnPropertyChanged(nameof(FatGoal));
                SaveSettings();
            }
        }

        public class Settings
        {
            public int Age { get; set; }
            public double FatGoal { get; set; }
            public double ProteinGoal { get; set; }
            public int Height { get; set; }
            public double Weight { get; set; }
        }
    }
}
