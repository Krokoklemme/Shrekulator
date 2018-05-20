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
    public class Unit
    {
        public string DisplayName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ValueInShreks { get; set; }

        private Unit(string displayName, string currency, decimal valueInShreks)
        {
            DisplayName = displayName;
            CurrencySymbol = currency;
            ValueInShreks = valueInShreks;
        }

        public static Unit Define(string name, string currency, decimal value) => new Unit(name, currency, value);
    }
}
