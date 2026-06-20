using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Tournament
{
    public List<Group> groups { get; set; } = new();
    public List<Game> games { get; set; } = new();
    public string name { get; set; } = string.Empty;
}

public class Group
{
    public string name { get; set; } = string.Empty;
    public List<Team> teams { get; set; } = new();
}

public class Team
{
    public string name { get; set; } = string.Empty;
}

public class Game
{
    public string GameID { get; set; } = string.Empty;

    public Team homeTeam { get; set; } = new();
    public Team awayTeam { get; set; } = new();

    public string result { get; set; } = string.Empty;

    public List<Odd> odds { get; set; } = new();

    public void SetResult(string result)
    {
        this.result = result;
    }
}

public class Odd
{
    public string BetType { get; set; } = string.Empty;
    public double Value { get; set; }
}

public class Bet
{
    public string BetID { get; set; } = string.Empty;

    public string GameID { get; set; } = string.Empty;

    public string BetType { get; set; } = string.Empty;

    public double OddValue { get; set; }

    public double Stake { get; set; }

    public bool IsEvaluated { get; set; }

    public bool IsWon { get; set; }

    public double PotentialWin()
    {
        return Stake * OddValue;
    }
}

public class User
{
    public string Name { get; set; } = string.Empty;

    public double Balance { get; set; }

    public void adjustBalance(double amount)
    {
        Balance += amount;
    }
}

public static class PersistenceManager
{
    public static void SaveTournament(
        Tournament tournament,
        string filename)
    {
        string json = JsonSerializer.Serialize(
            tournament,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(filename, json);
    }

    public static Tournament LoadTournament(string filename)
    {
        string json = File.ReadAllText(filename);

        return JsonSerializer.Deserialize<Tournament>(json)
               ?? throw new Exception("Turnier konnte nicht geladen werden.");
    }
}

public class Program
{
    public static Tournament NewTournament()
    {
        Tournament tournament = new Tournament();

        Console.WriteLine("Name des Turniers:");
        tournament.name = Console.ReadLine() ?? "";

        Console.WriteLine("Anzahl Gruppen:");

        int anzahlGruppen;
        while (!int.TryParse(Console.ReadLine(), out anzahlGruppen))
        {
            Console.WriteLine("Bitte Zahl der Teams in der Gruppe eigeben.");
        }

        for (int i = 0; i < anzahlGruppen; i++)
        {
            Group group = new Group();

            Console.WriteLine($"Name Gruppe {i + 1}:");
            group.name = Console.ReadLine() ?? "";

            Console.WriteLine($"Anzahl Teams in {group.name}:");

            int anzahlTeams;
            while (!int.TryParse(Console.ReadLine(), out anzahlTeams))
            {
                Console.WriteLine("Bitte Zahl eingeben.");
            }

            for (int j = 0; j < anzahlTeams; j++)
            {
                Team team = new Team();

                Console.WriteLine($"Name Team {j + 1}:");
                team.name = Console.ReadLine() ?? "";

                group.teams.Add(team);
            }

            tournament.groups.Add(group);

            Console.WriteLine(
                $"Spiele für Gruppe {group.name} erstellen");

            int gameCounter = 1;

            for (int a = 0; a < group.teams.Count; a++)
            {
                for (int b = a + 1; b < group.teams.Count; b++)
                {
                    Game game = new Game();

                    game.GameID =
                        $"G_{group.name}_{gameCounter++}";

                    game.homeTeam = group.teams[a];
                    game.awayTeam = group.teams[b];

                    Console.WriteLine(
                        $"{game.GameID}: {game.homeTeam.name} vs {game.awayTeam.name}");

                    Console.WriteLine("Quote Heimsieg:");
                    double homeOdd =
                        double.Parse(Console.ReadLine() ?? "1");

                    Console.WriteLine("Quote Unentschieden:");
                    double drawOdd =
                        double.Parse(Console.ReadLine() ?? "1");

                    Console.WriteLine("Quote Auswärtssieg:");
                    double awayOdd =
                        double.Parse(Console.ReadLine() ?? "1");

                    game.odds.Add(new Odd
                    {
                        BetType = "Heimsieg",
                        Value = homeOdd
                    });

                    game.odds.Add(new Odd
                    {
                        BetType = "Unentschieden",
                        Value = drawOdd
                    });

                    game.odds.Add(new Odd
                    {
                        BetType = "Auswärtssieg",
                        Value = awayOdd
                    });

                    tournament.games.Add(game);
                }
            }
        }

        return tournament;
    }

    public static void PrintTournament(Tournament tournament)
    {
        Console.WriteLine("\n===============================");
        Console.WriteLine($"Turnier: {tournament.name}");
        Console.WriteLine("===============================");

        foreach (Group group in tournament.groups)
        {
            Console.WriteLine($"\nGruppe {group.name}");

            foreach (Team team in group.teams)
            {
                Console.WriteLine($" - {team.name}");
            }
        }

        Console.WriteLine("\nSpiele:");

        foreach (Game game in tournament.games)
        {
            Console.WriteLine(
                $"{game.GameID}: {game.homeTeam.name} vs {game.awayTeam.name}");

            foreach (Odd odd in game.odds)
            {
                Console.WriteLine(
                    $"   {odd.BetType}: {odd.Value}");
            }

            if (!string.IsNullOrWhiteSpace(game.result))
            {
                Console.WriteLine(
                    $"   Ergebnis: {game.result}");
            }
        }
    }

    public static void Main()
    {
        Tournament tournament = NewTournament();

        PersistenceManager.SaveTournament(
            tournament,
            "tournament.json");

        Console.WriteLine(
            "\nTurnier wurde gespeichert.\n");

        Tournament loadedTournament =
            PersistenceManager.LoadTournament(
                "tournament.json");

        PrintTournament(loadedTournament);
    }
}

 /// #FIXME der JSON Serializer macht probleme 