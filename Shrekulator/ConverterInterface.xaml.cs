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
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using static Helpers;

    /// <summary>
    /// Interaction logic for ConverterInterface.xaml
    /// </summary>
    public sealed partial class ConverterInterface : UserControl
    {
        // used to determine whether we want to convert to or from inferior units
        private bool inShrekMode = true;

        private enum Props
        {
            GroupTitle,
            WatermarkText,
            Text,
            WatermarkShow,
            BtnContent,
            ResText,
            Items,
            SelVal,
            SelIdx,
            DispMemPath,
            SelValPath,
            FgClr,
            BgClr,
        }

        private static readonly Dictionary<Props, DependencyProperty> props = new Dictionary<Props, DependencyProperty>
        {
            [Props.GroupTitle] = DepPropReg<string, ConverterInterface>(nameof(GroupingTitle)),
            [Props.WatermarkText] = DepPropReg<string, ConverterInterface>(nameof(WatermarkText)),
            [Props.Text] = DepPropReg<string, ConverterInterface>(nameof(Text)),
            [Props.WatermarkShow] = DepPropReg<Visibility, ConverterInterface>(nameof(WatermarkVisibility)),
            [Props.BtnContent] = DepPropReg<object, ConverterInterface>(nameof(ButtonContent), string.Empty),
            [Props.ResText] = DepPropReg<string, ConverterInterface>(nameof(ResultText)),
            [Props.Items] = DepPropReg<IEnumerable, ConverterInterface>(nameof(ComboBoxItemSource)),
            [Props.SelVal] = DepPropReg<Unit, ConverterInterface>(nameof(SelectedValue)),
            [Props.SelIdx] = DepPropReg<int, ConverterInterface>(nameof(SelectedIndex)),
            [Props.DispMemPath] = DepPropReg<string, ConverterInterface>(nameof(DisplayMemberPath)),
            [Props.SelValPath] = DepPropReg<string, ConverterInterface>(nameof(SelectedValuePath)),
        };

        public string GroupingTitle
        {
            get =>  (string)GetValue(props[Props.GroupTitle]);
            set => SetValue(props[Props.GroupTitle], value);
        }

        public string WatermarkText
        {
            get =>  (string)GetValue(props[Props.WatermarkText]);
            set => SetValue(props[Props.WatermarkText], value);
        }

        public string Text
        {
            get => (string)GetValue(props[Props.Text]);
            set => SetValue(props[Props.Text], value);
        }

        public Visibility WatermarkVisibility
        {
            get =>  (Visibility)GetValue(props[Props.WatermarkShow]);
            set => SetValue(props[Props.WatermarkShow], value);
        }

        public object ButtonContent
        {
            get =>  GetValue(props[Props.BtnContent]);
            set => SetValue(props[Props.BtnContent], value);
        }

        public string ResultText
        {
            get =>  (string)GetValue(props[Props.ResText]);
            set => SetValue(props[Props.ResText], value);
        }

        public IEnumerable ComboBoxItemSource
        {
            get => (IEnumerable)GetValue(props[Props.Items]);
            set => SetValue(props[Props.Items], value);
        }

        public Unit SelectedValue
        {
            get => (Unit)GetValue(props[Props.SelVal]);
            set => SetValue(props[Props.SelVal], value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(props[Props.SelIdx]);
            set => SetValue(props[Props.SelIdx], value);
        }

        public string DisplayMemberPath
        {
            get => (string)GetValue(props[Props.DispMemPath]);
            set => SetValue(props[Props.DispMemPath], value);
        }

        public string SelectedValuePath
        {
            get => (string)GetValue(props[Props.SelValPath]);
            set => SetValue(props[Props.SelValPath], value);
        }

        public event TextChangedEventHandler TextChanged;

        private void OnTextChanged(object sender, TextChangedEventArgs arg)
        {
            TextChanged?.Invoke(sender, arg);

            if (!string.IsNullOrWhiteSpace(Text))
            {
                if (decimal.TryParse(Text, out var value))
                {
                    var unit = SelectedValue;

                    var res = value * unit.ValueInShreks;

                    ResultText = string.Format("{0} {1}", res, unit.CurrencySymbol);
                }
                else
                {

                }
            }
        }

        public ConverterInterface()
        {
            // this'll make the hints hide and appear again
            TextChanged += (o, e) => WatermarkVisibility = (string.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Hidden);

            InitializeComponent();
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            ResultText = Text;

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