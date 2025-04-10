using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public RegisterManager registerManager;
    public LoginManager loginManager;
    [Header("loginPanel")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    [Header("signUpPanel")]
    public TMP_InputField signUp_usernameInput;
    public TMP_InputField signUp_passwordInput;
    public TMP_InputField signUp_confirmPasswordInput;
    [Header("PanelObj")]
    public GameObject loginPanel;
    public GameObject signUpPanel;

    private void Awake()
    {
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    public void OnRegisterClick()
    {
        string username = signUp_usernameInput.text;
        string password = signUp_passwordInput.text;
        string confirmPassword = signUp_confirmPasswordInput.text;

        // ตรวจ username
        if (!Validator.ValidateUsername(username, out string userError))
        {
            //warningText.text = userError;
            Debug.LogWarning("!!" + userError);
            return;
        }

        // ตรวจ password + confirm
        if (!Validator.ValidatePassword(password, confirmPassword, out string passError))
        {
            //warningText.text = passError;
            Debug.LogWarning("!!" + passError);
            return;
        }

        //warningText.text = "";
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
}
