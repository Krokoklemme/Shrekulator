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
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using static Helpers.DPBuilder<MainWindow>;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly Dictionary<string, List<Unit>> lookupTables = new Dictionary<string, List<Unit>>();

        private readonly IReadOnlyList<string> quotes = Properties.Resources.Quotes.Split('\n');

        private readonly Queue<string> queuedMessages = new Queue<string>();

        private readonly FileSystemWatcher watcher = new FileSystemWatcher(".")
        {
            EnableRaisingEvents = true,
            Filter = "*.udef",
            IncludeSubdirectories = false,
            NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
            // Path = AppDomain.CurrentDomain.BaseDirectory,
        };

        #region IDisposable Support

        private bool disposedValue = false;

        void IDisposable.Dispose()
        {
            if (!disposedValue)
            {
                watcher.Dispose();

                disposedValue = true;
            }
        }

        #endregion IDisposable Support

        public MainWindow()
        {
            InitializeComponent();

            Closing += (o, e) => (this as IDisposable).Dispose();
        }

        public int TickerUpdateFrequency
        {
            get => (int)GetValue(TickerUpdateFrequencyProperty);
            set => SetValue(TickerUpdateFrequencyProperty, value);
        }

        public DependencyProperty TickerUpdateFrequencyProperty = IntDP(nameof(TickerUpdateFrequency));

        public string InputText
        {
            get => (string)GetValue(InputTextProperty);
            set => SetValue(InputTextProperty, value);
        }

        public DependencyProperty InputTextProperty = StringDP(nameof(InputText));

        //static MainWindow()
        //{
        //    var baseVal = 14.99m;

        //    Unit.LookupTables.Add(new List<Unit>
        //    {
        //        new Unit("USD", "$", baseVal, Unit.PrefixSymbol),
        //        new Unit("Euro", "€", 1.18m * baseVal),
        //        new Unit("Pound",  "1", 1.34m * baseVal, Unit.PrefixSymbol),
        //    });

        //    baseVal = 95m;

        //    Unit.LookupTables.Add(new List<Unit>
        //    {
        //        new Unit("Microseconds", "µs", baseVal * 60 * 1000 * 1000),
        //        new Unit("Milliseconds", "ms", baseVal * 60 * 1000),
        //        new Unit("Seconds", "sec", baseVal * 60),
        //        new Unit("Minutes", "min", baseVal),
        //        new Unit("Hours", "h", baseVal / 60),
        //        new Unit("Days", "d", baseVal / 60 / 24),
        //        new Unit("Weeks", "w", baseVal / 60 / 24 / 7),
        //        new Unit("Months", "m", baseVal / 60 / 24 / 7 / 4),
        //        new Unit("Years", "a", baseVal / 60 / 24 / 7 / 4 / 12),
        //    });

        //    baseVal = 2.0066m;

        //    Unit.LookupTables.Add(new List<Unit>
        //    {
        //        new Unit("Micrometers", "µm", baseVal * 100 * 100 * 100),
        //        new Unit("Millimeters", "mm", baseVal * 100 * 100),
        //        new Unit("Centimeters", "cm", baseVal * 100),
        //        new Unit("Decimeters", "dm", baseVal * 10),
        //        new Unit("Meters", "m", baseVal),
        //        new Unit("Kilometers", "km", baseVal / 1000),
        //    });

        //    baseVal = 94.34721m;

        //    Unit.LookupTables.Add(new List<Unit>
        //    {
        //        new Unit("Micrograms", "µg", baseVal * 1000 * 1000 * 1000),
        //        new Unit("Milligrams", "mg", baseVal * 1000 * 1000),
        //        new Unit("Grams", "g", baseVal * 1000),
        //        new Unit("Kilograms", "kg", baseVal),
        //        new Unit("Metric tonnes", "t", baseVal / 1000),
        //    });
        //}

        private void SetupAnimation(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock miscText && TryFindResource("FadeStoryboard") is Storyboard sb)
            {
                sb.Completed += async (o, arg) =>
                {
                    await miscText.Dispatcher.InvokeAsync(() => miscText.Text = quotes.SelectRandom());
                    sb.Begin(miscText);
                };

                sb.Begin(miscText);
            }
        }

        private void SetupShrekCoinTicker(object sender, RoutedEventArgs e)
        {
            if (sender is WebBrowser browser)
            {
            }
        }
    }
}