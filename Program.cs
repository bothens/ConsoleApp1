using Spectre.Console;

namespace ConsoleApp1
{
    class Program
{
    static void Main()
    {
        // Startmeddelande
        AnsiConsole.MarkupLine("[bold yellow]Välkommen till Hangman![/]");
        AnsiConsole.MarkupLine("[italic grey]Gissa ordet innan hängningen är komplett![/]");

        // Spelets variabler
        string[] ordlista = { "programmering", "spel", "dator", "visualstudio", "console" };
        var slump = new Random();
        string hemligtOrd = ordlista[slump.Next(ordlista.Length)];
        HashSet<char> gissadeBokstäver = new HashSet<char>();
        int felaktigaGissningar = 0;
        int maxFel = 6;

        // Spel-loop
        while (true)
        {
            // Visa gallgen
            VisaGallgen(felaktigaGissningar);

            // Visa ordet
            AnsiConsole.MarkupLine("[bold cyan]Ord:[/] " + VisaOrd(hemligtOrd, gissadeBokstäver));

            // Kontrollera om spelaren har vunnit
            if (AllaBokstäverGissade(hemligtOrd, gissadeBokstäver))
            {
                AnsiConsole.MarkupLine("[bold green]Grattis! Du vann![/]");
                break;
            }

            // Kontrollera om spelaren har förlorat
            if (felaktigaGissningar >= maxFel)
            {
                AnsiConsole.MarkupLine($"[bold red]Du förlorade! Ordet var: {hemligtOrd}[/]");
                break;
            }

            // Fråga användaren om en gissning
            char gissning = AnsiConsole.Ask<char>("[bold yellow]Gissa en bokstav:[/] ");

            // Kontrollera om bokstaven redan är gissad
            if (gissadeBokstäver.Contains(gissning))
            {
                AnsiConsole.MarkupLine("[bold red]Du har redan gissat den bokstaven![/]");
                continue;
            }

            // Lägg till gissningen till mängden gissade bokstäver
            gissadeBokstäver.Add(gissning);

            // Kontrollera om gissningen är korrekt
            if (!hemligtOrd.Contains(gissning))
            {
                felaktigaGissningar++;
                AnsiConsole.MarkupLine("[bold red]Fel gissning![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold green]Rätt gissning![/]");
            }
        }
    }

    static void VisaGallgen(int felaktigaGissningar)
    {
        string[] gallge = {
            "  +---+  ",
            "  |   |  ",
            "      |  ",
            "      |  ",
            "      |  ",
            "      |  ",
            "========="
        };

        if (felaktigaGissningar > 0) gallge[2] = "  O   |  ";
        if (felaktigaGissningar > 1) gallge[3] = "  |   |  ";
        if (felaktigaGissningar > 2) gallge[3] = " /|   |  ";
        if (felaktigaGissningar > 3) gallge[3] = " /|\\  |  ";
        if (felaktigaGissningar > 4) gallge[4] = " /    |  ";
        if (felaktigaGissningar > 5) gallge[4] = " / \\  |  ";

        foreach (string rad in gallge)
        {
            AnsiConsole.MarkupLine($"[grey]{rad}[/]");
        }
    }

    static string VisaOrd(string hemligtOrd, HashSet<char> gissadeBokstäver)
    {
        var visatOrd = "";
        foreach (char bokstav in hemligtOrd)
        {
            visatOrd += gissadeBokstäver.Contains(bokstav) ? $"{bokstav} " : "_ ";
        }
        return visatOrd.Trim();
    }

    static bool AllaBokstäverGissade(string hemligtOrd, HashSet<char> gissadeBokstäver)
    {
        foreach (char bokstav in hemligtOrd)
        {
            if (!gissadeBokstäver.Contains(bokstav))
            {
                return false;
            }
        }
        return true;
    }
}
}