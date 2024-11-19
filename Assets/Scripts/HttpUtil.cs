using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using static SpeechRecognition;

public static class HttpUtil
{
    private static string apiKey = ConfigManager.GetAPIKey("SttAPIKey");
    private static string cloudSttURL = "https://speech.googleapis.com/v1/speech:recognize?key=";

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
            string transcript = SpeechRecognition.SttResponseBody.ParseTranscript(request.downloadHandler.text);
            onSuccess?.Invoke(transcript);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

}
