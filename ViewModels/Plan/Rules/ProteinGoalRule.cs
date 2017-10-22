using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietPlanner.ViewModels.Settings;
using Prism.Mvvm;

namespace DietPlanner.ViewModels.Plan.Rules
{
    public class ProteinGoalRule : BindableBase, IRule
    {
        private PlanDayViewModel day;
        private SettingsViewModel settings;

        public ProteinGoalRule(PlanDayViewModel day, SettingsViewModel settings)
        {
            this.day = day;
            this.settings = settings;

            day.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Protein")
                {
                    OnPropertyChanged(nameof(IsViolated));
                    OnPropertyChanged(nameof(ViolatedText));
                }
            };
        }

        private double Required()
        {
            return settings.ProteinGoal * settings.Weight;
        }

        public bool IsViolated
        {
            get { return day.Protein - Required() < 0; }
        }

        public string ViolatedText
        {
            get
            {
                return "Protein gotal of " + Required().ToString("F1") + " not meet, " + (Required() - day.Protein).ToString("F1") + " missing.";
            }
        }
    }
}
