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
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string Root = AppDomain.CurrentDomain.BaseDirectory;

        private IReadOnlyList<string> _quotes = default;

        public IReadOnlyList<string> GetQuotes()
        {
            if (_quotes == null)
            {
                // I still gotta figure out where I could store
                // those, so it can download them if necessary
                var quoteFilePath = Path.Combine(Root, "quotes.txt");
                IList<string> lines = default;

                if (File.Exists(quoteFilePath))
                {
                    lines = File.ReadAllLines(quoteFilePath, Encoding.UTF8);

                    for (int i = 0; i < lines.Count; i++)
                    {
                        lines[i] = lines[i].Trim();
                    }
                }

                _quotes = new ReadOnlyCollection<string>(lines ?? new[] { "<insert deleted quotes here>" });
            }

            return _quotes;
        }

        private IReadOnlyDictionary<string, IReadOnlyList<string>> _convTables = default;

        public IReadOnlyDictionary<string, IReadOnlyList<string>> GetConversionTables()
        {
            if (_convTables == null)
            {
                var categoryDefinitions = Directory.EnumerateFiles(Root, "*.udef", SearchOption.TopDirectoryOnly);
                var tableBuffer = new Dictionary<string, IReadOnlyList<string>>(categoryDefinitions.Count());
                var splitChars = new[] { '=' };

                foreach (var definitionFile in categoryDefinitions)
                {
                    var data = File.ReadAllLines(definitionFile, Encoding.UTF8);

                    if (data.Length > 0)
                    {
                        var firstLine = data[0].Trim();
                        var categoryName = definitionFile;

                        if (firstLine.StartsWith("CategoryName"))
                        {
                            var categoryNameData = firstLine.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);


                        }
                    }
                }
            }

            return _convTables;
        }
    }
}
