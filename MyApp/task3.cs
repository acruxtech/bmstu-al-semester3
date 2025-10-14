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
}


