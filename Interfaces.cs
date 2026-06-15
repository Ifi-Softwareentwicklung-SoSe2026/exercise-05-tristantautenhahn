namespace GamblingApp; 

public interface SpeichernAlsJSON
{
    void SaveAsJSON(string filename);
    static abstract Tournament LoadTournamentsFromJSON(string filename);
    static abstract List<Bet> LoadBetsFromJSON(string filename);
    static abstract List<User> LoadUsersFromJSON(string filename);

}