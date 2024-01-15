using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace aplikacja_dziekanat
{
    public static class DebugService
    {
        public static void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }

        public static void WriteLine(string className, string methodName, string message)
        {
            Debug.WriteLine($"X: {className} at {methodName} method - {message}");
        }
    }
}
