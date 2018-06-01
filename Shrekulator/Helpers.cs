// Shrekulator - Tool to convert several units of measurements to Shrek's
// Copyright(C) 2018 Henning Hoppe
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<https://www.gnu.org/licenses/>.

namespace Shrekulator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;

    internal static class Helpers
    {
        private static Random _rand;

        public static Random Rand => _rand ?? (_rand = new Random());

        private static StringBuilder _builder;

        public static StringBuilder Builder => _builder ?? (_builder = new StringBuilder());

        private static object _syncObj;

        public static object SyncObj => _syncObj ?? (_syncObj = new object());

        public static T SelectRandom<T>(this IReadOnlyList<T> inst) => inst[Rand.Next(0, inst.Count - 1)];

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
                        .Aggregate(string.Empty, (a, s) => string.Join(" ", a, s));

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