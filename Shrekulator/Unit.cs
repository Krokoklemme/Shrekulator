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
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public struct Unit
    {
        public static IList<IList<Unit>> LookupTables { get; } = new List<IList<Unit>>();

        public const string PrefixSymbol = "{1}{2}";
        public const string PostfixSymbol = "{2}{1}";
        public const string FullName = "{2} {0}";
        public const string ValueOnly = "{2}";
        public const string CommaDash = "{2},-";

        public string DisplayName { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        public string Symbol { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        public decimal OneUnitInShreks => 1m / OneShrekInUnits;
        public decimal OneShrekInUnits { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }
        public string FormatString { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Format(decimal value) => string.Format(FormatString, DisplayName, Symbol, value);

        public decimal Convert(decimal value, Unit targetUnit) => decimal.Zero;

        public Unit(string displayName, string symbol, decimal value, string format = PostfixSymbol)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                Throw.ArgInvalid(nameof(displayName));
            }

            if (string.IsNullOrWhiteSpace(symbol))
            {
                Throw.ArgInvalid(nameof(symbol));
            }

            if (value <= 0m)
            {
                Throw.ArgInvalid(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(format))
            {
                Throw.ArgInvalid(nameof(format));
            }

            DisplayName = displayName;
            Symbol = symbol;
            OneShrekInUnits = value;
            FormatString = format;
        }
    }
}