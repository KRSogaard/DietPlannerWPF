using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Models
{
    public enum FoodType
    {
        [Description("Red Meat")]
        RedMeat,
        Poultry,
        Fruit,
        Fish,
        Vegetables,
        Diary,
        Condument,
        Cerial,
        Bread,
        Seed,
        Meal,
        Oil,
        Nuts,
        Rice,
        Other,
        Supplement
    }
}
