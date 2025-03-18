using System;
using System.Collections.Generic;
using Spectre.Console;

class Program
{
    static void Main()
    {
        Console.WriteLine("Spelet startar..."); // Debug-meddelande
        HangmanGame spel = new HangmanGame();
        spel.Starta();
    }
}

// Observer Interface
interface IObserver
{
    void Uppdatera(string meddelande);
}

// Hangman-spelet med Observer Pattern
class HangmanGame
{
    private string hemligtOrd;
    private HashSet<char> gissadeBokstäver = new HashSet<char>();
    private int felaktigaGissningar = 0;
    private const int maxFel = 6;
    private List<IObserver> observatörer = new List<IObserver>();

    public HangmanGame()
    {
        // Skapa en lista med ord och blanda dem
        string[] ordlista = {
            "programmering", "spel", "dator", "visualstudio", "console",
            "banan", "skola", "lärare", "elefant", "giraff",
            "konsert", "guitar", "solsken", "måne", "stjärna"
        };
        FisherYatesShuffle(ordlista);
        hemligtOrd = ordlista[0];

        // Lägg till observatörer
        LäggTillObservatör(new Gallgen());
        LäggTillObservatör(new Ordpanel());
    }

    public void Starta()
    {
        AnsiConsole.MarkupLine("[bold yellow]Välkommen till Hangman![/]");
        while (felaktigaGissningar < maxFel)
        {
            VisaStatus();
            char gissning = LäsGiltigBokstav();

            if (gissadeBokstäver.Contains(gissning))
            {
                AnsiConsole.MarkupLine("[bold red]Du har redan gissat den bokstaven![/]");
                continue;
            }

            gissadeBokstäver.Add(gissning);

            if (hemligtOrd.Contains(gissning))
            {
                SkickaNotis("[bold green]Rätt gissning![/]");
                if (AllaBokstäverGissade())
                {
                    AnsiConsole.MarkupLine($"[bold green]Grattis! Du vann! Ordet var: {hemligtOrd}[/]");
                    return;
                }
            }
            else
            {
                felaktigaGissningar++;
                SkickaNotis("[bold red]Fel gissning![/]");
            }
        }

        AnsiConsole.MarkupLine($"[bold red]Du förlorade! Ordet var: {hemligtOrd}[/]");
    }

    private char LäsGiltigBokstav()
    {
        while (true)
        {
            string input = AnsiConsole.Ask<string>("[bold yellow]Gissa en bokstav:[/] ").Trim().ToLower();
            if (input.Length == 1 && char.IsLetter(input[0]))
                return input[0];

            AnsiConsole.MarkupLine("[bold red]Felaktig inmatning! Ange EN bokstav.[/]");
        }
    }

    private void VisaStatus()
    {
        AnsiConsole.MarkupLine($"\n[bold cyan]Felaktiga gissningar:[/] {felaktigaGissningar}/{maxFel}");
        AnsiConsole.Markup("[bold cyan]Ord:[/] ");
        foreach (char bokstav in hemligtOrd)
            AnsiConsole.Write(gissadeBokstäver.Contains(bokstav) ? $"{bokstav} " : "_ ");
        Console.WriteLine();

        // Visa stickgubben baserat på antalet felaktiga gissningar
        RitaStickgubbe(felaktigaGissningar);
    }

    private bool AllaBokstäverGissade()
    {
        foreach (char bokstav in hemligtOrd)
            if (!gissadeBokstäver.Contains(bokstav))
                return false;
        return true;
    }

    private void LäggTillObservatör(IObserver observatör)
    {
        observatörer.Add(observatör);
    }

    private void SkickaNotis(string meddelande)
    {
        foreach (var obs in observatörer)
            obs.Uppdatera(meddelande);
    }

    private void FisherYatesShuffle(string[] lista)
    {
        Random rand = new Random();
        for (int i = lista.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }

    // Metod för att rita stickgubben
    private void RitaStickgubbe(int felaktigaGissningar)
    {
        string[] stickgubbe = {
            "  ____ \n |    |\n |    \n |    \n |    \n_|_",
            "  ____ \n |    |\n |    O\n |    \n |    \n_|_",
            "  ____ \n |    |\n |    O\n |    |\n |    \n_|_",
            "  ____ \n |    |\n |    O\n |   /|\n |    \n_|_",
            "  ____ \n |    |\n |    O\n |   /|\\\n |    \n_|_",
            "  ____ \n |    |\n |    O\n |   /|\\\n |   / \n_|_",
            "  ____ \n |    |\n |    O\n |   /|\\\n |   / \\\n_|_"
        };

        AnsiConsole.MarkupLine($"[bold red]{stickgubbe[felaktigaGissningar]}[/]");
    }
}

// Gallgen-observatör
class Gallgen : IObserver
{
    public void Uppdatera(string meddelande)
    {
        AnsiConsole.MarkupLine($"[italic grey](Gallgen uppdaterad: {meddelande})[/]");
    }
}

// Ordpanel-observatör
class Ordpanel : IObserver
{
    public void Uppdatera(string meddelande)
    {
        AnsiConsole.MarkupLine($"[italic grey](Ordpanel uppdaterad: {meddelande})[/]");
    }
}