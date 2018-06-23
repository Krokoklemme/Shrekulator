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
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using static Helpers;

    internal static class Ex
    {
        private static class DefaultMessages
        {
            public const string ARG_NULL = "Argument was null";
            public const string ARG_INV = "Argument was invalid";
            public const string FORM_ERR = "Invalid format";
            public const string NO_FILE = "File not found";
            public const string NO_IMPL = "This method hasn't been implemented yet";
            public const string INV_OP = "Method call is not allowed in current state";
        }

        private const string Auto = "";

        internal static string ToString<T>(string message, string caller, string file, int line) where T : Exception
            => $"{nameof(T)} at {caller} ({Path.GetFileName(file)}:{line})\n\n{message}";

        // This code could be written a little DRYer, but the only solution for
        // that I can think of would be reflection and, let's be honest that'd
        // be a little overkill
        public static ArgumentNullException ArgNull(
            string param,
            string message = DefaultMessages.ARG_NULL,
            [CallerMemberName] string caller = Auto,
            [CallerFilePath] string file = Auto,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<ArgumentNullException>(message, caller, file, line);

            if (!silent)
            {
                Log(msg, notes.Prepend(param) as string[]);
            }

            return new ArgumentNullException(param, msg);
        }

        public static ArgumentException ArgInvalid(
            string param,
            string message = DefaultMessages.ARG_INV,
            [CallerMemberName] string caller = Auto,
            [CallerFilePath] string file = Auto,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<ArgumentException>(message, caller, file, line);

            if (!silent)
            {
                Log(msg, notes.Prepend(param) as string[]);
            }

            return new ArgumentException(msg, param);
        }

        public static FormatException FormErr(
            string message = DefaultMessages.FORM_ERR,
            [CallerMemberName] string caller = Auto,
            [CallerFilePath] string file = Auto,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<FormatException>(message, caller, file, line);

            if (!silent)
            {
                Log(msg, notes);
            }

            return new FormatException(msg);
        }

        public static FileNotFoundException NoFile(
            string path,
            string message = DefaultMessages.NO_FILE,
            [CallerMemberName] string caller = Auto,
            [CallerFilePath] string file = Auto,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<FileNotFoundException>(message, caller, file, line);

            if (!silent)
            {
                Log(msg, notes.Prepend(path) as string[]);
            }

            return new FileNotFoundException(msg, path);
        }

        public static NotImplementedException NoImpl(
            string message = DefaultMessages.NO_IMPL,
            [CallerMemberName] string caller = Auto,
            [CallerFilePath] string file = Auto,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<NotImplementedException>(message, caller, file, line);

            if (!silent)
            {
                Log(msg, notes);
            }

            return new NotImplementedException(msg);
        }

        public static InvalidOperationException InvOp(
            string message = DefaultMessages.INV_OP,
            [CallerMemberName] string caller = Auto,
            [CallerFilePath] string file = Auto,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<InvalidOperationException>(message, caller, file, line);

            if (!silent)
            {
                Log(msg, notes);
            }

            return new InvalidOperationException(msg);
        }
    }
}