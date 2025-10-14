using System;
using System.Collections;
using System.Collections.Generic;
using Task2;

namespace Task3
{
    public class CarCatalog : IEnumerable<Car>
    {
        private readonly Car[] cars;

        public CarCatalog(Car[] cars)
        {
            this.cars = cars;
        }

        public IEnumerator<Car> GetEnumerator()
        {
            for (int i = 0; i < cars.Length; i++)
            {
                yield return cars[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Car> Reverse()
        {
            for (int i = cars.Length - 1; i >= 0; i--)
            {
                yield return cars[i];
            }
        }

        public IEnumerable<Car> WhereYear(int productionYear)
        {
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i].ProductionYear == productionYear)
                {
                    yield return cars[i];
                }
            }
        }

        public IEnumerable<Car> WhereMaxSpeedAtLeast(int minMaxSpeed)
        {
            for (int i = 0; i < cars.Length; i++)
            {
                if (cars[i].MaxSpeed >= minMaxSpeed)
                {
                    yield return cars[i];
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация CarCatalog ===");
            
            Car[] cars = {
                new Car("Toyota Camry", 2020, 200),
                new Car("BMW X5", 2021, 250),
                new Car("Honda Civic", 2019, 180),
                new Car("Audi A4", 2020, 240),
                new Car("Mercedes C-Class", 2021, 220)
            };

            CarCatalog catalog = new CarCatalog(cars);

            Console.WriteLine("Все машины:");
            foreach (var car in catalog)
            {
                Console.WriteLine(car);
            }

            Console.WriteLine("\nМашины в обратном порядке:");
            foreach (var car in catalog.Reverse())
            {
                Console.WriteLine(car);
            }

            Console.WriteLine("\nМашины 2020 года:");
            foreach (var car in catalog.WhereYear(2020))
            {
                Console.WriteLine(car);
            }

            Console.WriteLine("\nМашины со скоростью >= 220 км/ч:");
            foreach (var car in catalog.WhereMaxSpeedAtLeast(220))
            {
                Console.WriteLine(car);
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}


