using System.Runtime.InteropServices;

namespace AstolfoBot
{
    public static class OnExit
    {
        public static EventHandler<int>? onExit;
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
        static ConsoleEventDelegate? handler;   // Keeps it from getting garbage collected
                                                // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    }
}