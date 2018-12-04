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
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

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

        internal static string ToString<T>(string message, string caller, string file, int line) where T : Exception
            => $"{nameof(T)} at {caller} ({Path.GetFileName(file)}:{line})\n\n{message}";

        // This code could be written a little DRYer, but the only solution for
        // that I can think of would be reflection and, let's be honest that'd
        // be a little overkill
        public static ArgumentNullException ArgNull(
            string param,
            string message = DefaultMessages.ARG_NULL,
            [CallerMemberName] string caller = Helpers.Empty,
            [CallerFilePath] string file = Helpers.Empty,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<ArgumentNullException>(message, caller, file, line);

            if (!silent)
            {
                Helpers.Log(msg, notes);
            }

            return new ArgumentNullException(param, msg);
        }

        public static ArgumentException ArgInvalid(
            string param,
            string message = DefaultMessages.ARG_INV,
            [CallerMemberName] string caller = Helpers.Empty,
            [CallerFilePath] string file = Helpers.Empty,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<ArgumentException>(message, caller, file, line);

            if (!silent)
            {
                Helpers.Log(msg, notes);
            }

            return new ArgumentException(msg, param);
        }

        public static FormatException FormErr(
            string message = DefaultMessages.FORM_ERR,
            [CallerMemberName] string caller = Helpers.Empty,
            [CallerFilePath] string file = Helpers.Empty,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<FormatException>(message, caller, file, line);

            if (!silent)
            {
                Helpers.Log(msg, notes);
            }

            return new FormatException(msg);
        }

        public static FileNotFoundException NoFile(
            string path,
            string message = DefaultMessages.NO_FILE,
            [CallerMemberName] string caller = Helpers.Empty,
            [CallerFilePath] string file = Helpers.Empty,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<FileNotFoundException>(message, caller, file, line);

            if (!silent)
            {
                Helpers.Log(msg, notes);
            }

            return new FileNotFoundException(msg, path);
        }

        public static NotImplementedException NoImpl(
            string message = DefaultMessages.NO_IMPL,
            [CallerMemberName] string caller = Helpers.Empty,
            [CallerFilePath] string file = Helpers.Empty,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<NotImplementedException>(message, caller, file, line);

            if (!silent)
            {
                Helpers.Log(msg, notes);
            }

            return new NotImplementedException(msg);
        }

        public static InvalidOperationException InvOp(
            string message = DefaultMessages.INV_OP,
            [CallerMemberName] string caller = Helpers.Empty,
            [CallerFilePath] string file = Helpers.Empty,
            [CallerLineNumber] int line = 0,
            bool silent = false,
            params string[] notes)
        {
            var msg = ToString<InvalidOperationException>(message, caller, file, line);

            if (!silent)
            {
                Helpers.Log(msg, notes);
            }

            return new InvalidOperationException(msg);
        }
    }
}
