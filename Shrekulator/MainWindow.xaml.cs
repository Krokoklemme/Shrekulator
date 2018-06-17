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
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using static Helpers.DPBuilder<MainWindow>;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly IReadOnlyList<string> quotes = Properties.Resources.Quotes.Split('\n');

        private readonly Queue<string> queuedMessages = new Queue<string>();

        private readonly Timer timer = new Timer
        {
            AutoReset = true,
            Enabled = true,
            Interval = TimeSpan.FromMinutes(1).TotalMilliseconds,
        };

        private readonly FileSystemWatcher watcher = new FileSystemWatcher(".")
        {
            Filter = "*.udef",
            IncludeSubdirectories = false,
            NotifyFilter =
                NotifyFilters.FileName |
                NotifyFilters.CreationTime |
                NotifyFilters.LastWrite,
            Path = AppDomain.CurrentDomain.BaseDirectory,
        };

        #region IDisposable Support

        private bool disposedValue = false;

        void IDisposable.Dispose()
        {
            if (!disposedValue)
            {
                watcher.Dispose();
                timer.Dispose();

                disposedValue = true;
            }
        }

        #endregion IDisposable Support

        public MainWindow()
        {
            InitializeComponent();

            timer.Elapsed += async (o, e) =>
            {
                var request = WebRequest.Create(@"https://min-api.cryptocompare.com/data/price?fsym=SHREK&tsyms=USD,EUR&extraParams=Shrekulator");

                using (var response = await request.GetResponseAsync().ConfigureAwait(false))
                using (var responseStream = response.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    var result = await reader.ReadToEndAsync();
                    var valueTree = JObject.Parse(result);

                    var usdValue = valueTree.GetValue("USD").ToObject<decimal>();
                    var eurValue = valueTree.GetValue("EUR").ToObject<decimal>();

                    Application.Current.Dispatcher.Invoke(() => CoinTickerText = $"ShrekCoin ::: USD: {usdValue}      EUR: {eurValue}");
                }
            };

            var bfr = new List<Category>();

            foreach (var defFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.udef", SearchOption.TopDirectoryOnly))
            {
                var fullPath = Path.GetFullPath(defFile);
                bfr.Add(Category.Load(fullPath));
            }

            watcher.Deleted += (o, e) =>
            {
                
            };

            watcher.Created += (o, e) =>
            {
                var ctg = Category.Load(e.FullPath);
                var queriedCategories = from category in LoadedCategories
                                        where category.Name == ctg.Name
                                        select category;

                if (queriedCategories.Count() == 0)
                {
                    LoadedCategories.Add(ctg);
                }
                else
                {
                    var ctgToReplace = queriedCategories.ElementAt(0);
                    var idx = LoadedCategories.IndexOf(ctgToReplace);
                    LoadedCategories[idx] = ctg;
                }
            };

            watcher.Changed += (o, e) =>
            {
            };

            watcher.Renamed += (o, e) =>
            {
            };

            watcher.EnableRaisingEvents = true;

            Closing += (o, e) => (this as IDisposable).Dispose();
        }

        public string CoinTickerText
        {
            get => (string)GetValue(CoinTickerTextProperty);
            set => SetValue(CoinTickerTextProperty, value);
        }

        public DependencyProperty CoinTickerTextProperty = StringDP(nameof(CoinTickerText), "Fetching value...");

        public string InputText
        {
            get => (string)GetValue(InputTextProperty);
            set => SetValue(InputTextProperty, value);
        }

        public DependencyProperty InputTextProperty = StringDP(nameof(InputText));

        public string ResultText
        {
            get => (string)GetValue(ResultTextProperty);
            set => SetValue(ResultTextProperty, value);
        }

        public DependencyProperty ResultTextProperty = StringDP(nameof(ResultText));

        public IList<Category> LoadedCategories
        {
            get => (IList<Category>)GetValue(LoadedCategoriesProperty);
            set => SetValue(LoadedCategoriesProperty, value);
        }

        public DependencyProperty LoadedCategoriesProperty = AnyDP<IList<Category>>(nameof(LoadedCategories));

        public IList<Unit> AvailableUnits
        {
            get => (IList<Unit>)GetValue(AvailableUnitsProperty);
            set => SetValue(AvailableUnitsProperty, value);
        }

        public DependencyProperty AvailableUnitsProperty = AnyDP<IList<Unit>>(nameof(AvailableUnits));

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
                    await miscText.Dispatcher.InvokeAsync(() => miscText.Text = (queuedMessages.Count > 0 ? queuedMessages.Dequeue() : quotes.SelectRandom()));
                    sb.Begin(miscText);
                };

                sb.Begin(miscText);
            }
        }

        private void CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox categorySelection)
            {
                if (categorySelection.SelectedValue is Category)
                {
                    MessageBox.Show("yup");
                }
            }
        }

        private void UnitChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox unitSelection)
            {
                ResultText = unitSelection.SelectedIndex.ToString();
            }
        }
    }
}