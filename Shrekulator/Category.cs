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
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;

    public class Category
    {
        /// <summary>
        /// Represents a unit of a <see cref="Category"/>
        /// </summary>
        public struct Unit
        {
            /// <summary>
            /// Display name of the <see cref="Unit"/>
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Scientific symbol, intended for user-display
            /// </summary>
            public string Symbol { get; }

            /// <summary>
            /// Value of a single Shrek
            /// </summary>
            public decimal Value { get; }

            /// <summary>
            /// Format string to be used in <see cref="ToString(decimal)"/>
            /// </summary>
            public string Format { get; }

            /// <summary>
            /// Formats <paramref name="amount"/> according to <see cref="Format"/>
            /// </summary>
            /// <param name="amount">The amount to format</param>
            /// <returns><paramref name="amount"/> stringified according to <see cref="Format"/></returns>
            public string ToString(decimal amount) => string.Format(Format, amount, Name, Symbol);

            /// <summary>
            /// Converts from and to Shrek's
            /// </summary>
            /// <param name="amount">Value to convert</param>
            /// <param name="shrekmode">Indicates, whether it should convert from or to Shrek's</param>
            /// <returns>The converted value</returns>
            public decimal Convert(decimal amount, bool shrekmode = true)
            {
                if (!shrekmode)
                {
                    return amount * Value;
                }
                else
                {
                    var value = 1m / Value;
                    return amount * value;
                }
            }

            /// <summary>
            /// Constructs a new <see cref="Unit"/> object
            /// </summary>
            /// <param name="name">The <see cref="Name"/> of the unit</param>
            /// <param name="symbol">The <see cref="Symbol"/> of the unit</param>
            /// <param name="value">The <see cref="Value"/> of the unit</param>
            /// <param name="format">The <see cref="Format"/> of the unit</param>
            public Unit(string name, string symbol, decimal value, string format)
            {
                Name = name ?? throw Ex.ArgNull(nameof(name));
                Symbol = symbol ?? throw Ex.ArgNull(nameof(Symbol));
                Value = value >= decimal.Zero ? value : throw Ex.ArgInvalid(nameof(value), "Unit value may not be negative");
                Format = format ?? throw Ex.ArgNull(nameof(format));
            }
        }

        /// <summary>
        /// The name of the category
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Units, that belong to this category
        /// </summary>
        public IReadOnlyList<Unit> Units { get; }

        /// <summary>
        /// Instantiates a new <see cref="Category" /> object with the specified name and units
        /// </summary>
        /// <param name="name">The name of the category to be created</param>
        /// <param name="units">Units that belong to this category</param>
        /// <exception cref="ArgumentNullException">Thrown, when either <paramref name="name"/> or <paramref name="units"/> is <c>null</c></exception>
        public Category(string name, IReadOnlyList<Unit> units)
        {
            Name = name ?? throw Ex.ArgNull(nameof(name));
            Units = units ?? throw Ex.ArgNull(nameof(units));
        }

        /// <summary>
        /// Converts the given JSON string into a <see cref="Category"/> object
        /// </summary>
        /// <param name="data">The string to parse</param>
        /// <returns>A new <see cref="Category"/> object constructed from <paramref name="data"/></returns>
        /// <exception cref="ArgumentNullException">Thrown, when <paramref name="data"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">Thrown, when <paramref name="data"/> is empty</exception>
        /// <exception cref="JsonException">Thrown, when <paramref name="data"/> contains invalid JSON</exception>
        /// <exception cref="FormatException">Thrown, when <paramref name="data"/> doesn't contain a valid category definition</exception>
        public static Category Parse(string data)
        {
            if (data == null) throw Ex.ArgNull(nameof(data));
            if (string.IsNullOrWhiteSpace(data)) throw Ex.ArgInvalid(nameof(data), "String was empty");

            var definitionTree = JObject.Parse(data);

            if (definitionTree.Count == 2 &&
                definitionTree.TryGetValue(nameof(Category.Name), StringComparison.InvariantCulture, out var nameToken) &&
                definitionTree.TryGetValue(nameof(Category.Units), StringComparison.InvariantCulture, out var unitsToken) &&
                nameToken.Type == JTokenType.String &&
                unitsToken is JArray unitArray)
            {
                var name = (string)nameToken;
                var units = new List<Unit>(unitArray.Count);

                foreach (JObject item in unitArray)
                {
                    if (item.Count == 4 &&
                        item.TryGetValue(nameof(Unit.Name), StringComparison.InvariantCulture, out var itemName) &&
                        item.TryGetValue(nameof(Unit.Symbol), StringComparison.InvariantCulture, out var itemSymbol) &&
                        item.TryGetValue(nameof(Unit.Value), StringComparison.InvariantCulture, out var itemValue) &&
                        item.TryGetValue(nameof(Unit.Format), StringComparison.InvariantCulture, out var itemFormat) &&
                        itemName.Type == JTokenType.String &&
                        itemSymbol.Type == JTokenType.String &&
                        (itemValue.Type == JTokenType.Float || itemValue.Type == JTokenType.Integer) &&
                        itemFormat.Type == JTokenType.String)
                    {
                        units.Add(new Unit(
                                (string)itemName,
                                (string)itemSymbol,
                                (decimal)itemValue,
                                (string)itemFormat
                            ));
                    }
                    else
                    {
                        throw Ex.FormErr(notes: new[] {
                            "Unit must consist of four fields",
                            $"{nameof(Unit.Name)}: String",
                            $"{nameof(Unit.Symbol)}: String",
                            $"{nameof(Unit.Value)}: Number",
                            $"{nameof(Unit.Format)}: String",
                        });
                    }
                }

                return new Category(name, units);
            }

            throw Ex.FormErr(notes: new[] {
                "Definition file must consist of two fields",
                $"{nameof(Name)}: String",
                $"{nameof(Units)}: Array",
            });
        }
    }
}
