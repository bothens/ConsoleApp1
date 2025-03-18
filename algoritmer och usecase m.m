Inlämningsrapport – Hangman

1. Designmönster – Observer Pattern

Motivering

I vårt Hangman-spel behöver vi uppdatera UI när spelets status förändras, t.ex. när en spelare gör en gissning eller gallgen uppdateras. För att lösa detta på ett strukturerat sätt implementerar vi Observer Pattern, vilket gör att olika delar av spelet kan prenumerera på förändringar och uppdateras automatiskt.

Use Case

Titel: Uppdatera spelets UI vid statusändring

Beskrivning: När spelaren gissar en bokstav ska spelet uppdatera både gallgen och det visade ordet automatiskt.

Aktörer: Spelaren, spelet

Flöde:

Spelaren gissar en bokstav.

Spelet kontrollerar om gissningen är rätt.

Om gissningen är fel uppdateras gallgen.

Om gissningen är rätt uppdateras det visade ordet.

UI-komponenterna uppdateras automatiskt via Observer Pattern.

User Story

"Som spelare vill jag att spelets UI uppdateras automatiskt när jag gissar en bokstav, så att jag direkt ser hur långt jag kommit."

Kodexempel – Implementering av Observer Pattern

interface IObserver
{
    void Update(string status);
}

class Gallgen : IObserver
{
    public void Update(string status)
    {
        Console.WriteLine("Gallgen uppdateras: " + status);
    }
}

class Ordpanel : IObserver
{
    public void Update(string status)
    {
        Console.WriteLine("Ordpanel uppdateras: " + status);
    }
}

class HangmanGame
{
    private List<IObserver> observers = new List<IObserver>();
    
    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }
    
    public void NotifyObservers(string status)
    {
        foreach (var observer in observers)
        {
            observer.Update(status);
        }
    }

    public void Spela(char gissning)
    {
        // Logik för att kontrollera gissning
        string status = "Gissning: " + gissning;
        NotifyObservers(status);
    }
}

2. Algoritmer 🤖

Algoritm 1 – HashSet för snabba bokstavskontroller

Motivering: Vi använder redan HashSet<char> för att snabbt kontrollera om en bokstav har gissats tidigare. HashSet har O(1)-komplexitet för uppslagningar, vilket gör spelet snabbare.

Kodexempel:

HashSet<char> gissadeBokstäver = new HashSet<char>();

if (gissadeBokstäver.Contains(gissning))
{
    Console.WriteLine("Du har redan gissat denna bokstav!");
}
else
{
    gissadeBokstäver.Add(gissning);
}

Algoritm 2 – Fisher-Yates Shuffle för att blanda ordlistan

Motivering: Vi vill att orden slumpas på ett mer effektivt sätt varje gång spelet startas. Fisher-Yates-algoritmen gör detta i O(n)-tid.

Kodexempel:

static void FisherYatesShuffle(string[] ordlista)
{
    Random rand = new Random();
    for (int i = ordlista.Length - 1; i > 0; i--)
    {
        int j = rand.Next(i + 1);
        (ordlista[i], ordlista[j]) = (ordlista[j], ordlista[i]);
    }
}

3. Slutsats ✅

Genom att implementera Observer Pattern har vi skapat en mer skalbar och strukturerad kod där UI-komponenter automatiskt uppdateras. Dessutom förbättrar HashSet prestandan vid gissningar och Fisher-Yates Shuffle säkerställer att orden blandas effektivt. Dessa optimeringar gör spelet både mer responsivt och robust!

