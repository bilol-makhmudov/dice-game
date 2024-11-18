using Models;
using Helpers;
using Services;
using UI;

namespace Core
{
    public class GameEngine
    {
        private readonly List<Die> dice;
        private readonly CryptoManager cryptoManager;
        private readonly MenuRenderer menuRenderer;
        private Die userDie = null!;
        private Die computerDie = null!;
        private bool isUserFirst;

        public GameEngine(List<Die> dice)
        {
            this.dice = dice;
            cryptoManager = new CryptoManager();
            menuRenderer = new MenuRenderer();
        }

        public void Start()
        {
            ConsoleStyler.PrintHeading(Utilities.GetMessage("game", "welcome"));
            int userResult = -1, computerResult = -1;
            menuRenderer.HelpMenu();
            isUserFirst = DetermineFirstMove();
            SelectDice(isUserFirst);

            if (isUserFirst)
            {
                userResult = UserTurn();
                computerResult = ComputerTurn();
            }
            else
            {
                computerResult = ComputerTurn();
                userResult = UserTurn();
            }
            CompareRolls(userResult, computerResult);
            ConsoleStyler.PrintHeading(Utilities.GetMessage("game", "game_over"));
        }

        private bool DetermineFirstMove()
        {
            ConsoleStyler.PrintHeading(Utilities.GetMessage("game", "determining_first_move"));
            var (hmac, key, value) = cryptoManager.GenerateHmacRandom(2);
            ConsoleStyler.PrintSubHeading($"HMAC: {hmac}");
            menuRenderer.RenderFirstMoverMenu();
            string userInput;
            int userGuess = -1;
            do
            {
                userInput = Console.ReadLine()!.Trim().ToUpper();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    ConsoleStyler.PrintError(Utilities.GetMessage("errors", "invalid_choice"));
                }
                else if (userInput == "X")
                {
                    ConsoleStyler.PrintHeading(Utilities.GetMessage("menu", "exit"));
                    Environment.Exit(0);
                }
                else if (userInput == "?")
                {
                    menuRenderer.HelpMenu();
                    continue;
                }
                if (!int.TryParse(userInput, out userGuess) || (userGuess != 0 && userGuess != 1))
                {
                    ConsoleStyler.PrintError(Utilities.GetMessage("errors", "invalid_choice"));
                }
            } while (userGuess != 0 && userGuess != 1);
            ConsoleStyler.PrintSubHeading($"Actual Value: {value}, Key: {Convert.ToHexString(key)}");
            bool userChoosesFirst = userGuess == value;
            ConsoleStyler.PrintNormal(userChoosesFirst
                ? Utilities.GetMessage("game", "user_first_move")
                : Utilities.GetMessage("game", "computer_first_move"));
            return userChoosesFirst;
        }

        private void SelectDice(bool userChoosesFirst)
        {
            if (userChoosesFirst)
            {
                ConsoleStyler.PrintPrompt(Utilities.GetMessage("game", "choose_dice"));
                menuRenderer.DiceOptions(dice);
                ConsoleStyler.PrintPrompt(Utilities.GetMessage("game", "choose_dice_prompt"));
                string userInput;
                int userChoice;
                while (true)
                {
                    userInput = Console.ReadLine()?.Trim().ToUpper() ?? "";
                    if (userInput == "?")
                    {
                        menuRenderer.RenderProbabilitiesTable(dice);
                        ConsoleStyler.PrintPrompt(Utilities.GetMessage("game", "choose_dice_prompt"));
                    }
                    else if (int.TryParse(userInput, out userChoice) && userChoice >= 0 && userChoice < dice.Count)
                    {
                        break;
                    }
                    else
                    {
                        ConsoleStyler.PrintError(Utilities.GetMessage("errors", "invalid_choice"));
                    }
                }
                userDie = dice[userChoice];
                dice.RemoveAt(userChoice);
                int computerChoice = cryptoManager.GenerateRandom(dice.Count);
                computerDie = dice[computerChoice];
            }
            else
            {
                int computerChoice = cryptoManager.GenerateRandom(dice.Count);
                computerDie = dice[computerChoice];
                ConsoleStyler.PrintPrompt($"{Utilities.GetMessage("game", "computer_dice")}: " + computerDie);
                dice.RemoveAt(computerChoice);

                ConsoleStyler.PrintPrompt(Utilities.GetMessage("game", "choose_dice"));
                menuRenderer.DiceOptions(dice);
                ConsoleStyler.PrintPrompt(Utilities.GetMessage("game", "choose_dice_prompt"));

                string userInput;
                int userChoice;
                while (true)
                {
                    userInput = Console.ReadLine()?.Trim().ToUpper() ?? "";

                    if (userInput == "?")
                    {
                        menuRenderer.RenderProbabilitiesTable(dice);
                        ConsoleStyler.PrintPrompt(Utilities.GetMessage("game", "choose_dice_prompt"));
                    }
                    else if (int.TryParse(userInput, out userChoice) && userChoice >= 0 && userChoice < dice.Count)
                    {
                        break;
                    }
                    else
                    {
                        ConsoleStyler.PrintError(Utilities.GetMessage("errors", "invalid_choice"));
                    }
                }
                userDie = dice[userChoice];
            }
            ConsoleStyler.PrintNormal($"{Utilities.GetMessage("game", "user_dice")}: {userDie}");
            ConsoleStyler.PrintNormal($"{Utilities.GetMessage("game", "computer_dice")}: {computerDie}");
        }

