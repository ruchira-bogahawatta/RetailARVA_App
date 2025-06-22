using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    public TMPro.TextMeshProUGUI welcomeMsg;
    public TMPro.TextMeshProUGUI baseUrlMsg;
    public TMP_InputField baseUrlTxt;
    public TMP_InputField apiKeyTxt;

    public Button logoutBtn;
    public Button settingBtn;
    public Button overlayCloseBtn;
    public Button saveURLBtn;
    public Button startBtn;


    // Reference to the overlay GameObject
    [SerializeField] private GameObject overlay;
    void Start()
    {
        this.ShowSettingOverlay(false);
        logoutBtn?.onClick.AddListener(Logout);
        startBtn?.onClick.AddListener(ChangeToArCamera);
        saveURLBtn?.onClick.AddListener(SaveBaseURL);
        settingBtn?.onClick.AddListener(() => ShowSettingOverlay(true));
        overlayCloseBtn?.onClick.AddListener(CloseSettingOverlay);
        UserCheck();
        BaseURLCheck();

    }

    // Update is called once per frame
    void Update()
    {

    }


    void UserCheck()
    {

        welcomeMsg?.SetText(
            SessionManager.UserID != null
                ? $"Hi {SessionManager.FirstName} {SessionManager.LastName}"
                : "Hi There"
        );

    }

    void BaseURLCheck()
    {

        if (!string.IsNullOrEmpty(SessionManager.baseURL) || SessionManager.baseURL == "Base URL is not updated")
        {
            baseUrlMsg.text = "Base URL is : " + SessionManager.baseURL + "\nAPI Key : " + SessionManager.apiKey;
        }
        else
        {
            baseUrlMsg.text = "Base URL is not updated";
        }

    }

    void Logout()
    {
        SceneChange sceneChange = FindObjectOfType<SceneChange>();
        SessionManager.UserID = null;
        SessionManager.FirstName = null;
        SessionManager.LastName = null;
        SessionManager.Email = null;
        SessionManager.ChatID = null;
        SessionManager.isLogged = false;
        SessionManager.baseURL = null;
        SessionManager.LastScannedProductID = 0;
        SessionManager.isAvatarSpawned = false;
        SessionManager.welcomeMsg = null;
        SessionManager.apiKey = null;
        sceneChange.ChangeScene("Login");


    }

    public void ShowSettingOverlay(bool isVisible)
    {
        if (overlay != null)
        {
            overlay.SetActive(isVisible);
        }
    }

    void CloseSettingOverlay()
    {
        this.ShowSettingOverlay(false);
    }

    void SaveBaseURL()
    {
        string baseURL = baseUrlTxt.text.Trim();
        string apiKey = apiKeyTxt.text.Trim();
        Console.Write("API KEY :", apiKey);
        if (!string.IsNullOrEmpty(baseURL) || !string.IsNullOrEmpty(apiKey))
        {
            SessionManager.baseURL = "http://" + baseURL + ":5000/api";
            SessionManager.apiKey = apiKey;
            baseUrlTxt.text = "";
            this.CloseSettingOverlay();
            BaseURLCheck();
        }
        else
        {
            ToastNotification.Show("Invalid URL / API Key");
        }
    }

    void ChangeToArCamera()
    {
        if (!string.IsNullOrEmpty(SessionManager.baseURL) && !string.IsNullOrEmpty(SessionManager.apiKey))
        {
            SceneChange sceneChange = FindObjectOfType<SceneChange>();
            sceneChange.ChangeScene("AR Camera");
        }
        else
        {
            ToastNotification.Show("Please set the Base URL & API Key first");
        }

    }
}