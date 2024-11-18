using System.Text.Json;
using Models;
using UI;

namespace Helpers
{
    public static class Utilities
    {
        private static Dictionary<string, Dictionary<string, string>> messages;

        static Utilities()
        {
            var path = "Helpers/messages.json";
            if (!File.Exists(path))
            {
                ConsoleStyler.PrintError("Error: messages.json file is missing.");
                Environment.Exit(1);
            }

            try
            {
                var json = File.ReadAllText(path);
                messages = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

                if (messages == null)
                {
                    ConsoleStyler.PrintError("Error: Failed to parse messages.json.");
                    messages = new Dictionary<string, Dictionary<string, string>>();
                }
            }
            catch (Exception ex)
            {
                ConsoleStyler.PrintError($"Error loading messages.json: {ex.Message}");
                messages = new Dictionary<string, Dictionary<string, string>>();
            }
        }

        public static string GetMessage(string category, string key, params object[] args)
        {
            if (messages.ContainsKey(category) && messages[category].ContainsKey(key))
            {
                var message = messages[category][key];
                return args.Length > 0 ? string.Format(message, args) : message;
            }
            ConsoleStyler.PrintError($"Warning: Message not found for key: {category}.{key}");
            return $"[Missing Message: {category}.{key}]";
        }

        public static double CalculateWinProbability(Die dieA, Die dieB)
        {
            int totalRounds = 1000;
            int dieAWins = 0;

            for (int i = 0; i < totalRounds; i++)
            {
                int rollA = dieA.Values[new Random().Next(6)];
                int rollB = dieB.Values[new Random().Next(6)];
                if (rollA > rollB) dieAWins++;
            }

            return (double)dieAWins / totalRounds;
        }
    }
}