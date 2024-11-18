namespace UI
{
    public class ConsoleStyler
    {
        private static readonly ConsoleColor BackgroundColor = ConsoleColor.Black;
        private static readonly ConsoleColor TextColor = ConsoleColor.Gray;
        private static readonly ConsoleColor HeadingColor = ConsoleColor.Green;
        private static readonly ConsoleColor SubHeadingColor = ConsoleColor.Cyan;
        private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
        private static readonly ConsoleColor SuccessColor = ConsoleColor.Green;
        private static readonly ConsoleColor PromptColor = ConsoleColor.Magenta;

        public static void SetTerminalBackground(ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.Clear();
        }

        public static void PrintMessage(string message, ConsoleColor foreground, ConsoleColor background = ConsoleColor.Black)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintHeading(string message)
        {
            PrintMessage($"=== {message} ===", HeadingColor, BackgroundColor);
        }

        public static void PrintSubHeading(string message)
        {
            PrintMessage(message, SubHeadingColor, BackgroundColor);
        }

        public static void PrintError(string message)
        {
            PrintMessage($"Error: {message}", ErrorColor, BackgroundColor);
        }

        public static void PrintSuccess(string message)
        {
            PrintMessage($"Success: {message}", SuccessColor, BackgroundColor);
        }

        public static void PrintPrompt(string message)
        {
            PrintMessage(message, PromptColor, BackgroundColor);
        }

        public static void PrintNormal(string message)
        {
            PrintMessage(message, TextColor, BackgroundColor);
        }
    }
}