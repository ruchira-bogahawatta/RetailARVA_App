using System;
using UnityEngine;

[System.Serializable]
public class SpeechRecognition
{
    // Request Body Class
    [System.Serializable]
    public class SttRequestBody
    {
        public Config config;
        public Audio audio;

        [System.Serializable]
        public class Config
        {
            public string encoding;
            public int sampleRateHertz;
            public string languageCode;
            public int audioChannelCount;
        }

        [System.Serializable]
        public class Audio
        {
            public string content;
        }

 
        public static SttRequestBody Create(string base64Audio, int sampleRateHertz, int audioChannelCount, string languageCode)
        {
            return new SttRequestBody
            {
                config = new Config
                {
                    encoding = "LINEAR16", 
                    sampleRateHertz = sampleRateHertz,
                    languageCode = languageCode,
                    audioChannelCount = audioChannelCount
                },
                audio = new Audio
                {
                    content = base64Audio
                }
            };
        }
    }

    // Response Body Class
    [System.Serializable]
    public class SttResponseBody
    {
        public Result[] results;

        [System.Serializable]
        public class Result
        {
            public Alternative[] alternatives;
            public string resultEndTime;
            public string languageCode;
        }

        [System.Serializable]
        public class Alternative
        {
            public string transcript;
            public string confidence;
        }

        
        public static string ParseTranscript(string responseText)
        {
            SttResponseBody response = JsonUtility.FromJson<SttResponseBody>(responseText);

            if (response != null && response.results != null)
            {
                string transcript = "";
                foreach (var result in response.results)
                {
                    if (result.alternatives != null && result.alternatives.Length > 0)
                    {
                        transcript += result.alternatives[0].transcript + " ";
                        Debug.Log("Confidence of " + result.alternatives[0].transcript + " is " + result.alternatives[0].confidence);

                    }
                }
                return transcript.Trim(); // Return the concatenated transcript
            }

            return ""; 
        }
    }
}
