public static class Logger
{
    public static event System.Action<string> OnLog;
    public static void Log(string message) => OnLog?.Invoke(message);
}
