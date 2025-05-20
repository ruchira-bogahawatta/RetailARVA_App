using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseurlInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    public Button settingBtn;
    public Button overlayCloseBtn;
    public Button saveURLBtn;
    public TMP_InputField baseUrlTxt;
    public TMPro.TextMeshProUGUI baseUrlMsg;


    [SerializeField] private GameObject overlay;



    void Start()
    {
        this.ShowSettingOverlay(false);
        saveURLBtn.onClick.AddListener(SaveBaseURL);
        settingBtn.onClick.AddListener(() => ShowSettingOverlay(true));
        overlayCloseBtn.onClick.AddListener(CloseSettingOverlay);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BaseURLCheck()
    {
        if (!string.IsNullOrEmpty(SessionManager.baseURL) || SessionManager.baseURL == "Base URL is not updated")
        {
            baseUrlMsg.text = "Base URL is : " + SessionManager.baseURL;
        }
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

        if (!string.IsNullOrEmpty(baseURL) || SessionManager.baseURL == "Base URL is not updated")
        {
            SessionManager.baseURL = "http://" + baseURL + ":5000/api";
            baseUrlTxt.text = "";
            this.CloseSettingOverlay();
            BaseURLCheck();
        }
        else
        {
            ToastNotification.Show("Invalid URL");
        }
    }
}