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
    using Shrekulator.Container;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using static Helpers.ShrekUnitType;

    internal static class Helpers
    {
        public static DependencyProperty DepPropReg<Type, Target>(string name) => DependencyProperty.Register(name, typeof(Type), typeof(Target), new PropertyMetadata(default(Type)));

        public static DependencyProperty DepPropReg<Type, Target>(string name, object defaultValue) => DependencyProperty.Register(name, typeof(Type), typeof(Target), new PropertyMetadata(defaultValue));

        private static DispatcherTimer _fetchScheduler;
        private static object _lockObject;

        public enum ShrekUnitType
        {
            Money,
            Time,
            Distance,
            Weight,
        }

        public static TrackableMap<ShrekUnitType, decimal> ShrekMap { get; }

        internal static string AppData { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        internal static string LangDir { get; } = Path.Combine(AppData, "lang");

        private static void FetchPrice(object sender, EventArgs e) =>
            Task.Run(async () =>
            {
                const string _priceTagBeginningTag = "Your price for this item is $";

                using (var wc = new WebClient())
                {
                    var res = await wc.DownloadStringTaskAsync("https://www.bestbuy.com/site/shrek-includes-digital-copy-blu-ray-dvd-2001/9660139.p");
                    var startingIndex = res.IndexOf(_priceTagBeginningTag) + _priceTagBeginningTag.Length;

                    var priceAsString = string.Concat(res.Substring(startingIndex).TakeWhile(ch => char.IsDigit(ch) || ch == '.').ToArray());

                    if (decimal.TryParse(priceAsString, out var price))
                    {
                        return price;
                    }
                    else
                    {
                        MessageBox.Show("Failed to fetch values from Best-Buy!");
                    }

                    return decimal.MinusOne;
                }
            }).ContinueWith(res => ShrekMap[Money] = res.Result);

        static Helpers()
        {
            _lockObject = new object();

            ShrekMap = new TrackableMap<ShrekUnitType, decimal>
            {
                [Money] = 14.99m, // in USD
                [Time] = TimeSpan.FromMinutes(95.0).Minutes,
                [Distance] = 10.0m, // in feet
                [Weight] = 650.0m, // in pounds
            };

            FetchPrice(null, null);

            _fetchScheduler = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromMinutes(15.0),
                IsEnabled = true,
            };

            _fetchScheduler.Tick += FetchPrice;
        }

        public static List<T> ListOf<T>(params T[] args) => new List<T>(args);
    }
}