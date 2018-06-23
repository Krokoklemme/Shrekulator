// This is free and unencumbered software released into the public domain.
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a
// compiled binary, for any purpose, commercial or non-commercial,
// and by any means.
// In jurisdictions that recognize copyright laws, the author or
// authors of this software dedicate any and all copyright interest
// in the software to the public domain. We make this dedication for
// the benefit of the public at large and to the detriment of our
// heirs and successors. We intend this dedication to be an overt act
// of relinquishment in perpetuity of all present and future rights to
// this software under copyright law.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// For more information, please refer to <https://unlicense.org>

namespace Shrekulator
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Category
    {
        public string Name { get; }
        public IList<Unit> Units { get; }

        public Category(string name, IList<Unit> units)
        {
            Name = name ?? throw Ex.ArgNull(nameof(name));
            Units = units ?? throw Ex.ArgNull(nameof(units));
        }

        public static Category Load(string path)
        {
            if (!File.Exists(path))
            {
                throw Ex.NoFile(path, "Specified definition file not found");
            }

            var definitionTree = JObject.Parse(File.ReadAllText(path));

            if (!definitionTree.HasValues)
            {
                return new Category(Path.GetFileNameWithoutExtension(path).SanitizeCasing(), new List<Unit>());
            }
            else
            {
                if (definitionTree.Count <= 2 &&
                    definitionTree.TryGetValue("Units", StringComparison.InvariantCulture, out var unitNode) &&
                    unitNode is JArray unitArray)
                {
                    var unitList = new List<Unit>(unitArray.Count);

                    var name = string.Empty;

                    if (definitionTree.Count == 2 &&
                        definitionTree.TryGetValue("Name", StringComparison.InvariantCulture, out var nameNode) && nameNode.Type == JTokenType.String)
                    {
                        name = (string)nameNode;
                    }
                    else
                    {
                        name = Path.GetFileNameWithoutExtension(path).SanitizeCasing();
                    }

                    foreach (JObject unit in unitArray)
                    {
                        if (unit.Count == 4 &&
                            unit.TryGetValue("Name", StringComparison.InvariantCulture, out var nameToken) && nameToken.Type == JTokenType.String &&
                            unit.TryGetValue("Symbol", StringComparison.InvariantCulture, out var symbolToken) && symbolToken.Type == JTokenType.String &&
                            unit.TryGetValue("Value", StringComparison.InvariantCulture, out var valueToken) && valueToken.Type == JTokenType.Float &&
                            unit.TryGetValue("Format", StringComparison.InvariantCulture, out var formatToken) && formatToken.Type == JTokenType.String)
                        {
                            unitList.Add(
                                new Unit(
                                    name: (string)nameToken,
                                    symbol: (string)symbolToken,
                                    value: (decimal)valueToken,
                                    format: (string)formatToken
                                ));
                        }
                        else
                        {
                            throw Ex.FormErr($"{name} - Invalid format at index {unitArray.IndexOf(unit)}",
                                notes: "Check out 'ModdingReadme.txt' for more information");
                        }
                    }

                    return new Category(name, unitList);
                }
                else
                {
                    throw Ex.FormErr("Category files must consist of a 'Units'-array + optional 'Name'-property");
                }
            }
        }
    }
}