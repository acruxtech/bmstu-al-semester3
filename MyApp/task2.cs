namespace Task2
{
    using System.Collections.Generic;

    public class Car : IEquatable<Car>
    {
        public string Name { get; }
        public string Engine { get; }
        public int MaxSpeed { get; }

        public Car(string name, string engine, int maxSpeed)
        {
            Name = name;
            Engine = engine;
            MaxSpeed = maxSpeed;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(Car? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Engine == other.Engine && MaxSpeed == other.MaxSpeed;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Car);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Engine, MaxSpeed);
        }

        public static bool operator ==(Car? left, Car? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Car? left, Car? right)
        {
            return !(left == right);
        }
    }

    public class CarsCatalog
    {
        private List<Car> cars = new List<Car>();

        public void Add(Car car)
        {
            cars.Add(car);
        }

        public string this[int index]
        {
            get
            {
                Car car = cars[index];
                return $"{car.Name} ({car.Engine})";
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Car a4 = new Car("Audi A4", "2.0", 240);
            Car a4Same = new Car("Audi A4", "2.0", 240);
            Car bmw = new Car("BMW 320i", "2.0", 235);

            Console.WriteLine(a4);

            Console.WriteLine($"==: {a4 == a4Same}, Equals: {a4.Equals(a4Same)}, !=: {a4 != bmw}");

            CarsCatalog catalog = new CarsCatalog();
            foreach (var car in new[] { a4, bmw }) catalog.Add(car);
            for (int i = 0; i < 2; i++) Console.WriteLine($"Catalog[{i}]: {catalog[i]}");
        }
    }
}


