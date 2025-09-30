using System;

namespace Task3
{
    public class DocumentWorker
    {
        public virtual void OpenDocument()
        {
            Console.WriteLine("Документ открыт");
        }

        public virtual void EditDocument()
        {
            Console.WriteLine("Редактирование документа доступно в версии Pro");
        }

        public virtual void SaveDocument()
        {
            Console.WriteLine("Сохранение документа доступно в версии Pro");
        }
    }

    public class ProDocumentWorker : DocumentWorker
    {
        public override void EditDocument()
        {
            Console.WriteLine("Документ отредактирован");
        }

        public override void SaveDocument()
        {
            Console.WriteLine("Документ сохранен в старом формате, сохранение в остальных форматах доступно в версии Expert");
        }
    }

    public class ExpertDocumentWorker : ProDocumentWorker
    {
        public override void SaveDocument()
        {
            Console.WriteLine("Документ сохранен в новом формате");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const string proKey = "pro123";
            const string expKey = "exp123";

            Console.WriteLine("Введите ключ доступа (pro/exp). Пусто — бесплатная версия:");
            string input = Console.ReadLine();

            DocumentWorker worker;
            if (input == expKey)
            {
                worker = new ExpertDocumentWorker();
                Console.WriteLine("Активирована версия: Expert");
            }
            else if (input == proKey)
            {
                worker = new ProDocumentWorker();
                Console.WriteLine("Активирована версия: Pro");
            }
            else
            {
                worker = new DocumentWorker();
                Console.WriteLine("Активирована версия: Free");
            }

            worker.OpenDocument();
            worker.EditDocument();
            worker.SaveDocument();

            Console.ReadKey();
        }
    }
}


