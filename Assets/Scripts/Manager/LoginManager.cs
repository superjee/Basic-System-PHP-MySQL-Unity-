using UnityEngine;

[System.Serializable]
public class UserLoginData
{
    public int diamond;
    public int heart;
}

public class LoginManager : MonoBehaviour
{
    public UserDataSO userData;
    private string currentUsername;

    public void Login(string username, string password)
    {
        currentUsername = username;
        var data = new AuthRequest(username, password);
        string json = JsonUtility.ToJson(data);
        string url = UrlConfig.Instance.LoginEndpoint;
        StartCoroutine(WebRequestManager.Instance.PostRequest(
            "https://test-piggy.codedefeat.com/worktest/dev05/api/auth/login.php",
            json,
            OnLoginResponse));
    }

    private void OnLoginResponse(string responseJson)
    {
        var res = JsonUtility.FromJson<ServerResponse<UserLoginData>>(responseJson);
        if (res.success)
        {
            userData.username = currentUsername;
            userData.diamond = res.data.diamond;
            userData.heart = res.data.heart;
            Debug.Log("Login Success");
            Debug.Log($"User: {userData.username}, Diamond: {userData.diamond}, Heart: {userData.heart}");
        }
        else
        {
            Debug.Log("Login Failed: " + res.message);
        }
    }
    public void Logout()
    {
        userData.ResetData();
        Debug.Log("Logged out!");
    }
}
