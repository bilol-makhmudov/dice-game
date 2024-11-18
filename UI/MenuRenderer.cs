using Models;
using Spectre.Console;
using Helpers;
using UI;
namespace Services
{
    public class MenuRenderer
    {
        public void DiceOptions(List<Die> dice)
        {
            var table = CreateStyledTable("Option", "Dice Configuration");

            for (int i = 0; i < dice.Count; i++)
            {
                table.AddRow($"[bold yellow]{i}[/]", $"[cyan]{dice[i]}[/]");
            }

            AnsiConsole.MarkupLine("[bold green]=== Choose Your Dice ===[/]");
            AnsiConsole.Write(table);
        }

        public void RenderProbabilitiesTable(List<Die> dice)
        {
            ConsoleStyler.PrintHeading("=== Winning chance of each die ===");

            var table = new Table();
            table.AddColumn("Your Dice \\ Opponent Dice");
            for (int i = 0; i < dice.Count; i++)
            {
                table.AddColumn(dice[i].ToString());
            }
            for (int i = 0; i < dice.Count; i++)
            {
                var row = new List<string> {dice[i].ToString()};
                for (int j = 0; j < dice.Count; j++)
                {
                    double probability = Utilities.CalculateWinProbability(dice[i], dice[j]);
                    row.Add(probability.ToString("P2"));
                }
                table.AddRow(row.ToArray());
            }
            AnsiConsole.Write(table);
        }

        public void Results(string message)
        {
            AnsiConsole.MarkupLine($"[bold green]{message}[/]");
        }

        public void HelpMenu()
        {
            var table = CreateStyledTable("Command", "Description");
            table.AddRow("[bold yellow]?[/]", "[cyan]Display this help menu.[/]");
            table.AddRow("[bold yellow]X[/]", "[cyan]Exit the game.[/]");
            table.AddRow("[bold yellow]0-5[/]", "[cyan]Choose a number for your dice throw (during gameplay).[/]");
            table.AddRow("[bold yellow]0, 1[/]", "[cyan]Guess the number during the first move determination.[/]");

            AnsiConsole.MarkupLine("[bold white]=== Help Menu ===[/]");
            AnsiConsole.Write(table);

            AnsiConsole.MarkupLine("[bold green]Game Rules:[/]");
            AnsiConsole.MarkupLine("[cyan]1.[/] The computer and user take turns selecting dice and rolling them.");
            AnsiConsole.MarkupLine("[cyan]2.[/] The first move is determined by a cryptographic proof-based random number.");
            AnsiConsole.MarkupLine("[cyan]3.[/] The player with the higher roll wins the round.");
            AnsiConsole.MarkupLine("[cyan]4.[/] The game continues until you exit.");
        }

        public void RenderFirstMoverMenu()
        {
            var table = CreateStyledTable("Command", "Description");
            table.AddRow("[bold yellow]0[/]", "[cyan]0[/]");
            table.AddRow("[bold yellow]1[/]", "[cyan]1[/]");
            table.AddRow("[bold yellow]X[/]", "[cyan]Exit[/]");
            table.AddRow("[bold yellow]?[/]", "[cyan]Help[/]");

            AnsiConsole.MarkupLine("[bold yellow]=== First Mover Menu ===[/]");
            AnsiConsole.Write(table);
        }
        private Table CreateStyledTable(params string[] columnHeaders)
        {
            var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey);
            foreach (var header in columnHeaders)
            {
                table.AddColumn(new TableColumn(header).LeftAligned());
            }
            return table;
        }
    }
}