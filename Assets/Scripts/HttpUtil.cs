using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using static JsonObjectMapper;
using Unity.VisualScripting;
using System;

public static class HttpUtil
{
    private static string apiKey = ConfigManager.GetAPIKey("SttAPIKey");
    private static string baseURL = "http://216.81.248.136:5000/api";
    private static string cloudSttURL = "https://speech.googleapis.com/v1/speech:recognize?key=";
    private static string cloudTssURL = "https://texttospeech.googleapis.com/v1beta1/text:synthesize?key=";
    private static string loginURL = baseURL + "/users/login";
    private static string registerURL = baseURL + "/users";


    // Coroutine to send AudioClip to Google Cloud Speech-to-Text API
    public static IEnumerator SendAudioToCloudSTT(string base64Audio, int sampleRateHertz , int audioChannelCount, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = cloudSttURL + apiKey;

        // Create the request body using the new SttRequestBody class
        SttRequestBody requestBody = SttRequestBody.Create(base64Audio, sampleRateHertz, audioChannelCount,  "en-US");

        string jsonBody = JsonUtility.ToJson(requestBody);

        // Prepare the UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody); 
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string transcript = JsonObjectMapper.SttResponseBody.ParseTranscript(request.downloadHandler.text);
            onSuccess?.Invoke(transcript);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    // Coroutine to send Text to Google Cloud Text-to-Speech API
    public static IEnumerator SendTextToCloudTTS(string textToConvert, System.Action<string> onSuccess, System.Action<string> onError) {

        string url = cloudTssURL + apiKey;

        TssRequestBody requestBody = TssRequestBody.Create(textToConvert);

        string jsonBody = JsonUtility.ToJson(requestBody);

        // Prepare the UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            TssResponseBody responseBody = JsonObjectMapper.TssResponseBody.Get(request.downloadHandler.text);
            onSuccess?.Invoke(responseBody.audioContent);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    public static IEnumerator CreateChat(System.Action<LLMResponseBody> onSuccess, System.Action<string>onError)
    {

        string url = baseURL + "/chat/new/" + SessionManager.UserID;

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            LLMResponseBody responseBody = JsonObjectMapper.LLMResponseBody.Get(request.downloadHandler.text);
            onSuccess?.Invoke(responseBody);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    //Send the request to LLM
    public static IEnumerator SendMsgToLlm(string userInquiry, System.Action<string> onSuccess, System.Action<string> onError) {

        LlmRequestBody requestBody = LlmRequestBody.Create(userInquiry, SessionManager.UserID);

        string url = baseURL + "/chat/" + SessionManager.ChatID;

        string jsonBody = JsonUtility.ToJson(requestBody);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            LLMResponseBody responseBody = JsonObjectMapper.LLMResponseBody.Get(request.downloadHandler.text);
            onSuccess?.Invoke(responseBody.data.content);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }


    public static IEnumerator GetProductInfo(string productID, System.Action<ProductInfoResponseBody> onSuccess, System.Action<string> onError)
    {
        string url = baseURL + "/products/" + productID;

        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ProductInfoResponseBody responseBody = JsonObjectMapper.ProductInfoResponseBody.Get(request.downloadHandler.text);
            onSuccess?.Invoke(responseBody);
        }
        else
        {
            onError?.Invoke(request.error);
        }

    }


    public static IEnumerator SendProfileInfo(ProfileData profileData, string userID, System.Action onSuccess, System.Action<string> onError)
    {
        string url = baseURL + "/profile/" + userID;

        ProfileDataReqBody requestBody = ProfileDataReqBody.Create(profileData);

        string jsonBody = JsonUtility.ToJson(requestBody);
        Debug.Log(jsonBody);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody); 
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }


    public static IEnumerator GetProfileInfo(System.Action<ProfileData> onSuccess, System.Action<string> onError)
    {
        string userID = SessionManager.UserID;

        string url = baseURL + "/profile/" + userID;

        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ProfileData responseBody = JsonObjectMapper.ProfileData.Get(request.downloadHandler.text);
            onSuccess?.Invoke(responseBody);
        }
        else if (request.responseCode == 404) {
            ProfileData responseBody = null;
            onSuccess?.Invoke(responseBody);

        } else { 
            onError?.Invoke(request.error);

        }

    }

    public static IEnumerator Login(String email, System.Action<UserInfo> onSuccess, System.Action<string> onError)
    {

        LoginRequestBody requestBody = LoginRequestBody.Create(email);

        string jsonBody = JsonUtility.ToJson(requestBody);

        UnityWebRequest request = new UnityWebRequest(loginURL, "POST");
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            UserInfo responseBody = JsonObjectMapper.UserInfo.Get(request.downloadHandler.text);

            //Debug.Log(responseBody.lName);
            //Debug.Log(responseBody.userID);
            //Debug.Log(responseBody.fName);
            //Debug.Log(responseBody.email);
            onSuccess?.Invoke(responseBody);
        }
        else if (request.responseCode == 404)
        {
            UserInfo userInfo = null;
            onSuccess?.Invoke(userInfo);

        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    public static IEnumerator Register(UserInfo userInfo, System.Action onSuccess, System.Action<string> onError)
    {

        string jsonBody = UserInfo.ToJson(userInfo);

        UnityWebRequest request = new UnityWebRequest(registerURL, "POST");
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();
        Debug.Log("Request sent in register");

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }


}
