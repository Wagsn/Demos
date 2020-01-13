using System;

namespace WS.Extensions.System.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = "";
            text = "三万万";
            Console.WriteLine($"{text}: {text.ToArabicNumeral()}");
            text = "一亿零三千四";
            Console.WriteLine($"{text}: {text.ToArabicNumeral()}");
            text = "一亿二千三百四十五万六千七百八十九";
            Console.WriteLine($"{text}: {text.ToArabicNumeral()}");
            text = "一亿二千八十九";
            Console.WriteLine($"{text}: {text.ToArabicNumeral()}");
            text = "一亿二千八十九万万";
            Console.WriteLine($"{text}: {text.ToArabicNumeral()}");
            text = "一千五";
            Console.WriteLine($"{text}: {text.ToArabicNumeral()}");
        }
    }
}
