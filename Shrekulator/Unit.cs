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
    public struct Unit
    {
        public string Name { get; }

        public string Symbol { get; }

        public decimal Value { get; }

        public string Format { get; }

        public string ToString(decimal amount) => string.Format(Format, amount, Name, Symbol);

        public decimal Convert(decimal amount, Unit target) => throw Ex.NoImpl();

        public Unit(string name, string symbol, decimal value, string format)
        {
            Name = name ?? throw Ex.ArgNull(nameof(name));
            Symbol = symbol ?? throw Ex.ArgNull(nameof(Symbol));
            Value = value >= decimal.Zero ? value : throw Ex.ArgInvalid(nameof(value), "Unit value may not be negative");
            Format = format ?? throw Ex.ArgNull(nameof(format));
        }
    }
}