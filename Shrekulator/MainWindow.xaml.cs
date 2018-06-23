/*
This is free and unencumbered software released into the public domain.
Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a
compiled binary, for any purpose, commercial or non-commercial,
and by any means.
In jurisdictions that recognize copyright laws, the author or
authors of this software dedicate any and all copyright interest
in the software to the public domain. We make this dedication for
the benefit of the public at large and to the detriment of our
heirs and successors. We intend this dedication to be an overt act
of relinquishment in perpetuity of all present and future rights to
this software under copyright law.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
For more information, please refer to <https://unlicense.org>
*/

namespace Shrekulator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        private readonly IReadOnlyList<string> quotes = Properties.Resources.Quotes.Split('\n');

        private readonly Queue<string> queuedMessages = new Queue<string>();

        private readonly DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromMinutes(1.0), DispatcherPriority.Background, (o, e) => { }, Application.Current.Dispatcher)
        {
            IsEnabled = true
        };

        public MainWindow()
        {
            InitializeComponent();

            var bfr = new List<Category>();

            foreach (var defFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.udef", SearchOption.TopDirectoryOnly))
            {
                var fullPath = Path.GetFullPath(defFile);
                bfr.Add(Category.Load(fullPath));
            }
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