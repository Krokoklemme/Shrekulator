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
    using static Helpers;

    /// <summary>
    /// Interaction logic for ConverterInterface.xaml
    /// </summary>
    public partial class ConverterInterface : UserControl
    {
        private enum Props
        {
            GroupTitle,
            WatermarkText,
            Text,
            WatermarkShow,
            BtnContent,
            ResText,
        }

        private static readonly Dictionary<Props, DependencyProperty> props = new Dictionary<Props, DependencyProperty>
        {
            [Props.GroupTitle] = DepPropReg<string, ConverterInterface>(nameof(GroupingTitle)),
            [Props.WatermarkText] = DepPropReg<string, ConverterInterface>(nameof(WatermarkText)),
            [Props.Text] = DepPropReg<string, ConverterInterface>(nameof(Text)),
            [Props.WatermarkShow] = DepPropReg<Visibility, ConverterInterface>(nameof(WatermarkVisibility)),
            [Props.BtnContent] = DepPropReg<object, ConverterInterface>(nameof(ButtonContent), string.Empty),
            [Props.ResText] = DepPropReg<string, ConverterInterface>(nameof(ResultText)),
        };

        public string GroupingTitle
        {
            get { return (string)GetValue(props[Props.GroupTitle]); }
            set { SetValue(props[Props.GroupTitle], value); }
        }

        public string WatermarkText
        {
            get { return (string)GetValue(props[Props.WatermarkText]); }
            set { SetValue(props[Props.WatermarkText], value); }
        }

        public string Text
        {
            get { return (string)GetValue(props[Props.Text]); }
            set { SetValue(props[Props.Text], value); }
        }

        public Visibility WatermarkVisibility
        {
            get { return (Visibility)GetValue(props[Props.WatermarkShow]); }
            set { SetValue(props[Props.WatermarkShow], value); }
        }

        public object ButtonContent
        {
            get { return GetValue(props[Props.BtnContent]); }
            set { SetValue(props[Props.BtnContent], value); }
        }

        public string ResultText
        {
            get { return (string)GetValue(props[Props.ResText]); }
            set { SetValue(props[Props.ResText], value); }
        }

        public event TextChangedEventHandler TextChanged;

        private void OnTextChanged(object sender, TextChangedEventArgs arg)
        {
            UpdateComponentLabels();
            TextChanged?.Invoke(sender, arg);
        }

        public event EventHandler SomethingHappened;

        public ConverterInterface()
        {
            // this'll make the hints hide and appear again
            TextChanged += (o, e) => WatermarkVisibility = (string.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Hidden);
            InitializeComponent();
        }

        private void UpdateComponentLabels()
        {

        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateComponentLabels();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ResultText))
            {
                if (decimal.TryParse(ResultText.Split(' ')[0] ?? string.Empty, out var number))
                {

                }
                else
                {
                    MessageBox.Show("how tf you mess *this* up?");
                }
            }
        }
    }
}