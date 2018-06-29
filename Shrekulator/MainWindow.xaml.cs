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
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;
    using static Helpers.DPBuilder<MainWindow>;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string PostFmt = "{0} {2}";

        private readonly IReadOnlyList<string> quotes = Properties.Resources.Quotes.Split('\n');

        private readonly Queue<string> queuedMessages = new Queue<string>();

        private readonly Random rand = new Random();

        private readonly DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromMinutes(1.0), DispatcherPriority.Background, (o, e) => { }, Application.Current.Dispatcher)
        {
            IsEnabled = true
        };

        //private Category[] arr = new[]
        //{
        //        new Category(
        //             "Zeit",
        //             new List<Unit>
        //             {
        //                 new Unit("Millisekunden", "ms", 95 * 60 * 1000, PostFmt),
        //                 new Unit("Sekunden", "sec", 95 * 60, PostFmt),
        //                 new Unit("Minuten", "min", 95, PostFmt),
        //                 new Unit("Stunden", "h", 95 / 60, PostFmt),
        //                 new Unit("Tage", "d", 95 / 60 / 24, PostFmt),
        //                 new Unit("Wochen", "w", 95 / 60 / 24 / 7, PostFmt),
        //                 new Unit("Monate", "m", 95 / 60 / 24 / 7 / 4, PostFmt),
        //                 new Unit("Jahre", "a", 95 / 60 / 24 / 7 / 4 / 12, PostFmt)
        //             }),

        //         new Category(
        //            "Entfernung",
        //            new List<Unit>
        //            {
        //                // Imperial Units, Shrek is considered to be 8 foot tall
        //                // so that's what we'll be using as a baseunit
        //                new Unit("Zoll", "'", 8 * 12, PostFmt),
        //                new Unit("Fuß", "\"", 8, PostFmt),
        //                new Unit("Yard", "yd", 8 / 3, PostFmt),
        //                new Unit("Meile", "mile", 8 / 3 / 1760, PostFmt),

        //                // Metric units
        //                new Unit("Millimeter", "mm", 8 * 30.48m * 1000, PostFmt),
        //                new Unit("Zentimeter", "cm", 8 * 30.48m, PostFmt),
        //                new Unit("Meter", "cm", 8 * 30.48m / 100, PostFmt),
        //                new Unit("Kilometer", "cm", 8 * 30.48m / 100 / 1000, PostFmt),
        //            }),
        //};

        public MainWindow()
        {
            InitializeComponent();
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

        public IReadOnlyList<Unit> AvailableUnits
        {
            get => (IReadOnlyList<Unit>)GetValue(AvailableUnitsProperty);
            set => SetValue(AvailableUnitsProperty, value);
        }

        public DependencyProperty AvailableUnitsProperty = AnyDP<IReadOnlyList<Unit>>(nameof(AvailableUnits));

        private void SetupAnimation(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock miscText &&
                TryFindResource("FadeStoryboard") is Storyboard sb)
            {
                sb.Completed += async (o, arg) =>
                {
                    var idx = rand.Next(0, quotes.Count);
                    await miscText.Dispatcher.InvokeAsync(() => miscText.Text = (queuedMessages.Count > 0 ? queuedMessages.Dequeue() : quotes[idx]));
                    sb.Begin(miscText);
                };

                sb.Begin(miscText);
            }
        }

        private void CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox categorySelection &&
                categorySelection.SelectedValue is Category value)
            {
                AvailableUnits = value.Units;
            }
        }

        private void UnitChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox unitSelection)
            {
                if (unitSelection.SelectedValue is Unit value)
                {
                    MessageBox.Show(value.Name);
                }
                else
                {
                    unitSelection.SelectedIndex = 0;
                }
            }
        }

        private void LoadCategories(object sender, RoutedEventArgs e)
        {
            if (sender is ComboBox categorySelection)
            {
                categorySelection.DisplayMemberPath = nameof(Category.Name);
                categorySelection.ItemsSource = new List<Category>(from file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.udef", SearchOption.TopDirectoryOnly) select Category.Parse(File.ReadAllText(file)));
                categorySelection.SelectedIndex = 0;
            }
        }
    }
}