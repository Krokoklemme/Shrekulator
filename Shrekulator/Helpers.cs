// This is free and unencumbered software released into the public domain.
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a
// compiled binary, for any purpose, commercial or non-commercial,
// and by any means.
// In jurisdictions that recognize copyright laws, the author or
// authors of this software dedicate any and all copyright interest
// in the software to the public domain. We make this dedication for
// the benefit of the public at large and to the detriment of our
// heirs and successors. We intend this dedication to be an overt act
// of relinquishment in perpetuity of all present and future rights to
// this software under copyright law.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// For more information, please refer to <https://unlicense.org>

namespace Shrekulator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    internal static class Helpers
    {
        internal static class List
        {
            public static List<TType> Of<TType>(params TType[] range) => new List<TType>(range);
        }

        private static Random _rand;

        public static Random Rand => _rand ?? (_rand = new Random());

        public static TType SelectRandom<TType>(this IReadOnlyList<TType> obj) => obj[Rand.Next(0, obj.Count - 1)];

        public static ICollection<TType> Append<TType>(this ICollection<TType> obj, TType item)
        {
            obj.Add(item);
            return obj;
        }

        public static ICollection<TType> Append<TType>(this ICollection<TType> obj, IEnumerable<TType> range)
        {
            foreach (var item in range)
            {
                obj.Add(item);
            }

            return obj;
        }

        public static ICollection<TType> Prepend<TType>(this ICollection<TType> obj, TType item)
        {
            var buffer = new List<TType>(obj.Count() + 1)
            {
                [0] = item
            };

            buffer.AddRange(obj);

            return buffer;
        }

        public static ICollection<TType> Prepend<TType>(this ICollection<TType> obj, IEnumerable<TType> range)
        {
            var buffer = new List<TType>(obj.Count() + range.Count());

            buffer.AddRange(range);
            buffer.AddRange(obj);

            return buffer;
        }

        public static string SanitizeCasing(this string @this) =>
            @this.Split(' ')
                        .Select(
                            str => str
                                .Substring(1)
                                .ToLowerInvariant()
                                .Insert(0, str[0]
                                    .ToString()
                                    .ToUpperInvariant()
                                )
                            )
                        .Aggregate(string.Empty, (lhs, rhs) => string.Concat(lhs, " ", rhs));

        public static void Log(string text, params string[] notes)
        {
            Console.Error.WriteLine($"[{DateTime.UtcNow}] ::: {text}");

            if (notes.Length > 0)
            {
                foreach (var line in notes)
                {
                    Console.Error.WriteLine($"\t{line}");
                }

                Console.Error.WriteLine();
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