using UnityEngine;
using Newtonsoft.Json;
[System.Serializable]
public class UserLoginData
{
    public int user_id;
    public int diamond;
    public int heart;
}

public class LoginManager : MonoBehaviour
{
    private string currentUsername;
    public void Login(string username, string password)
    {
        currentUsername = username;
        var data = new AuthRequest(username, password);
        string json = JsonUtility.ToJson(data);
        string url = UrlConfig.Instance.LoginEndpoint;
        StartCoroutine(WebRequestManager.Instance.PostRequest(
            url,
            json,
            OnLoginResponse));
    }

    private void OnLoginResponse(string responseJson)
    {
        if (UserDataSO.Instance == null)
        {
            Debug.LogError("!userData.asset Null");
            return;
        }
        Debug.Log("OnLoginResponse Raw JSON: " + responseJson);
        var res = JsonConvert.DeserializeObject<ServerResponse<UserLoginData>>(responseJson);
        if (res.success && res.data != null)
        {
            UserDataSO.Instance.SetAll(res.data.user_id, currentUsername, res.data.diamond, res.data.heart);
            Debug.Log("Login Success");
            Debug.Log($"User: {UserDataSO.Instance.Username}, Diamond: {UserDataSO.Instance.Diamond}, Heart: {UserDataSO.Instance.Heart}");
            PopupManager.Instance.ShowMessage(res.message);
            UIManager.Instance.UpdateDataUI(UserDataSO.Instance.Diamond, UserDataSO.Instance.Heart);
            UIManager.Instance.ShowLobbyUI();
        }
        else
        {
            Debug.Log("Login Failed: " + res.message);
            if (res.data == null)
                Debug.LogWarning("res.data is null");

            PopupManager.Instance.ShowMessage(res.message);
        }
    }
    public void Logout()
    {
        UserDataSO.Instance.ResetData();
        UIManager.Instance.ShowLoginUI();
        Debug.Log("Logged out!");
        PopupManager.Instance.ShowMessage("Logged out !");

    }
}
