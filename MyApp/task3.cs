using System;
using System.Collections;
using System.Collections.Generic;

namespace Task3
{
    public struct KeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public KeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"[{Key}, {Value}]";
        }
    }

    public class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private KeyValuePair<TKey, TValue>[] items;
        private int count;
        private int capacity;

        public int Count => count;

        public int Capacity => capacity;

        public MyDictionary()
        {
            capacity = 4;
            items = new KeyValuePair<TKey, TValue>[capacity];
            count = 0;
        }

        public MyDictionary(int initialCapacity)
        {
            if (initialCapacity < 0)
                throw new ArgumentException("Емкость не может быть отрицательной");
            
            capacity = initialCapacity > 0 ? initialCapacity : 4;
            items = new KeyValuePair<TKey, TValue>[capacity];
            count = 0;
        }

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
                throw new ArgumentException($"Ключ '{key}' уже существует в словаре");

            if (count >= capacity)
            {
                Resize();
            }
            
            items[count] = new KeyValuePair<TKey, TValue>(key, value);
            count++;
        }

        public TValue this[TKey key]
        {
            get
            {
                int index = IndexOfKey(key);
                if (index == -1)
                    throw new KeyNotFoundException($"Ключ '{key}' не найден в словаре");
                return items[index].Value;
            }
            set
            {
                int index = IndexOfKey(key);
                if (index == -1)
                {
                    Add(key, value);
                }
                else
                {
                    items[index] = new KeyValuePair<TKey, TValue>(key, value);
                }
            }
        }

        private int IndexOfKey(TKey key)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<TKey>.Default.Equals(items[i].Key, key))
                    return i;
            }
            return -1;
        }

        public bool ContainsKey(TKey key)
        {
            return IndexOfKey(key) != -1;
        }

        public bool ContainsValue(TValue value)
        {
            for (int i = 0; i < count; i++)
            {
                if (EqualityComparer<TValue>.Default.Equals(items[i].Value, value))
                    return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = IndexOfKey(key);
            if (index != -1)
            {
                value = items[index].Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public bool Remove(TKey key)
        {
            int index = IndexOfKey(key);
            if (index == -1)
                return false;

            // Сдвигаем элементы влево
            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }
            count--;
            return true;
        }

        public void Clear()
        {
            count = 0;
        }

        private void Resize()
        {
            capacity *= 2;
            KeyValuePair<TKey, TValue>[] newItems = new KeyValuePair<TKey, TValue>[capacity];
            for (int i = 0; i < count; i++)
            {
                newItems[i] = items[i];
            }
            items = newItems;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
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

        public void Show()
        {
            Console.Write("{");
            for (int i = 0; i < count; i++)
            {
                Console.Write($"{items[i].Key}: {items[i].Value}");
                if (i < count - 1)
                    Console.Write(", ");
            }
            Console.WriteLine("}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация работы с MyDictionary<TKey, TValue> ===");

            // Создание словаря строк
            Console.WriteLine("\n1. Создание словаря строк:");
            MyDictionary<string, int> stringDict = new MyDictionary<string, int>();
            Console.WriteLine($"Начальная емкость: {stringDict.Capacity}, количество элементов: {stringDict.Count}");

            // Добавление элементов
            Console.WriteLine("\n2. Добавление элементов:");
            stringDict.Add("один", 1);
            stringDict.Add("два", 2);
            stringDict.Add("три", 3);
            stringDict.Add("четыре", 4);
            stringDict.Add("пять", 5);
            Console.WriteLine($"После добавления: емкость = {stringDict.Capacity}, количество = {stringDict.Count}");

            // Вывод словаря
            Console.WriteLine("\n3. Содержимое словаря:");
            stringDict.Show();

            // Использование индексатора для получения значений
            Console.WriteLine("\n4. Использование индексатора для получения значений:");
            Console.WriteLine($"stringDict[\"один\"] = {stringDict["один"]}");
            Console.WriteLine($"stringDict[\"три\"] = {stringDict["три"]}");
            Console.WriteLine($"stringDict[\"пять\"] = {stringDict["пять"]}");

            // Использование индексатора для изменения значений
            Console.WriteLine("\n5. Использование индексатора для изменения значений:");
            Console.WriteLine($"До изменения: stringDict[\"два\"] = {stringDict["два"]}");
            stringDict["два"] = 22;
            Console.WriteLine($"После изменения: stringDict[\"два\"] = {stringDict["два"]}");

            // Добавление нового элемента через индексатор
            Console.WriteLine("\n6. Добавление нового элемента через индексатор:");
            stringDict["шесть"] = 6;
            stringDict.Show();

            // Создание словаря с разными типами
            Console.WriteLine("\n7. Создание словаря с разными типами:");
            MyDictionary<int, string> intDict = new MyDictionary<int, string>();
            intDict.Add(1, "Понедельник");
            intDict.Add(2, "Вторник");
            intDict.Add(3, "Среда");
            intDict.Add(4, "Четверг");
            intDict.Add(5, "Пятница");
            intDict.Show();

            // Использование foreach
            Console.WriteLine("\n8. Использование foreach:");
            Console.Write("Элементы словаря: ");
            foreach (var kvp in intDict)
            {
                Console.Write($"{kvp.Key}={kvp.Value} ");
            }
            Console.WriteLine();

            // Дополнительные методы
            Console.WriteLine("\n9. Дополнительные методы:");
            Console.WriteLine($"Содержит ли ключ '3'? {intDict.ContainsKey(3)}");
            Console.WriteLine($"Содержит ли значение 'Суббота'? {intDict.ContainsValue("Суббота")}");
            
            if (intDict.TryGetValue(2, out string day))
            {
                Console.WriteLine($"Значение для ключа '2': {day}");
            }

            // Удаление элемента
            Console.WriteLine("\n10. Удаление элемента:");
            Console.WriteLine($"До удаления: количество = {intDict.Count}");
            bool removed = intDict.Remove(4);
            Console.WriteLine($"Удален элемент с ключом '4': {removed}");
            Console.WriteLine($"После удаления: количество = {intDict.Count}");
            intDict.Show();

            // Очистка словаря
            Console.WriteLine("\n11. Очистка словаря:");
            Console.WriteLine($"До очистки: количество = {intDict.Count}");
            intDict.Clear();
            Console.WriteLine($"После очистки: количество = {intDict.Count}");

            // Демонстрация обработки исключений
            Console.WriteLine("\n12. Демонстрация обработки исключений:");
            try
            {
                Console.WriteLine($"Попытка получить несуществующий ключ: {stringDict["несуществующий"]}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
            }

            try
            {
                stringDict.Add("один", 10); // Попытка добавить существующий ключ
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
