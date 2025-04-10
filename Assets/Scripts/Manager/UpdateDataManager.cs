using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class UpdateDataManager : MonoBehaviour
{
    int? lastAddDiamond = null;
    int? lastAddHeart = null;

    public void OnAddDiamond()
    {
        int userId = UserDataSO.Instance.UserId;
        UpdateDataRequest.SendUpdateRequest(userId, 100, null, OnUpdateResult);
        lastAddDiamond = 100;
    }

    void OnUpdateResult(string resJson)
    {
        var res = JsonConvert.DeserializeObject<ServerResponse<UserLoginData>>(resJson);
        if (res.success)
        {
            string message = "Update successful";

            if (lastAddDiamond.HasValue && lastAddHeart.HasValue)
            {
                message = "Diamond and Heart updated successfully!";
            }
            else if (lastAddDiamond.HasValue)
            {
                message = "Diamond updated successfully!";
                UserDataSO.Instance.SetDiamond(res.data.diamond);
            }
            else if (lastAddHeart.HasValue)
            {
                message = "Heart updated successfully!";
            }
            Debug.Log(message);
            PopupManager.Instance.ShowMessage(message);
        }
        else
        {
            Debug.LogWarning("Update failed: " + res.message);
            PopupManager.Instance.ShowMessage("Update failed: " + res.message);
        }
    }
}
