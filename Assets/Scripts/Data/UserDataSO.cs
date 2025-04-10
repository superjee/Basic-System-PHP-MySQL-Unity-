using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Game/User Data")]
public class UserDataSO : ScriptableObject
{
    public string username;
    public int diamond;
    public int heart;

    public void ResetData()
    {
        username = "";
        diamond = 0;
        heart = 0;
    }
}
