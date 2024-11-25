using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeechRecognition
{
    // STT Request Body
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

    //STT Request Body
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

    // TTS Request Body
    [System.Serializable]
    public class TssRequestBody
    {
        public SynthInput input;
        public VoiceConfig voice;
        public AudioConfig audioConfig;

        [Serializable]
        public class SynthInput
        {
            public string text;
        }

        [Serializable]
        public class SynthOutput
        {
            public string audioContent;
        }

        [Serializable]
        public class VoiceConfig
        {
            public string languageCode;
            public string name;
            public string ssmlGender;
        }

        [Serializable]
        public class AudioConfig
        {
            public string audioEncoding;

        }

        public static TssRequestBody Create(string textToConvert)
        {
            return new TssRequestBody
            {
                input = new SynthInput
                {
                    text = textToConvert,
                },
                voice = new VoiceConfig
                {
                    languageCode = "en-US",
                    name = "en-US-Journey-F",
                    ssmlGender = "FEMALE"
                },
                audioConfig = new AudioConfig
                {
                    audioEncoding = "MP3"
                }
            };
        }
    }

    // TTS Response Body
    [Serializable]
    public class TssResponseBody
    {
        public string audioContent;
        public List<Timepoint> timepoints;
        public AudioConfig audioConfig;

        [Serializable]
        public class Timepoint
        {
            public string markName;
            public float timeSeconds;
        }

        [Serializable]
        public class AudioConfig
        {
            public string audioEncoding;
            public double speakingRate;
            public double pitch;
            public double volumeGainDb;
            public int sampleRateHertz;
            public List<string> effectsProfileId;
        }

        public static TssResponseBody Get(string responseText) 
        {
            TssResponseBody response = JsonUtility.FromJson<TssResponseBody>(responseText);

            return response;
        }
    }

}
