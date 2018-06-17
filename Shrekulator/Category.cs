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
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;

    public class Category
    {
        private static readonly JsonLoadSettings loadSettings = new JsonLoadSettings { LineInfoHandling = LineInfoHandling.Load, CommentHandling = CommentHandling.Ignore };

        public string Name { get; }
        public IList<Unit> Units { get; }

        public Category(string name, IList<Unit> units)
        {
            Name = name ?? throw Throw.ArgNull(nameof(name));
            Units = units ?? throw Throw.ArgNull(nameof(units));
        }

        public static Category Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Failed to find specified file", path);
            }

            var content = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(content))
            {
                content = "{}";
            }

            var cat = JsonConvert.DeserializeObject<Category>(content);

            MessageBox.Show(cat.Name);

            return default;
        }
    }
}