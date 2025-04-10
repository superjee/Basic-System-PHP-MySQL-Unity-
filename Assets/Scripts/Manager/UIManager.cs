using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public RegisterManager registerManager;
    public LoginManager loginManager;
    [Header("loginPanel")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    [Header("signUpPanel")]
    public TMP_InputField signUp_usernameInput;
    public TMP_InputField signUp_passwordInput;
    public TMP_InputField signUp_confirmPasswordInput;
    [Header("LobbyPanel")]
    public TextMeshProUGUI lobby_DiamondText;
    public Slider lobby_HeartSlider;

    [Header("PanelObj")]
    public GameObject loginPanel;
    public GameObject signUpPanel;
    public GameObject lobbyPanal;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ShowLoginUI();
    }
    private void Start()
    {
        if (registerManager == null)
            registerManager = FindObjectOfType<RegisterManager>();

        if (loginManager == null)
            loginManager = FindObjectOfType<LoginManager>();

        lobby_HeartSlider.maxValue = 100;
        lobby_HeartSlider.minValue = 0;
    }

    public void UpdateDataUI(int diamond, int heart)
    {
        lobby_DiamondText.text = ""+ diamond;
        lobby_HeartSlider.value = heart;
    }

    public void OnRegisterClick()
    {
        string username = signUp_usernameInput.text;
        string password = signUp_passwordInput.text;
        string confirmPassword = signUp_confirmPasswordInput.text;

        // ตรวจ username
        if (!Validator.ValidateUsername(username, out string userError))
        {
            PopupManager.Instance.ShowMessage(userError);
            Debug.LogWarning("!!" + userError);
            return;
        }

        // ตรวจ password + confirm
        if (!Validator.ValidatePassword(password, confirmPassword, out string passError))
        {
            PopupManager.Instance.ShowMessage(passError);
            Debug.LogWarning("!!" + passError);
            return;
        }

        registerManager.Register(username, password);
    }

    public void OnLoginClick()
    {
        loginManager.Login(usernameInput.text, passwordInput.text);
    }

    public void OnSignUpClick()
    {
        loginPanel.SetActive(false);
        usernameInput.text = null;
        passwordInput.text = null;
        signUpPanel.SetActive(true);
    }

    public void OnBackClick()
    {
        signUpPanel.SetActive(false);
        signUp_usernameInput.text = null;
        signUp_passwordInput.text = null;
        signUp_confirmPasswordInput.text = null;
        loginPanel.SetActive(true);
    }

    public void ShowLobbyUI()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(false);
        lobbyPanal.SetActive(true);
    }

    public void ShowLoginUI()
    {
        usernameInput.text = null;
        passwordInput.text = null;
        signUp_usernameInput.text = null;
        signUp_passwordInput.text = null;
        signUp_confirmPasswordInput.text = null;
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
        lobbyPanal.SetActive(false);
    }
}
