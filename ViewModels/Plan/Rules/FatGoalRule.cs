using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietPlanner.ViewModels.Settings;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan.Rules
{
    public class FatGoalRule : BindableBase, IRule
    {
        private PlanDayViewModel day;
        private SettingsViewModel settings;

        public FatGoalRule(PlanDayViewModel day, SettingsViewModel settings)
        {
            this.day = day;
            this.settings = settings;

            day.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "FatTotal")
                {
                    OnPropertyChanged(nameof(IsViolated));
                    OnPropertyChanged(nameof(ViolatedText));
                }
            };
        }

        private double Required()
        {
            return settings.FatGoal * settings.Weight;
        }

        public bool IsViolated
        {
            get { return day.FatTotal - Required() < 0; }
        }

        public string ViolatedText
        {
            get
            {
                return "Fat gotal of " + Required().ToString("F1") + " not meet, " + (Required() - day.FatTotal).ToString("F1") + " missing.";
            }
        }
    }
}
