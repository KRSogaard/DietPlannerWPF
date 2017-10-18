using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Models
{
    public interface IConsumable
    {
        string Name { get; }
        string Unit { get; }
        FoodType Type { get; }

        double Calories { get; }
        Fats Fat { get; }
        double Cholesterol { get; }
        double Sodium { get; }
        Carbohydrates Carbohydrate { get; }
        double Protein { get; }
        VitaminList Vitamins { get; }
    }

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
        Other
    }

    public class Fats
    {
        public double Total { get; set; }
        public double Saturated { get; set; }
        public double Polyunsaturated { get; set; }
        public double Monounsaturated { get; set; }
        public double Trans { get; set; }

        public static Fats operator +(Fats x, Fats y)
        {
            Fats n = new Fats();

            n.Total = x.Total + y.Total;
            n.Saturated = x.Saturated + y.Saturated;
            n.Polyunsaturated = x.Polyunsaturated + y.Polyunsaturated;
            n.Monounsaturated = x.Monounsaturated + y.Monounsaturated;
            n.Trans = x.Trans + y.Trans;
            return n;
        }
        public static Fats operator *(Fats x, double multi)
        {
            Fats n = new Fats();

            n.Total = x.Total + multi;
            n.Saturated = x.Saturated + multi;
            n.Polyunsaturated = x.Polyunsaturated + multi;
            n.Monounsaturated = x.Monounsaturated + multi;
            n.Trans = x.Trans + multi;
            return n;
        }
    }

    public class Carbohydrates
    {
        public double Total { get; set; }
        public double DietaryFiber { get; set; }
        public double Sugar { get; set; }

        public static Carbohydrates operator +(Carbohydrates x, Carbohydrates y)
        {
            Carbohydrates n = new Carbohydrates();

            n.Total = x.Total + y.Total;
            n.DietaryFiber = x.DietaryFiber + y.DietaryFiber;
            n.Sugar = x.Sugar + y.Sugar;
            return n;
        }
        public static Carbohydrates operator *(Carbohydrates x, double multi)
        {
            Carbohydrates n = new Carbohydrates();

            n.Total = x.Total + multi;
            n.DietaryFiber = x.DietaryFiber + multi;
            n.Sugar = x.Sugar + multi;
            return n;
        }
    }

    public class VitaminList : List<Vitamin>
    {
        public static VitaminList operator *(VitaminList list, double units)
        {
            VitaminList newList = new VitaminList();
            foreach (Vitamin v in list)
            {
                newList.Add(new Vitamin()
                {
                    Type = v.Type,
                    Value = v.Value * units
                });
            }
            return newList;
        }
    }

    public class Vitamin
    {
        public VitaminType Type;
        public double Value;

        public class VitaminType
        {
            public string Name;
            public string Unit;

            public VitaminType()
            {
            }

            public VitaminType(string name, string unit)
            {
                this.Name = name;
                this.Unit = unit;
            }
        }

        public static List<VitaminType> GetVitamins()
        {
            return new List<VitaminType>()
            {
                new VitaminType("A", "mcg"),
                new VitaminType("B", "mcg"),
                new VitaminType("C", "mcg"),
                new VitaminType("D", "mcg"),
                new VitaminType("Calcium", "mg"),
                new VitaminType("Iron", "mg"),
                new VitaminType("Potassium", "mg"),
            };
        }
    }
}
