using System;

namespace Task2
{
    public class Vehicle
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }
        public decimal Price { get; protected set; }
        public double Speed { get; protected set; }
        public int Year { get; protected set; }

        protected Vehicle() { }

        public Vehicle(double x, double y, decimal price, double speed, int year)
        {
            X = x;
            Y = y;
            Price = price;
            Speed = speed;
            Year = year;
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"Координаты: ({X}, {Y})");
            Console.WriteLine($"Цена: {Price:C}");
            Console.WriteLine($"Скорость: {Speed} км/ч");
            Console.WriteLine($"Год выпуска: {Year}");
        }
    }

    public class Plane : Vehicle
    {
        public double Altitude { get; }
        public int Passengers { get; }

        public Plane(double x, double y, decimal price, double speed, int year, double altitude, int passengers)
        {
            X = x;
            Y = y;
            Price = price;
            Speed = speed;
            Year = year;
            Altitude = altitude;
            Passengers = passengers;
        }

        public override void PrintInfo()
        {
            Console.WriteLine("Самолет:");
            base.PrintInfo();
            Console.WriteLine($"Высота: {Altitude} м");
            Console.WriteLine($"Пассажиров: {Passengers}");
        }
    }

    public class Car : Vehicle
    {
        public Car(double x, double y, decimal price, double speed, int year)
        {
            X = x;
            Y = y;
            Price = price;
            Speed = speed;
            Year = year;
        }

        public override void PrintInfo()
        {
            Console.WriteLine("Автомобиль:");
            base.PrintInfo();
        }
    }

    public class Ship : Vehicle
    {
        public int Passengers { get; }
        public string Port { get; }

        public Ship(double x, double y, decimal price, double speed, int year, int passengers, string port)
        {
            X = x;
            Y = y;
            Price = price;
            Speed = speed;
            Year = year;
            Passengers = passengers;
            Port = port;
        }

        public override void PrintInfo()
        {
            Console.WriteLine("Корабль:");
            base.PrintInfo();
            Console.WriteLine($"Пассажиров: {Passengers}");
            Console.WriteLine($"Порт приписки: {Port}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Plane plane = new Plane(55.75, 37.62, 150000000m, 900, 2018, 10000, 220);
            Car car = new Car(59.93, 30.36, 2500000m, 180, 2022);
            Ship ship = new Ship(60.00, 31.00, 500000000m, 40, 2010, 3000, "Москва");

            Vehicle[] vehicles = { plane, car, ship };

            Console.WriteLine("=== Информация о транспортных средствах ===");
            foreach (Vehicle v in vehicles)
            {
                v.PrintInfo();
                Console.WriteLine(new string('-', 40));
            }

            Console.ReadKey();
        }
    }
}


