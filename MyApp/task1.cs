using System;

namespace Task1
{
    public class MyMatrix
    {
        private int[,] data;
        private static Random random = new Random();
        private static int minValue = 0;
        private static int maxValue = 10;

        public int Rows { get; set; }
        public int Cols { get; set; }

        public int this[int rowIndex, int colIndex]
        {
            get
            {
                if (rowIndex < 0 || rowIndex >= Rows || colIndex < 0 || colIndex >= Cols)
                    throw new IndexOutOfRangeException("Индекс выходит за границы матрицы");
                return data[rowIndex, colIndex];
            }
            set
            {
                if (rowIndex < 0 || rowIndex >= Rows || colIndex < 0 || colIndex >= Cols)
                    throw new IndexOutOfRangeException("Индекс выходит за границы матрицы");
                data[rowIndex, colIndex] = value;
            }
        }

        public MyMatrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            data = new int[Rows, Cols];
            Fill();
        }

        public MyMatrix()
        {
            Rows = ReadInt("Введите количество строк: ");
            Cols = ReadInt("Введите количество столбцов: ");
            data = new int[Rows, Cols];
            Fill();
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

        public void Fill()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    data[i, j] = random.Next(minValue, maxValue + 1);
                }
            }
        }

        public void ChangeSize(int newRows, int newCols)
        {
            int[,] newData = new int[newRows, newCols];
            
            int copyRows = Math.Min(Rows, newRows);
            int copyCols = Math.Min(Cols, newCols);
            
            for (int i = 0; i < copyRows; i++)
            {
                for (int j = 0; j < copyCols; j++)
                {
                    newData[i, j] = data[i, j];
                }
            }
            
            for (int i = 0; i < newRows; i++)
            {
                for (int j = 0; j < newCols; j++)
                {
                    if (i >= copyRows || j >= copyCols)
                    {
                        newData[i, j] = random.Next(minValue, maxValue + 1);
                    }
                }
            }
            
            data = newData;
            Rows = newRows;
            Cols = newCols;
        }

        public void ShowPartially(int startRow, int endRow, int startCol, int endCol)
        {
            if (startRow < 0 || endRow >= Rows || startCol < 0 || endCol >= Cols || 
                startRow > endRow || startCol > endCol)
            {
                Console.WriteLine("Некорректные границы для вывода");
                return;
            }

            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    Console.Write($"{data[i, j]}\t");
                }
                Console.WriteLine();
            }
        }

        public void Show()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Console.Write($"{data[i, j]}\t");
                }
                Console.WriteLine();
            }
        }

        public static void SetRandomRange()
        {
            Console.WriteLine("Введите диапазон случайных чисел:");
            minValue = ReadIntStatic("Минимальное значение: ");
            maxValue = ReadIntStatic("Максимальное значение: ");
        }

        private static int ReadIntStatic(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? s = Console.ReadLine();
                if (int.TryParse(s, out int value))
                {
                    return value;
                }
                Console.WriteLine("Введите целое число.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация работы с матрицами ===");
            
            MyMatrix.SetRandomRange();
            
            Console.WriteLine("\nСоздание матрицы A (3x3):");
            MyMatrix matrixA = new MyMatrix(3, 3);
            
            Console.WriteLine("\nМатрица A:");
            matrixA.Show();
            
            Console.WriteLine("\nЧастичный вывод матрицы A (строки 0-1, столбцы 0-1):");
            matrixA.ShowPartially(0, 1, 0, 1);
            
            // Демонстрация изменения размера
            Console.WriteLine("\nИзменение размера матрицы A на 4x4:");
            matrixA.ChangeSize(4, 4);
            matrixA.Show();
            
            Console.WriteLine("\nПерезаполнение матрицы A:");
            matrixA.Fill();
            matrixA.Show();
            
            Console.WriteLine("\nДоступ к элементам через индексатор:");
            Console.WriteLine($"matrixA[0,0] = {matrixA[0, 0]}");
            Console.WriteLine($"matrixA[1,1] = {matrixA[1, 1]}");
            
            matrixA[0, 0] = 99;
            Console.WriteLine("После изменения matrixA[0,0] = 99:");
            matrixA.Show();
            
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
