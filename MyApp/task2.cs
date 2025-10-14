using System;
using System.Collections;
using System.Collections.Generic;

namespace Task2
{
    public class MyList<T> : IEnumerable<T>
    {
        private T[] items;
        private int count;
        private int capacity;

        public int Count => count;

        public int Capacity => capacity;

        public MyList()
        {
            capacity = 4;
            items = new T[capacity];
            count = 0;
        }

        public MyList(int initialCapacity)
        {
            if (initialCapacity < 0)
                throw new ArgumentException("Емкость не может быть отрицательной");
            
            capacity = initialCapacity > 0 ? initialCapacity : 4;
            items = new T[capacity];
            count = 0;
        }

        public void Add(T item)
        {
            if (count >= capacity)
            {
                Resize();
            }
            items[count] = item;
            count++;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException($"Индекс {index} выходит за границы списка (0-{count - 1})");
                return items[index];
            }
            set
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException($"Индекс {index} выходит за границы списка (0-{count - 1})");
                items[index] = value;
            }
        }

        private void Resize()
        {
            capacity *= 2;
            T[] newItems = new T[capacity];
            for (int i = 0; i < count; i++)
            {
                newItems[i] = items[i];
            }
            items = newItems;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            count = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(items[i], item))
                    return true;
            }
            return false;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(items[i], item))
                    return i;
            }
            return -1;
        }

        public void Show()
        {
            Console.Write("[");
            for (int i = 0; i < count; i++)
            {
                Console.Write(items[i]);
                if (i < count - 1)
                    Console.Write(", ");
            }
            Console.WriteLine("]");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация работы с MyList<T> ===");

            // Создание списка целых чисел
            Console.WriteLine("\n1. Создание списка целых чисел:");
            MyList<int> intList = new MyList<int>();
            Console.WriteLine($"Начальная емкость: {intList.Capacity}, количество элементов: {intList.Count}");

            // Добавление элементов
            Console.WriteLine("\n2. Добавление элементов:");
            for (int i = 1; i <= 5; i++)
            {
                intList.Add(i * 10);
                Console.WriteLine($"Добавлен элемент {i * 10}, емкость: {intList.Capacity}, количество: {intList.Count}");
            }

            // Вывод списка
            Console.WriteLine("\n3. Содержимое списка:");
            intList.Show();

            // Использование индексатора
            Console.WriteLine("\n4. Использование индексатора:");
            Console.WriteLine($"intList[0] = {intList[0]}");
            Console.WriteLine($"intList[2] = {intList[2]}");
            Console.WriteLine($"intList[4] = {intList[4]}");

            // Изменение элемента через индексатор
            Console.WriteLine("\n5. Изменение элемента через индексатор:");
            Console.WriteLine($"До изменения: intList[1] = {intList[1]}");
            intList[1] = 999;
            Console.WriteLine($"После изменения: intList[1] = {intList[1]}");
            intList.Show();

            // Создание списка строк
            Console.WriteLine("\n6. Создание списка строк:");
            MyList<string> stringList = new MyList<string>();
            stringList.Add("Hello");
            stringList.Add("World");
            stringList.Add("C#");
            stringList.Add("Programming");
            stringList.Show();

            // Использование foreach (благодаря IEnumerable<T>)
            Console.WriteLine("\n7. Использование foreach:");
            Console.Write("Элементы списка строк: ");
            foreach (string item in stringList)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            // Демонстрация инициализатора коллекции
            Console.WriteLine("\n8. Демонстрация инициализатора коллекции:");
            MyList<double> doubleList = new MyList<double> { 1.5, 2.7, 3.14, 4.2, 5.0 };
            Console.WriteLine($"Список чисел с плавающей точкой:");
            doubleList.Show();
            Console.WriteLine($"Количество элементов: {doubleList.Count}");

            // Дополнительные методы
            Console.WriteLine("\n9. Дополнительные методы:");
            Console.WriteLine($"Содержит ли список число 3.14? {doubleList.Contains(3.14)}");
            Console.WriteLine($"Индекс числа 2.7: {doubleList.IndexOf(2.7)}");
            Console.WriteLine($"Индекс несуществующего числа: {doubleList.IndexOf(10.0)}");

            // Очистка списка
            Console.WriteLine("\n10. Очистка списка:");
            Console.WriteLine($"До очистки: количество = {doubleList.Count}");
            doubleList.Clear();
            Console.WriteLine($"После очистки: количество = {doubleList.Count}");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
