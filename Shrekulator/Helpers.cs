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
    using Shrekulator.Container;
    using System;
    using System.Windows;
    using static Helpers.ShrekUnitType;

    internal static class Helpers
    {
        public static DependencyProperty DepPropReg<Type, Target>(string name) => DependencyProperty.Register(name, typeof(Type), typeof(Target), new PropertyMetadata(default(Type)));

        public static DependencyProperty DepPropReg<Type, Target>(string name, object defaultValue) => DependencyProperty.Register(name, typeof(Type), typeof(Target), new PropertyMetadata(defaultValue));

        private static object _lockObject;

        public enum ShrekUnitType
        {
            Money,
            Time,
            Distance,
            Weight,
        }

        public static TrackableMap<ShrekUnitType, decimal> ShrekMap { get; }

        static Helpers()
        {
            _lockObject = new object();

            ShrekMap = new TrackableMap<ShrekUnitType, decimal>
            {
                [Money] = 14.99m, // in USD
                [Time] = TimeSpan.FromMinutes(95.0).Minutes,
                [Distance] = 10.0m, // in feet
                [Weight] = 650.0m, // in pounds
            };
        }
    }
}