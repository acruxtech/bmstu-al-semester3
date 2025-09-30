using System;

namespace Task1
{
    public struct Vector
    {
        public double x;
        public double y;
        public double z;

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double Length() {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static double operator *(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vector operator *(Vector a, double k)
        {
            return new Vector(a.x * k, a.y * k, a.z * k);
        }

        public static Vector operator *(double k, Vector a)
        {
            return a * k;
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.Length() == b.Length();
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        public static bool operator <(Vector a, Vector b)
        {
            return a.Length() < b.Length();
        }

        public static bool operator >(Vector a, Vector b)
        {
            return a.Length() > b.Length();
        }

        public static bool operator <=(Vector a, Vector b)
        {
            return a.Length() <= b.Length();
        }

        public static bool operator >=(Vector a, Vector b)
        {
            return a.Length() >= b.Length();
        }

        public override bool Equals(object? obj)
        {
            if (obj is Vector v)
            {
                return x == v.x && y == v.y && z == v.z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}) |L|={Length()}";
        }
    }

 
    class Program
    {
        static void Main(string[] args)
        {
            Vector v1 = new Vector(1, 2, 3);
            Vector v2 = new Vector(3, 2, 1);

            Console.WriteLine($"v1 = {v1}");
            Console.WriteLine($"v2 = {v2}");

            var sum = v1 + v2;
            Console.WriteLine($"v1 + v2 = {sum}");

            var dot = v1 * v2;
            Console.WriteLine($"v1 * v2 (dot) = {dot}");

            var scaled1 = v1 * 2;
            var scaled2 = 2 * v2;
            Console.WriteLine($"v1 * 2 = {scaled1}");
            Console.WriteLine($"2 * v2 = {scaled2}");

            Console.WriteLine($"v1 == v2 ? { (v1 == v2) }");
            Console.WriteLine($"v1 != v2 ? { (v1 != v2) }");
            Console.WriteLine($"v1 < v2 ? { (v1 < v2) }");
            Console.WriteLine($"v1 > v2 ? { (v1 > v2) }");
            Console.WriteLine($"v1 <= v2 ? { (v1 <= v2) }");
            Console.WriteLine($"v1 >= v2 ? { (v1 >= v2) }");

            Console.ReadKey();
        }
    }
}


