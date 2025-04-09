using UnityEngine;

[System.Serializable]
public class LoginData
{
    public string username;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
    public int diamond;
    public int heart;
}

public class LoginManager : MonoBehaviour
{
    public static string LoggedInUsername;
    public static int Diamond;
    public static int Heart;
    private string currentUsername;
    public void Login(string username, string password)
    {
        currentUsername = username;
        LoginData data = new LoginData
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(data);

        StartCoroutine(WebRequestManager.Instance.PostRequest(
            "https://test-piggy.codedefeat.com/worktest/dev05/login.php",
            json,
            OnLoginResponse));
    }

    private void OnLoginResponse(string responseJson)
    {
        LoginResponse res = JsonUtility.FromJson<LoginResponse>(responseJson);

        if (res.success)
        {
            LoggedInUsername = currentUsername;
            Diamond = res.diamond;
            Heart = res.heart;
            Debug.Log("Login Success");
            Debug.Log($"User: {LoggedInUsername}, Diamond: {Diamond}, Heart: {Heart}");
        }
        else
        {
            Debug.Log("Login Failed: " + res.message);
        }
    }
}
