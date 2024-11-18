using Helpers;
using Services;
using Core;
using UI;

class Program
{
    static void Main(string[] args)
    {
        ConsoleStyler.SetTerminalBackground(ConsoleColor.Black);
        if (args.Length < 3)
        {
            ConsoleStyler.PrintError(Utilities.GetMessage("errors", "not_enough_dice"));
            ConsoleStyler.PrintNormal(Utilities.GetMessage("errors", "usage"));
            ConsoleStyler.PrintNormal(Utilities.GetMessage("errors", "dice_format"));
            return;
        }
        try
        {
            var diceManager = new DiceManager();
            var dice = diceManager.ParseDice(args);
            while(true){
                var gameEngine = new GameEngine(dice);
                gameEngine.Start();
                ConsoleStyler.PrintPrompt("Do you want to play another round? (Y/N): ");
                string input = Console.ReadLine()?.Trim().ToUpper() ?? "N";
                if (input.ToLower() == "n" || input.ToLower() == "no")
                {
                    ConsoleStyler.PrintNormal("Thank you for playing! Goodbye!");
                    break;
                }
                else if (input.ToLower() != "y" || input.ToLower() == "yes")
                {
                    ConsoleStyler.PrintError("Invalid input. Exiting the game.");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleStyler.PrintError($"{Utilities.GetMessage("errors", "unexpected_error")}: {ex.Message}");
        }
    }
}