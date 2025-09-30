using System;

namespace Task1
{
    public class Pupil
    {
        public virtual void Study()
        {
            Console.WriteLine("Ученик учится");
        }

        public virtual void Read()
        {
            Console.WriteLine("Ученик читает");
        }

        public virtual void Write()
        {
            Console.WriteLine("Ученик пишет");
        }

        public virtual void Relax()
        {
            Console.WriteLine("Ученик отдыхает");
        }
    }

    public class ExcelentPupil : Pupil
    {
        public override void Study()
        {
            Console.WriteLine("Отличник усердно учится и получает только пятерки");
        }

        public override void Read()
        {
            Console.WriteLine("Отличник читает много дополнительной литературы");
        }

        public override void Write()
        {
            Console.WriteLine("Отличник пишет аккуратно и без ошибок");
        }

        public override void Relax()
        {
            Console.WriteLine("Отличник отдыхает, читая научные книги");
        }
    }

    public class GoodPupil : Pupil
    {
        public override void Study()
        {
            Console.WriteLine("Хорошист старательно учится и получает четверки и пятерки");
        }

        public override void Read()
        {
            Console.WriteLine("Хорошист читает учебники и иногда дополнительную литературу");
        }

        public override void Write()
        {
            Console.WriteLine("Хорошист пишет аккуратно, иногда допускает мелкие ошибки");
        }

        public override void Relax()
        {
            Console.WriteLine("Хорошист отдыхает, играя в развивающие игры");
        }
    }

    public class BadPupil : Pupil
    {
        public override void Study()
        {
            Console.WriteLine("Двоечник не хочет учиться и получает двойки");
        }

        public override void Read()
        {
            Console.WriteLine("Двоечник читает только комиксы и мемы");
        }

        public override void Write()
        {
            Console.WriteLine("Двоечник пишет неаккуратно и с множеством ошибок");
        }

        public override void Relax()
        {
            Console.WriteLine("Двоечник отдыхает, играя в компьютерные игры весь день");
        }
    }

    public class ClassRoom
    {
        private Pupil[] pupils;

        public ClassRoom(params Pupil[] pupils)
        {
            if (pupils.Length < 4)
            {
                this.pupils = new Pupil[4];
                for (int i = 0; i < pupils.Length; i++)
                {
                    this.pupils[i] = pupils[i];
                }
                for (int i = pupils.Length; i < 4; i++)
                {
                    this.pupils[i] = new Pupil();
                }
            }
            else
            {
                this.pupils = new Pupil[4];
                for (int i = 0; i < 4; i++)
                {
                    this.pupils[i] = pupils[i];
                }
            }
        }

        public void ShowClassInfo()
        {
            Console.WriteLine("=== Информация о классе ===");
            
            for (int i = 0; i < pupils.Length; i++)
            {
                Console.WriteLine($"\n--- Ученик {i + 1} ---");
                Console.WriteLine("Учеба:");
                pupils[i].Study();
                Console.WriteLine("Чтение:");
                pupils[i].Read();
                Console.WriteLine("Письмо:");
                pupils[i].Write();
                Console.WriteLine("Отдых:");
                pupils[i].Relax();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
        }
    }
}
