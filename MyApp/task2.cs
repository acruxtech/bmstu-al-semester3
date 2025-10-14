using System;
using System.Collections.Generic;

namespace Task2
{
    public class Car
    {
        public string Name { get; init; }
        public int ProductionYear { get; init; }
        public int MaxSpeed { get; init; }

        public Car(string name, int productionYear, int maxSpeed)
        {
            Name = name;
            ProductionYear = productionYear;
            MaxSpeed = maxSpeed;
        }

        public override string ToString()
        {
            return $"{Name} ({ProductionYear}), {MaxSpeed} km/h";
        }
    }

    public class CarComparer : IComparer<Car>
    {
        private readonly string key;

        public CarComparer(string key)
        {
            this.key = key;
        }

        public int Compare(Car? x, Car? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            switch (key)
            {
                case "name":
                    return x.Name.CompareTo(y.Name);
                case "year":
                    return x.ProductionYear.CompareTo(y.ProductionYear);
                case "speed":
                case "maxspeed":
                    return x.MaxSpeed.CompareTo(y.MaxSpeed);
                default:
                    return 0;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var cars = new[]
            {
                new Car("Audi A4", 2018, 240),
                new Car("BMW 320i", 2016, 235),
                new Car("Toyota Camry", 2020, 210),
                new Car("Volkswagen Golf", 2015, 220),
                new Car("Mercedes C200", 2019, 245)
            };

            Array.Sort(cars, new CarComparer("name"));
            Print("По названию", cars);

            Array.Sort(cars, new CarComparer("year"));
            Print("По году", cars);

            Array.Sort(cars, new CarComparer("speed"));
            Print("По макс скорости", cars);
        }

        private static void Print(string title, Car[] cars)
        {
            Console.WriteLine(title);
            foreach (var car in cars)
            {
                Console.WriteLine(car);
            }
            Console.WriteLine();
        }
    }
}


