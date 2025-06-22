using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static JsonObjectMapper;

public class Auth : MonoBehaviour
{
    public TMP_InputField firstNameTxt;
    public TMP_InputField lastNameTxt;
    public TMP_InputField emailTxt;

    public TMP_InputField loginEmailTxt;

    public Button loginBtn;
    public Button registerBtn;


    void Start()
    {
        if (loginBtn != null)
        {
            if (SessionManager.isLogged == true)
            {
                ToastNotification.Show("User Registered Successfully");
                SessionManager.isLogged = false;
            }
            loginBtn.onClick.AddListener(Login);
        }
        if (registerBtn != null)
        {
            registerBtn.onClick.AddListener(register);
        }
    }
    void Update()
    {

    }

    public void register()
    {
        registerBtn.interactable = false;
        string fName = firstNameTxt.text;
        string lName = lastNameTxt.text;
        string email = emailTxt.text;

        UserInfo userInfo = new UserInfo();

        userInfo.email = email;
        userInfo.fName = fName;
        userInfo.lName = lName;
        StartCoroutine(HttpUtil.Register(userInfo, OnRegisterSuccess, OnRegisterError));
    }

    public void Login()
    {
        loginBtn.interactable = false;
        if (!string.IsNullOrEmpty(SessionManager.baseURL) && !string.IsNullOrEmpty(SessionManager.apiKey))
        {
            string loginEmail = loginEmailTxt.text;
            if (!string.IsNullOrWhiteSpace(loginEmail))
            {
                StartCoroutine(HttpUtil.Login(loginEmail, OnLoginSuccess, OnLoginError));
            }
            else
            {
                ToastNotification.Show("Enter a proper email");
                loginBtn.interactable = true;
            }
        }
        else
        {
            ToastNotification.Show("Please set the Base URL & API Key first");
            loginBtn.interactable = true;
        }

    }


    private void OnRegisterSuccess()
    {
        SessionManager.isLogged = true;
        SceneChange sceneChange = FindObjectOfType<SceneChange>();
        sceneChange.ChangeScene("Login");
        registerBtn.interactable = true;
    }

    private void OnRegisterError(string error)
    {
        Debug.Log("Error occurred: " + error);
    }

    private void OnLoginSuccess(UserInfo userInfo)
    {
        if (userInfo != null)
        {
            SceneChange sceneChange = FindObjectOfType<SceneChange>();
            SessionManager.UserID = userInfo.userID;
            SessionManager.FirstName = userInfo.fName;
            SessionManager.LastName = userInfo.lName;
            SessionManager.Email = userInfo.email;

            sceneChange.ChangeScene("Welcome Screen");
        }
        else
        {
            loginEmailTxt.text = "";
            ToastNotification.Show("Invalid Email");
        }
        loginBtn.interactable = true;

    }

    private void OnLoginError(string error)
    {
        loginBtn.interactable = true;
        Debug.Log("Error occurred: " + error);
    }
}