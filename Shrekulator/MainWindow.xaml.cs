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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using static Helpers.DPBuilder<MainWindow>;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int TickerUpdateFrequency
        {
            get => (int)GetValue(TickerUpdateFrequencyProperty);
            set => SetValue(TickerUpdateFrequencyProperty, value);
        }
        
        public DependencyProperty TickerUpdateFrequencyProperty = IntDP(nameof(TickerUpdateFrequency));

        static MainWindow()
        {
            var baseVal = 14.99m;

            Unit.LookupTables.Add(new List<Unit>
            {
                new Unit("USD", "$", baseVal, Unit.PrefixSymbol),
                new Unit("Euro", "€", 1.18m * baseVal),
                new Unit("Pound",  "1", 1.34m * baseVal, Unit.PrefixSymbol),
            });

            baseVal = 95m;

            Unit.LookupTables.Add(new List<Unit>
            {
                new Unit("Microseconds", "µs", baseVal * 60 * 1000 * 1000),
                new Unit("Milliseconds", "ms", baseVal * 60 * 1000),
                new Unit("Seconds", "sec", baseVal * 60),
                new Unit("Minutes", "min", baseVal),
                new Unit("Hours", "h", baseVal / 60),
                new Unit("Days", "d", baseVal / 60 / 24),
                new Unit("Weeks", "w", baseVal / 60 / 24 / 7),
                new Unit("Months", "m", baseVal / 60 / 24 / 7 / 4),
                new Unit("Years", "a", baseVal / 60 / 24 / 7 / 4 / 12),
            });

            baseVal = 2.0066m;

            Unit.LookupTables.Add(new List<Unit>
            {
                new Unit("Micrometers", "µm", baseVal * 100 * 100 * 100),
                new Unit("Millimeters", "mm", baseVal * 100 * 100),
                new Unit("Centimeters", "cm", baseVal * 100),
                new Unit("Decimeters", "dm", baseVal * 10),
                new Unit("Meters", "m", baseVal),
                new Unit("Kilometers", "km", baseVal / 1000),
            });

            baseVal = 94.34721m;

            Unit.LookupTables.Add(new List<Unit>
            {
                new Unit("Micrograms", "µg", baseVal * 1000 * 1000 * 1000),
                new Unit("Milligrams", "mg", baseVal * 1000 * 1000),
                new Unit("Grams", "g", baseVal * 1000),
                new Unit("Kilograms", "kg", baseVal),
                new Unit("Metric tonnes", "t", baseVal / 1000),
            });
        }

        private void ManageDispatchTimer(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock miscText)
            {
                var animDuration = new Duration(TimeSpan.FromSeconds(3.0));
                var opacPropPath = new PropertyPath(OpacityProperty);

                NameScope.SetNameScope(miscText, new NameScope());

                var fadeInAnim = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = animDuration,
                    BeginTime = TimeSpan.Zero,
                    AutoReverse = false,
                };
                Storyboard.SetTarget(fadeInAnim, miscText);
                Storyboard.SetTargetProperty(fadeInAnim, opacPropPath);

                var fadeOutAnim = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = animDuration,
                    BeginTime = TimeSpan.FromSeconds(10.0),
                    AutoReverse = false,
                };
                Storyboard.SetTarget(fadeOutAnim, miscText);
                Storyboard.SetTargetProperty(fadeOutAnim, opacPropPath);

                var storyboard = new Storyboard
                {
                    AutoReverse = false,
                    BeginTime = TimeSpan.Zero,
                    Children = new TimelineCollection
                    {
                        fadeInAnim,
                        fadeOutAnim,
                    },
                };
                storyboard.Completed += async (o, arg) =>
                {
                    await miscText.Dispatcher.InvokeAsync(() => miscText.Text = (Application.Current as App).GetQuotes().SelectRandom());
                    storyboard.Begin(miscText);
                };

                storyboard.Begin(miscText);
            }
        }
    }
}