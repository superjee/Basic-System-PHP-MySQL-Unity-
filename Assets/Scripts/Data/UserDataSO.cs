using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Game/User Data")]
public class UserDataSO : ScriptableObject
{
    private static UserDataSO _instance;
    public static UserDataSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<UserDataSO>("UserData");

                if (_instance == null)
                    Debug.LogError("UserData.asset not found in Resources folder!");
            }
            return _instance;
        }
    }

    [Header("User Data")]
    [SerializeField] private int userid;
    [SerializeField] private string username;
    [SerializeField] private int diamond;
    [SerializeField] private int heart;
    public int UserId => userid;
    public string Username => username;
    public int Diamond => diamond;
    public int Heart => heart;
    public void ResetData()
    {
        userid = 0;
        username = "";
        diamond = 0;
        heart = 0;
    }
    public void SetAll(int userId, string username, int diamond, int heart)
    {
        this.userid = userId;
        this.username = username;
        this.diamond = diamond;
        this.heart = heart;
    }
    public void SetDiamond(int amount)
    {
        diamond = Mathf.Clamp(amount, 0, 10000);
        UIManager.Instance.lobby_DiamondText.text = ""+diamond;
    }

    public void SetHeart(int amount)
    {
        heart = Mathf.Clamp(amount, 0, 100);
        UIManager.Instance.lobby_HeartSlider.value = heart;
    }
}
