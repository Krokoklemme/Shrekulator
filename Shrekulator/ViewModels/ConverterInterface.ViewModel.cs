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

namespace Shrekulator.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Data;

    public partial class ConverterInterface
    {
        public class ViewModel : INotifyPropertyChanged
        {
            public ViewModel(IEnumerable<Unit> units)
            {
                units = units ?? Array.Empty<Unit>();

                MeasurementUnits = new CollectionView(units);
            }

            public CollectionView MeasurementUnits { get; }

            private Unit _selectedUnit;

            public Unit SelectedUnit
            {
                get => _selectedUnit;
                set
                {
                    if (_selectedUnit != value)
                    {
                        _selectedUnit = value;
                        NotifyPropertyChanged();
                    }
                }
            }

            private void NotifyPropertyChanged([CallerMemberName] string property = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}