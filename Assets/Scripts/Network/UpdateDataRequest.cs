using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public static class UpdateDataRequest
{
    [System.Serializable]
    public class UpdateDataRequestPayload
    {
        public int user_id;
        public int? add_diamond;
        public int? add_heart;

        public UpdateDataRequestPayload(int userId, int? diamond, int? heart)
        {
            user_id = userId;
            add_diamond = diamond;
            add_heart = heart;
        }
    }
    public static void SendUpdateRequest(int userId, int? addDiamond, int? addHeart, System.Action<string> callback)
    {
        var payload = new UpdateDataRequestPayload(userId, addDiamond, addHeart);
        string json = JsonConvert.SerializeObject(payload);
        string url = UrlConfig.Instance.UpdateDataEndpoint;
        WebRequestManager.Instance.StartCoroutine(
            WebRequestManager.Instance.PostRequest(
                url,
                json,
                callback
            )
        );
    }
}
