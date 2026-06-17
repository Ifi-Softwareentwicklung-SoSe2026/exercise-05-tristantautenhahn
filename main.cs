using GamblingApp;

public class Tournament
{
    public List<Group> groups;
    public List<Game> games; 

    public String name; 

}

public class Group 
{
    public string name { get; set; }
    public List<Team> teams { get; set; } = new(); 
}

public class Team
{
    public string name { get; set; }
}

public class Game
{
    public string GameID { get; set; }
    public Team homeTeam { get; set; }
    public Team awayTeam { get; set; }
    public string result { get; set; }
    public List<Odd> odds { get; set; } = new();

    public void SetResult(string result)
    {
        this.result = result;
    }
}

public class Odd
{
   //  public string BetType { get; set; }
    public double Value { get; set; } 
}

public class Bet
{
    public string BetType { get; set; }
    public double oddValue { get; set; }
    public double stake { get; set; }
    public bool isEvaluated { get; set; }
}

public class User
{
    public string Name { get; set; }
    public double Balance { get; set; }
    public void adjustBalance(double amount)
    {
        Balance += amount;
    }
}
public class Main
{
    public static void New_Tournament()
    {
        Tournament tournament = new Tournament();
        Console.WriteLine("Geben Sie den Namen des Turniers ein:");
        tournament.name = Console.ReadLine() ?? string.Empty;
        int anzahl = 0;
        Console.WriteLine("Wie viele Gruppen soll es in diesem Turnier geben?");
        int.TryParse(Console.ReadLine(), out anzahl);
        Console.WriteLine("Bitte Erstellen Sie die Gruppen für das Turnier:");
        for (int i = 0; i < anzahl; i++){
            Group group = new Group();
            Console.WriteLine($"Bitte geben Sie den Namen der Gruppe {i+1} ein:");
            group.name = Console.ReadLine() ?? string.Empty;
            int anzahlTeams = 0;
            Console.WriteLine($"Wie viele Teams soll es in der Gruppe {group.name} geben?");
            int.TryParse(Console.ReadLine(), out anzahlTeams); 
            Console.WriteLine($"Bitte Erstellen Sie die Teams für die Gruppe {group.name}:");
            for (int j = 0; j < anzahlTeams; j++){
                Team team = new Team();
                Console.WriteLine($"Bitte geben Sie den Namen des Teams {j+1} ein:");
                team.name = Console.ReadLine() ?? string.Empty;
                group.teams.Add(team);
            }
            Console.WriteLine($"Bitte Erstellen Sie jetzt die Spiele für die Gruppe {group.name}");
            anzahlSpiele = anzahlTeams * (anzahlTeams - 1) / 2;
            for (int k = 0; k < anzahlSpiele; k++)
            {
                Game game = new Game();
                GameID = $"G{group.name}{k+1}";
                Console.WriteLine($"Bitte geben Sie den Namen des Heimteams für das Spiel {GameID} ein:");
                string homeTeamName = Console.ReadLine() ?? string.Empty;
                Console.WriteLine($"Bitte geben Sie den Namen des Auswärtsteams für das Spiel {GameID} ein:");
                string awayTeamName = Console.ReadLine() ?? string.Empty;
                Console.WriteLine($"Bitte geben Sie das Datum für das Spiel {GameID} ein (Format: yyyy-MM-dd):");
                string dateInput = Console.ReadLine() ?? string.Empty;
                DateTime gameDate;
                while (!DateTime.TryParse(dateInput, out gameDate))
                {
                    Console.WriteLine("Ungültiges Datum. Bitte geben Sie das Datum im Format yyyy-MM-dd ein:");
                    dateInput = Console.ReadLine() ?? string.Empty;
                }
                Console.WriteLine($"Bitte geben Sie den Ausgang des Spiels {GameID} ein (Heimsieg, Unentschieden, Auswärtssieg):");
                string result = Console.ReadLine() ?? string.Empty;
                Console.WriteLine($"Bitte geben Sie die Quoten für das Spiel {GameID} ein:");
                Console.WriteLine("Heimsieg Quote:");
                Odd odd = new Odd();
                odd.Value = double.Parse(Console.ReadLine() ?? string.Empty); 
                Console.WriteLine("Auswertsteam Quote:");
                Odd odd = new Odd();
                odd.Value = double.Parse(Console.ReadLine() ?? string.Empty); 

            }

    }
    public static void Main (){
         
    }
}
public class PersistenceManager
{

    // Datenspeicherung 
    public void SpeichernAlsJSON(string dateipfad)
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dateipfad, json);
    }

    // Daten wieder bekommen
    public static Tournament LadenAusJSON(string dateipfad)
    {
        string json = File.ReadAllText(dateipfad);
        Tournament tournament = JsonSerializer.Deserialize<Tournament>(json) ?? throw new InvalidDataException($"JSON-Datei konnte nicht gelesen werden: {dateipfad}");
        return tournament;
    }


    public void SaveTournament(Tournament tournament)
    {
        string filename = "tournament.json";
        SpeichernAlsJSON(filename); 
    }


    void saveBets(List<Bet> bets, string filename)
    {
        Directory.CreateDirectory(filename);

        int i = 1; 
        foreach (Bet bet in bets)
        {
            string betFile = Path.Combine(filename, $"bet_{i}.json");
            bet.SpeichernAlsJSON(betFile); 
            i++;
        }
    }


    public static List<Bet> LadenAusJSON(string filename)
    {
        string json = File.ReadAllText(filename);
        List<Bet> bets = JsonSerializer.Deserialize<List<Bet>>(json) ?? throw new InvalidDataException($"JSON-Datei konnte nicht gelesen werden: {filename}");
        return bets;
        // ?? nullcheck --> wenn null das was dahinter steht  
    }


    void saveUsers(List<User> users, string filename)
    {
        Directory.CreateDirectory(filename);

        int i = 1; 
        foreach (User user in users)
        {
            string userFile = Path.Combine(filename, $"user_{i}.json");
            user.SpeichernAlsJSON(userFile); 
            i++;
        }   

    }

    public static List<User> LadenAusJSON(string filename)
    {
        string json = File.ReadAllText(filename);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? throw new InvalidDataException($"JSON-Datei konnte nicht gelesen werden: {filename}");
        return users;   
    }
    
}