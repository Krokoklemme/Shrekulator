/*
        DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE 
                    Version 2, December 2004 

 Copyright (C) 2018 Henning Hoppe

 Everyone is permitted to copy and distribute verbatim or modified 
 copies of this license document, and changing it is allowed as long 
 as the name is changed. 

            DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE 
   TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION 

0. You just DO WHAT THE FUCK YOU WANT TO.
*/

namespace Shrekulator
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows;

    internal static class Helpers
    {
        public const string Empty = "";

        //public static string SanitizeCasing(this string @this) =>
        //    @this.Split(' ')
        //                .Select(
        //                    str => str
        //                        .Substring(1)
        //                        .ToLowerInvariant()
        //                        .Insert(0, str[0]
        //                            .ToString()
        //                            .ToUpperInvariant()
        //                        )
        //                    )
        //                .Aggregate(string.Empty, (lhs, rhs) => string.Concat(lhs, " ", rhs));

        public static void Log(string text, params string[] notes)
        {
            var time = DateTime.Now;
            var log = $"Log_{time.ToString("yyyy_MM_dd")}.txt";

            using (var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, log), true, Encoding.UTF8)
            {
                AutoFlush = false,
                NewLine = "\n",
            })
            {
                writer.WriteLine($"[{time.ToString("hh:mm:ss")}]: {text}");

                foreach (var line in notes)
                {
                    writer.Write("    - ");
                    writer.WriteLine(line);
                }

                writer.WriteLine();
                writer.Flush();
            }
        }

        public static class DPBuilder<TTarget>
        {
            private static readonly Type[] commonTypes = new[]
            {
                typeof(TTarget),
                typeof(object),
                typeof(string),
                typeof(int),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(bool),
            };

            private static DependencyProperty RegHelper(string name, object defaultValue, Type propType) => DependencyProperty.Register(name, propType, commonTypes[0], new PropertyMetadata(defaultValue));

            public static DependencyProperty AnyDP<TProp>(string name, TProp defaultValue = default) => RegHelper(name, defaultValue, typeof(TProp));

            public static DependencyProperty ObjectDP(string name, object defaultValue = default) => RegHelper(name, defaultValue, commonTypes[1]);

            public static DependencyProperty StringDP(string name, string defaultValue = default) => RegHelper(name, defaultValue, commonTypes[2]);

            public static DependencyProperty IntDP(string name, int defaultValue = default) => RegHelper(name, defaultValue, commonTypes[3]);

            public static DependencyProperty LongDP(string name, long defaultValue = default) => RegHelper(name, defaultValue, commonTypes[4]);

            public static DependencyProperty FloatDP(string name, float defaultValue = default) => RegHelper(name, defaultValue, commonTypes[5]);

            public static DependencyProperty DoubleDP(string name, double defaultValue = default) => RegHelper(name, defaultValue, commonTypes[6]);

            public static DependencyProperty DecimalDP(string name, decimal defaultValue = default) => RegHelper(name, defaultValue, commonTypes[7]);

            public static DependencyProperty BoolDP(string name, bool defaultValue = default) => RegHelper(name, defaultValue, commonTypes[8]);
        }
    }
}
