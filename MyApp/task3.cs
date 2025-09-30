using System;
using System.Globalization;

namespace Task3
{
    public abstract class Currency
    {
        public decimal Value { get; set; }

        protected Currency() {}

        protected Currency(decimal value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Value}";
        }
    }

    public static class CurrencyRates
    {
        public static decimal UsdToRub { get; set; } = 0;
        public static decimal EurToRub { get; set; } = 0;
    }

    public class CurrencyUSD : Currency
    {
        public CurrencyUSD(decimal value)
        {
            Value = value;
        }

        // USD -> RUB (неявно)
        public static implicit operator CurrencyRUB(CurrencyUSD usd)
        {
            return new CurrencyRUB(usd.Value * CurrencyRates.UsdToRub);
        }

        // USD -> EUR (явно через RUB)
        public static explicit operator CurrencyEUR(CurrencyUSD usd)
        {
            decimal rub = usd.Value * CurrencyRates.UsdToRub;
            decimal eur = CurrencyRates.EurToRub == 0 ? 0 : rub / CurrencyRates.EurToRub;
            return new CurrencyEUR(eur);
        }
    }

    public class CurrencyEUR : Currency
    {
        public CurrencyEUR(decimal value)
        {
            Value = value;
        }

        // EUR -> RUB (неявно)
        public static implicit operator CurrencyRUB(CurrencyEUR eur)
        {
            return new CurrencyRUB(eur.Value * CurrencyRates.EurToRub);
        }

        // EUR -> USD (явно через RUB)
        public static explicit operator CurrencyUSD(CurrencyEUR eur)
        {
            decimal rub = eur.Value * CurrencyRates.EurToRub;
            decimal usd = CurrencyRates.UsdToRub == 0 ? 0 : rub / CurrencyRates.UsdToRub;
            return new CurrencyUSD(usd);
        }
    }

    public class CurrencyRUB : Currency
    {
        public CurrencyRUB(decimal value)
        {
            Value = value;
        }

        // RUB -> USD (явно)
        public static explicit operator CurrencyUSD(CurrencyRUB rub)
        {
            decimal usd = CurrencyRates.UsdToRub == 0 ? 0 : rub.Value / CurrencyRates.UsdToRub;
            return new CurrencyUSD(usd);
        }

        // RUB -> EUR (явно)
        public static explicit operator CurrencyEUR(CurrencyRUB rub)
        {
            decimal eur = CurrencyRates.EurToRub == 0 ? 0 : rub.Value / CurrencyRates.EurToRub;
            return new CurrencyEUR(eur);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите курс USD->RUB:");
            CurrencyRates.UsdToRub = ReadDecimal();

            Console.WriteLine("Введите курс EUR->RUB:");
            CurrencyRates.EurToRub = ReadDecimal();

            CurrencyUSD usd = new CurrencyUSD(3);
            CurrencyEUR eur = new CurrencyEUR(3);
            CurrencyRUB rubFromUsd = usd; // неявно в RUB
            CurrencyRUB rubFromEur = eur; // неявно в RUB
            CurrencyUSD usdFromRub = (CurrencyUSD)rubFromEur; // явно
            CurrencyEUR eurFromUsd = (CurrencyEUR)usd;        // явно

            Console.WriteLine(usd);
            Console.WriteLine(eur);
            Console.WriteLine(rubFromUsd);
            Console.WriteLine(rubFromEur);
            Console.WriteLine(usdFromRub);
            Console.WriteLine(eurFromUsd);

            Console.ReadKey();
        }

        private static decimal ReadDecimal()
        {
            string? s = Console.ReadLine();
            if (s == null) return 0m;
            decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out var v);
            return v;
        }
    }
}

