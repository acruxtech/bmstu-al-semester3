using System;

namespace Task1
{
    public class MyMatrix
    {
        private readonly int[,] data;

        public int Rows { get; }
        public int Cols { get; }

        public int this[int rowIndex, int colIndex]
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

        public MyMatrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new int[Rows, Cols];

            FillWithUserInput();
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
                        if (int.TryParse(s, out int value))
                        {
                            data[i, j] = value;
                            break;
                        }
                        Console.WriteLine("Введите целое число.");
                    }
                }
            }
        }

        public static MyMatrix operator +(MyMatrix left, MyMatrix right)
        {
            if (left.Rows != right.Rows || left.Cols != right.Cols) throw new ArgumentException();

            var result = new MyMatrix(left.Rows, left.Cols) { };
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

            var result = new MyMatrix(left.Rows, left.Cols) { };
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

            var result = new MyMatrix(left.Rows, right.Cols) { };
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Cols; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < left.Cols; k++)
                    {
                        sum += left.data[i, k] * right.data[k, j];
                    }
                    result.data[i, j] = sum;
                }
            }
            return result;
        }

        public static MyMatrix operator *(MyMatrix matrix, int scalar)
        {
            var result = new MyMatrix(matrix.Rows, matrix.Cols) { };
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    result.data[i, j] = matrix.data[i, j] * scalar;
                }
            }
            return result;
        }

        public static MyMatrix operator /(MyMatrix matrix, int divisor)
        {
            if (divisor == 0) throw new DivideByZeroException();
            var result = new MyMatrix(matrix.Rows, matrix.Cols) { };
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
}
