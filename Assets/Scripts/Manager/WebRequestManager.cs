using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;

public class WebRequestManager : MonoBehaviour
{
    public static WebRequestManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator PostRequest(string url, string jsonData, System.Action<string> callback)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] body = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            callback?.Invoke(request.downloadHandler.text);
        else
            Debug.LogError("Error: " + request.error);
    }
}
