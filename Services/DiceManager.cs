using Models;
using Helpers;
using UI;

namespace Services
{
    public class DiceManager
    {
        public List<Die> ParseDice(string[] args)
        {
            if (args.Length < 3)
            {
                ConsoleStyler.PrintError(Utilities.GetMessage("errors", "not_enough_dice"));
                return new List<Die>();
            }

            var dice = ValidateDice(args);

            if (dice.Count < 3)
            {
                ConsoleStyler.PrintError(Utilities.GetMessage("errors", "not_enough_dice"));
                return new List<Die>();
            }

            return dice;
        }

        public List<Die> ValidateDice(string[] args)
        {
            var dice = new List<Die>();
            foreach (var arg in args)
            {
                try
                {
                    var values = arg.Split(',').Select(int.Parse).ToArray();

                    if (values.Length != 6)
                    {
                        ConsoleStyler.PrintError(Utilities.GetMessage("errors", "dice_format"));
                        Environment.Exit(1);
                    }
                    dice.Add(new Die(values));
                }
                catch (Exception)
                {
                    ConsoleStyler.PrintError(Utilities.GetMessage("errors", "dice_format"));
                    Environment.Exit(1);
                }
            }

            return dice;
        }
    }
}