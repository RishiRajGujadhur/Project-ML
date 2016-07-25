using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Titanic1
    {
        private int _Passenger_Class;
        private double? _Average_Age;
        private int? _Minimum_Age;
        private int? _Max_Age;
        public Titanic1(
            int APassenger_Class, double? AAverage_Age, int? AMinimum_Age,
            int? AMax_Age)
        {
            _Passenger_Class = APassenger_Class;
            _Average_Age = AAverage_Age;
            _Minimum_Age = AMinimum_Age;
            _Max_Age = AMax_Age;
        }
        public int Passenger_Class { get { return _Passenger_Class; } }
        public double? Average_Age { get { return _Average_Age; } }
        public int? Minimum_Age { get { return _Minimum_Age; } }
        public int? Max_Age { get { return _Max_Age; } }
    }
    public class TitanicList : List<Titanic1>
    {
        public TitanicList(AzureDBEntities db)
        {
            var query =
                from Titanics in db.Titanics
                where
                  Titanics.Passenger_class != null
                group Titanics by new
                {
                    Titanics.Passenger_class
                } into g
                select new
                {
                    Passenger_Class = g.Key.Passenger_class,
                    Average_Age = (double?)g.Average(p => p.Age),
                    Minimum_Age = (int?)g.Min(p => p.Age),
                    Max_Age = (int?)g.Max(p => p.Age)
                };     
        }
    }

}