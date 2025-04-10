using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [Header("UI References")]
    public GameObject popupPanel;
    public TMP_Text messageText;

    private void Awake()
    {
        Instance = this;
        popupPanel.SetActive(false);
    }

    public void ShowMessage(string message, float duration = 5f)
    {
        popupPanel.SetActive(true);
        messageText.text = message;
        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), duration);
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }
}
