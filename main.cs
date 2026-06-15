using GamblingApp;

public class Tournament
{
    private List<Group> groups;
    private List<Game> games; 

}

public class Group
{
    private string name; 
    private List<Team> teams;
}

public class Team
{
    private string name; 
}

public class Game
{
    private string GameID;
    private Team homeTeam;
    private Team awayTeam; 
    private string result;
    private List<Odd> odds; 

    public void SetResult(string result)
    {
        this.result = result;
    }
}

public class Odd
{
    private string BetType;
    private double Value; 
}

public class Bet
{
    private string BetType;
    private double oddValue;
    private double stake; 
    private bool isEvaluated;
}

public class User
{
    private string Name;
    private double Balance; 
    public void adjustBalance(double amount)
    {
        Balance += amount;
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
        // FIXME 
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