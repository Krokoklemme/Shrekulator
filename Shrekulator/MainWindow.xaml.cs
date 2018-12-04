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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using static Category;
    using static System.Text.Encoding;
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

        public MainWindow()
        {
            InitializeComponent();
        }

        public string InputText
        {
            get => (string)GetValue(InputTextProperty);
            set => SetValue(InputTextProperty, value);
        }

        public DependencyProperty InputTextProperty = StringDP(nameof(InputText), string.Empty);

        public string ResultText
        {
            get => (string)GetValue(ResultTextProperty);
            set => SetValue(ResultTextProperty, value);
        }

        public DependencyProperty ResultTextProperty = StringDP(nameof(ResultText));

        public IReadOnlyList<Category> AvailableCategories
        {
            get => (IReadOnlyList<Category>)GetValue(AvailableCategoriesProperty);
            set => SetValue(AvailableCategoriesProperty, value);
        }

        public DependencyProperty AvailableCategoriesProperty = AnyDP<IReadOnlyList<Category>>(nameof(AvailableCategories),
            new List<Category>(
                    new[]
                    {
                        Parse(UTF8.GetString(Properties.Resources.DistanceCategory)),
                        Parse(UTF8.GetString(Properties.Resources.TimeCategory)),
                    }
                    .Concat(
                        from file
                        in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.udef", SearchOption.TopDirectoryOnly)
                        select Parse(File.ReadAllText(file)))
                    .Distinct()));

        public IReadOnlyList<Unit> AvailableUnits
        {
            get => (IReadOnlyList<Unit>)GetValue(AvailableUnitsProperty);
            set => SetValue(AvailableUnitsProperty, value);
        }

        public DependencyProperty AvailableUnitsProperty = AnyDP<IReadOnlyList<Unit>>(nameof(AvailableUnits));

        public bool ShrekMode
        {
            get => (bool)GetValue(ShrekModeProperty);
            set => SetValue(ShrekModeProperty, value);
        }

        public DependencyProperty ShrekModeProperty = BoolDP(nameof(ShrekMode), true);

        public Unit SelectedUnit
        {
            get => (Unit)GetValue(SelectedUnitProperty);
            set => SetValue(SelectedUnitProperty, value);
        }

        public DependencyProperty SelectedUnitProperty = AnyDP<Unit>(nameof(SelectedUnit));

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

        private void ModeChanged(object sender, RoutedEventArgs e) => UpdateResultText();

        private void UpdateResultText()
        {
            if (InputText != string.Empty)
            {
                if (decimal.TryParse(InputText, out var result))
                {
                    var value = SelectedUnit.Convert(result, ShrekMode);
                    ResultText = SelectedUnit.ToString(value);
                }
                else
                {
                    ResultText = "Invalid input!";
                }
            }
            else
            {
                ResultText = string.Empty;
            }
        }

        private void CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox categorySelection &&
                categorySelection.SelectedValue is Category value)
            {
                AvailableUnits = value.Units;
                SelectedUnit = AvailableUnits[0];
            }
        }

        private void UnitChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox unitSelection)
            {
                if (unitSelection.SelectedValue is Unit value)
                {
                    UpdateResultText();
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
                categorySelection.ItemsSource = new List<Category>();
                categorySelection.SelectedIndex = 0;
            }
        }

        private void InputTextChanged(object sender, TextChangedEventArgs e) => UpdateResultText();
    }
}
