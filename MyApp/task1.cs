using System;

namespace Task1
{
    public class MyMatrix
    {
        private double[,] data;

        public int Rows { get; set; }
        public int Cols { get; set; }

        public double this[int rowIndex, int colIndex]
        {
            get
            {
                return data[rowIndex, colIndex];
            }
            set
            {
                data[rowIndex, colIndex] = value;
            }
        }

        public MyMatrix()
        {
            Rows = ReadInt("Введите количество строк: ");
            Cols = ReadInt("Введите количество столбцов: ");
            data = new double[Rows, Cols];

            FillWithUserInput();
        }

        private MyMatrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new double[Rows, Cols];
        }

        private int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? s = Console.ReadLine();
                if (int.TryParse(s, out int value) && value > 0)
                {
                    return value;
                }
                Console.WriteLine("Введите положительное целое число.");
            }
        }

        private void FillWithUserInput()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    while (true)
                    {
                        Console.Write($"a[{i},{j}]= ");
                        string? s = Console.ReadLine();
                        if (double.TryParse(s, out double value))
                        {
                            data[i, j] = value;
                            break;
                        }
                        Console.WriteLine("Введите число.");
                    }
                }
            }
        }

        public static MyMatrix operator +(MyMatrix left, MyMatrix right)
        {
            if (left.Rows != right.Rows || left.Cols != right.Cols) throw new ArgumentException();

            var result = new MyMatrix(left.Rows, left.Cols);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Cols; j++)
                {
                    result.data[i, j] = left.data[i, j] + right.data[i, j];
                }
            }
            return result;
        }

        public static MyMatrix operator -(MyMatrix left, MyMatrix right)
        {
            if (left.Rows != right.Rows || left.Cols != right.Cols) throw new ArgumentException();

            var result = new MyMatrix(left.Rows, left.Cols);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Cols; j++)
                {
                    result.data[i, j] = left.data[i, j] - right.data[i, j];
                }
            }
            return result;
        }

        public static MyMatrix operator *(MyMatrix left, MyMatrix right)
        {
            if (left.Cols != right.Rows) throw new ArgumentException();

            var result = new MyMatrix(left.Rows, right.Cols);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < left.Cols; k++)
                    {
                        sum += left.data[i, k] * right.data[k, j];
                    }
                    result.data[i, j] = sum;
                }
            }
            return result;
        }

        public static MyMatrix operator *(MyMatrix matrix, double scalar)
        {
            var result = new MyMatrix(matrix.Rows, matrix.Cols);
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    result.data[i, j] = matrix.data[i, j] * scalar;
                }
            }
            return result;
        }

        public static MyMatrix operator /(MyMatrix matrix, double divisor)
        {
            if (divisor == 0) throw new DivideByZeroException();
            var result = new MyMatrix(matrix.Rows, matrix.Cols);
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    result.data[i, j] = matrix.data[i, j] / divisor;
                }
            }
            return result;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    sb.Append(data[i, j]);
                    sb.Append('\t');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация работы с матрицами ===");
            
            Console.WriteLine("Создание матрицы A:");
            MyMatrix matrixA = new MyMatrix();
            
            Console.WriteLine("Создание матрицы B:");
            MyMatrix matrixB = new MyMatrix();
            
            Console.WriteLine("\nМатрица A:");
            Console.WriteLine(matrixA);
            
            Console.WriteLine("Матрица B:");
            Console.WriteLine(matrixB);
            
            // Сложение
            MyMatrix sum = matrixA + matrixB;
            Console.WriteLine("A + B:");
            Console.WriteLine(sum);
            
            // Вычитание
            MyMatrix diff = matrixA - matrixB;
            Console.WriteLine("A - B:");
            Console.WriteLine(diff);
            
            // Умножение на скаляр
            MyMatrix scaled = matrixA * 2.5;
            Console.WriteLine("A * 2.5:");
            Console.WriteLine(scaled);
            
            // Деление на скаляр
            MyMatrix divided = matrixA / 2.0;
            Console.WriteLine("A / 2.0:");
            Console.WriteLine(divided);
            
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
