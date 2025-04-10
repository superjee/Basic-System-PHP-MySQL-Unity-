using UnityEngine;
public class RegisterManager : MonoBehaviour
{
    public void Register(string username, string password)
    {
        var data = new AuthRequest(username, password);
        string json = JsonUtility.ToJson(data);
        string url = UrlConfig.Instance.RegisterEndpoint;
        StartCoroutine(WebRequestManager.Instance.PostRequest(
            url,
            json,
            OnRegisterResponse));
    }

    private void OnRegisterResponse(string responseJson)
    {
        ServerResponse<object> res = JsonUtility.FromJson<ServerResponse<object>>(responseJson);
        if (res.success)
        {
            Debug.Log("Register Success: " + res.message);
            UIManager.Instance.OnBackClick();
            PopupManager.Instance.ShowMessage(res.message);
        }
        else
        {
            Debug.Log("Register Failed: " + res.message);
            PopupManager.Instance.ShowMessage(res.message);
        }
    }
}