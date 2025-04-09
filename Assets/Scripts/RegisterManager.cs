using UnityEngine;

[System.Serializable]
public class RegisterData
{
    public string username;
    public string password;
}

[System.Serializable]
public class RegisterResponse
{
    public bool success;
    public string message;
}

public class RegisterManager : MonoBehaviour
{
    public void Register(string username, string password)
    {
        RegisterData data = new RegisterData
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(data);

        StartCoroutine(WebRequestManager.Instance.PostRequest(
            "https://test-piggy.codedefeat.com/worktest/dev05/register.php",
            json,
            OnRegisterResponse));
    }

    private void OnRegisterResponse(string responseJson)
    {
        RegisterResponse res = JsonUtility.FromJson<RegisterResponse>(responseJson);

        if (res.success)
            Debug.Log("Register Success: " + res.message);
        else
            Debug.Log("Register Failed: " + res.message);
    }
}