        private int UserTurn()
        {
            var (hmac, key, value) = cryptoManager.GenerateHmacRandom(6);
            ConsoleStyler.PrintPrompt("I choose a random number between 0-5");
            ConsoleStyler.PrintSubHeading($"HMAC: {hmac}");

            ConsoleStyler.PrintHeading(Utilities.GetMessage("game", "user_turn"));
            int userThrow = GetPlayerThrow();
            int userResult = userDie.Values[(userThrow + value) % 6];
            ConsoleStyler.PrintNormal($"Computer's random number: {value}, Key: {Convert.ToHexString(key)}");
            ConsoleStyler.PrintNormal($"{Utilities.GetMessage("game", "user_roll")}: {userResult} \n(({value} + {userThrow})%6)");
            
            return userResult;
        }

        private int ComputerTurn()
        {
            ConsoleStyler.PrintHeading(Utilities.GetMessage("game", "computer_turn"));
            var (hmac, key, value) = cryptoManager.GenerateHmacRandom(6);
            ConsoleStyler.PrintSubHeading($"HMAC: {hmac}");
            ConsoleStyler.PrintPrompt(Utilities.GetMessage("game", "add_user_value"));
            string userInput;       
            int userValue;
            while (true)
            {
                userInput = Console.ReadLine()!.Trim().ToUpper();
                if (userInput == "X")
                {
                    ConsoleStyler.PrintHeading(Utilities.GetMessage("menu", "exit"));
                    Environment.Exit(0);
                }
                else if (userInput == "?")
                {
                    menuRenderer.HelpMenu();
                    continue;
                }
                if (int.TryParse(userInput, out userValue) && userValue >= 0 && userValue < 6)
                {
                    break;
                }
                ConsoleStyler.PrintError(Utilities.GetMessage("errors", "invalid_choice"));
            }

            int computerResult = computerDie.Values[(value + userValue) % 6];
            ConsoleStyler.PrintSubHeading($"My random selection value between 0-5: {value}, Key: {Convert.ToHexString(key)}");
            ConsoleStyler.PrintNormal($"{Utilities.GetMessage("game", "computer_roll")}: {computerResult} \n(({value} + {userValue})%6)");

            return computerResult;
        }

        private int GetPlayerThrow()
        {
            ConsoleStyler.PrintPrompt(Utilities.GetMessage("game", "choose_throw"));
            int choice;

            while (!int.TryParse(Console.ReadLine()?.Trim(), out choice) || choice < 0 || choice >= 6)
            {
                ConsoleStyler.PrintError(Utilities.GetMessage("errors", "invalid_choice"));
            }
            return choice;
        }

        private void CompareRolls(int userResult, int computerResult)
        {
            ConsoleStyler.PrintNormal(Utilities.GetMessage("game", "user_roll") + userResult);
            ConsoleStyler.PrintNormal(Utilities.GetMessage("game", "computer_roll") + computerResult);

            if (userResult > computerResult)
            {
                ConsoleStyler.PrintSuccess(Utilities.GetMessage("game", "user_wins_round"));
            }
            else if (userResult < computerResult)
            {
                ConsoleStyler.PrintNormal(Utilities.GetMessage("game", "computer_wins_round"));
            }
            else
            {
                ConsoleStyler.PrintNormal(Utilities.GetMessage("game", "round_tie"));
            }
        }
    }
}