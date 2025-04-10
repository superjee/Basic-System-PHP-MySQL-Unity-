[System.Serializable]
public class AuthRequest
{
    public string username;
    public string password;

    public AuthRequest(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}