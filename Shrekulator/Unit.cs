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

    public struct Unit
    {
        public string DisplayName { get; }

        public string ValueSymbol { get; }

        public decimal ValueInShreks { get; }

        public string FormatString { get; }

        public string Format(decimal amount) => string.Format(FormatString, amount, DisplayName, ValueSymbol);

        public decimal Convert(decimal amount, Unit target)
        {
            throw Throw.NotYet();
        }

        public Unit(string displayName, string valueSymbol, decimal valueInShreks, string formatString)
        {
            DisplayName = displayName ?? throw Throw.ArgNull(nameof(displayName));
            ValueSymbol = valueSymbol ?? throw Throw.ArgNull(nameof(ValueSymbol));
            ValueInShreks = valueInShreks;
            FormatString = formatString ?? throw Throw.ArgNull(nameof(formatString));
        }
    }
}