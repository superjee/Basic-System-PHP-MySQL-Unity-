using UnityEngine;

[CreateAssetMenu(fileName = "UrlConfig", menuName = "Config/URL")]
public class UrlConfig : ScriptableObject
{
    private static UrlConfig _instance;
    public static UrlConfig Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<UrlConfig>("UrlConfig");
            if (_instance == null)
                Debug.LogError("UrlConfig.asset not found in Resources folder!");

            return _instance;
        }
    }

    [Header("Base URL (without trailing slash)")]
    public string baseURL = "https://test-piggy.codedefeat.com/worktest/dev05";

    [Header("Endpoints")]
    [SerializeField] private string registerPath = "/api/auth/register.php";
    [SerializeField] private string loginPath = "/api/auth/login.php";
    [SerializeField] private string updateDataPath = "/api/user/update_data.php";

    public string RegisterEndpoint => $"{baseURL}{registerPath}";
    public string LoginEndpoint => $"{baseURL}{loginPath}";
    public string UpdateDataEndpoint => $"{baseURL}{updateDataPath}";
}
