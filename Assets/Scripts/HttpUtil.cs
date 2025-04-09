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
    private static string cloudSttURL = "https://speech.googleapis.com/v1/speech:recognize?key=";
    private static string cloudTssURL = "https://texttospeech.googleapis.com/v1beta1/text:synthesize?key=";
    //private static string llmURL = "https://alert-evolved-chicken.ngrok-free.app/api/chat";
    private static string llmURL = "https://fc0fa5abad6ca6d800770feea90d44e8.loophole.site/";
    private static string profileInfoUrl = "https://fc0fa5abad6ca6d800770feea90d44e8.loophole.site/profile";
    private static string prodcutInfoURL = "https://fc0fa5abad6ca6d800770feea90d44e8.loophole.site/product";
    private static string loginURL = "https://fc0fa5abad6ca6d800770feea90d44e8.loophole.site/login";
    private static string registerURL = "https://fc0fa5abad6ca6d800770feea90d44e8.loophole.site/register";
    // private static string prodcutInfoURL = "https://alert-evolved-chicken.ngrok-free.app/api/products/";


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


    //Send the request to LLM
    public static IEnumerator SendMsgToLlm(string userInquiry, System.Action<string> onSuccess, System.Action<string> onError) {

        LlmRequestBody requestBody = LlmRequestBody.Create(userInquiry);

        string jsonBody = JsonUtility.ToJson(requestBody);
        //UnityWebRequest request = new UnityWebRequest(llmURL, "POST");
        UnityWebRequest request = new UnityWebRequest(llmURL, "GET");
        //byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonBody);
        //request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        //request.SetRequestHeader("Content-Type", "application/json");

        //request.timeout = 180;
        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            LlmResponseBody responseBody = JsonObjectMapper.LlmResponseBody.Get(request.downloadHandler.text);
            onSuccess?.Invoke(responseBody.response);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }


    public static IEnumerator GetProductInfo(string productID, System.Action<ProductInfoResponseBody> onSuccess, System.Action<string> onError)
    {
        //string url = prodcutInfoURL + productID;
        string url = prodcutInfoURL;

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


    public static IEnumerator SendProfileInfo(ProfileData profileData, System.Action onSuccess, System.Action<string> onError)
    {

        ProfileDataReqBody requestBody = ProfileDataReqBody.Create(profileData);

        string jsonBody = JsonUtility.ToJson(requestBody);
        UnityWebRequest request = new UnityWebRequest(profileInfoUrl, "POST");
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


    public static IEnumerator GetProfileInfo(string username, System.Action<ProfileData> onSuccess, System.Action<string> onError)
    {
        //string url = prodcutInfoURL + productID;
        string url = profileInfoUrl;

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

        UserInfoReqBody requestBody = UserInfoReqBody.Create(userInfo);

        string jsonBody = JsonUtility.ToJson(requestBody);
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
