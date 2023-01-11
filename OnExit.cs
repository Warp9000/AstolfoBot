using System.Runtime.InteropServices;

namespace AstolfoBot
{
    public static partial class OnExit
    {
        public static readonly EventHandler<int>? onExit;

        public static void Start()
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
        }

        static bool ConsoleEventCallback(int eventType)
        {
            onExit?.Invoke(null, eventType);
            return false;
        }

        static ConsoleEventDelegate? handler; // Keeps it from getting garbage collected

        // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);

        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool
        SetConsoleCtrlHandler(
            ConsoleEventDelegate callback,
            [MarshalAs(UnmanagedType.Bool)] bool add
        );
    }
}
