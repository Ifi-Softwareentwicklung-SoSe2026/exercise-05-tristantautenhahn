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

