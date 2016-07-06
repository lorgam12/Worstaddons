namespace KappaUtilityReborn.Main.Common
{
    using System;

    internal class Logger
    {
        public static void Error(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "Kutility] Error: " + error);
            Console.ResetColor();
        }

        public static void Info(string info)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "Kutility] Info: " + info);
            Console.ResetColor();
        }

        public static void Warn(string Warn)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(DateTime.Now.ToString("[H:mm:ss - ") + "Kutility] Warn: " + Warn);
            Console.ResetColor();
        }
    }
}
