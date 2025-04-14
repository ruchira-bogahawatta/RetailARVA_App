using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    public TMPro.TextMeshProUGUI welcomeMsg;
    public Button logoutBtn;

    // Start is called before the first frame update
    void Start()
    {
        logoutBtn.onClick.AddListener(Logout);

        if (SessionManager.UserID != null) { 
        
            welcomeMsg.text = "Hi " + SessionManager.FirstName + " " + SessionManager.LastName ;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Logout() {
        SceneChange sceneChange = FindObjectOfType<SceneChange>();
        SessionManager.UserID = null;
        SessionManager.FirstName = null;
        SessionManager.LastName = null;
        SessionManager.Email = null;
        SessionManager.ChatID = null;
        SessionManager.isLogged = false;
        sceneChange.ChangeScene("Login");


    }
}
