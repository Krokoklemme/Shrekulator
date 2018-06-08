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
    using System.IO;
    using System.Runtime.CompilerServices;

    public static class Throw
    {
        internal static ArgumentNullException ArgNull(
            string param,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0) => new ArgumentNullException(param, $"{nameof(ArgumentNullException)} at {caller} ({Path.GetFileName(file)}:{line})");

        internal static ArgumentException ArgInvalid(
            string param,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0) => new ArgumentException(param, $"{nameof(ArgumentException)} at {caller} ({Path.GetFileName(file)}:{line})");

        internal static NotImplementedException NotYet(
            [CallerMemberName] string caller = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0) => new NotImplementedException($"{nameof(NotImplementedException)} at {caller} ({Path.GetFileName(file)}:{line})");
    }
}