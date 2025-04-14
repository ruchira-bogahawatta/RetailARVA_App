using System;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class JsonObjectMapper
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
                        //Debug.Log("Confidence of " + result.alternatives[0].transcript + " is " + result.alternatives[0].confidence);

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
                    audioEncoding = "LINEAR16"
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

    [Serializable]
    public class LlmRequestBody {
        public string user_id;
        public string message;
        public string role;
        public static LlmRequestBody Create(string userInquiry, string userID) {
            
            return new LlmRequestBody { 
                user_id = userID,    
                message = userInquiry ,
                role = "user"
            
            };
        }
    }

    [Serializable]
    public class LlmResponseBody
    {
        public string response;

        public static LlmResponseBody Get(string responseText)
        {
            LlmResponseBody response = JsonUtility.FromJson<LlmResponseBody>(responseText);
            return response;
        }

    }

    [Serializable]
    public class ProductInfoResponseBody
    {
        public ProductData data;

        [Serializable]
        public class ProductData
        {
            public List<string> allergens;
            public string application_tips;
            public List<string> benefits;
            public string brand;
            public string category;
            public string concentrations;
            public float average_rating;

            [JsonProperty("product_id")]
            public int id;
            public List<string> ingredients;
            public bool is_natural;
            public List<string> key_ingredients;
            public string name;
            public float price;
            public string sensitivities;
            public List<string> side_effects;
            public List<string> skin_concerns;
            public List<string> skin_types;
            public string usage;
            public string expert_review;
            public List<string> claims;
        }


        public static ProductInfoResponseBody Get(string responseText)
        {
            ProductInfoResponseBody response = JsonUtility.FromJson<ProductInfoResponseBody>(responseText);

            return response;
        }

    }

    [Serializable]
    public class ProfileData
    {
        public int age;
        public string gender;

        [JsonProperty("skin_type")]
        public string skinType;

        [JsonProperty("sensitive_skin")]
        public string sensitiveSkin;

        [JsonProperty("skin_concerns")]
        public List<string> skinConcerns;

        [JsonProperty("ingredients_to_avoid")]
        public List<string> ingredientsToAvoid;

        [JsonProperty("known_allergies")]
        public List<string> knownAllergies;

        [JsonProperty("min_price")]
        public float minPrice;

        [JsonProperty("max_price")]
        public float maxPrice;

        public List<string> preferences;

        // Constructor to initialize lists (optional, for safety)
        public ProfileData()
        {
            skinConcerns = new List<string>();
            ingredientsToAvoid = new List<string>();
            knownAllergies = new List<string>();
            preferences = new List<string>();
        }

        public static ProfileData Get(string responseText)
        {
            //ProfileData response = JsonUtility.FromJson<ProfileData>(responseText);

            JObject fullJson = JObject.Parse(responseText);
            JToken dataToken = fullJson["data"];
            return dataToken.ToObject<ProfileData>();
        }
    }

    public class ProfileDataReqBody
    {
        public ProfileData profileData;

        public static ProfileDataReqBody Create(ProfileData data)
        {
            return new ProfileDataReqBody
            {
                profileData = data
            };
        }
    }

    [Serializable]
    public class UserInfo
    {
        [JsonProperty("_id")]
        public string userID;

        [JsonProperty("first_name")]
        public string fName;

        [JsonProperty("last_name")]
        public string lName;

        [JsonProperty("email")]
        public string email;

        //[JsonProperty("created_at")]
        //public DateTime createdAt;

        public static UserInfo Get(string responseText)
        {
            JObject fullJson = JObject.Parse(responseText);
            JToken dataToken = fullJson["data"];
            return dataToken.ToObject<UserInfo>();
        }

        public static string ToJson(UserInfo data) {

            return JsonConvert.SerializeObject(data);

        }
    }

    public class UserInfoReqBody
    {
        public UserInfo userInfo;

        public static UserInfoReqBody Create(UserInfo data)
        {
            return new UserInfoReqBody
            {
                userInfo = data
            };
        }
    }

    [System.Serializable]
    public class LoginRequestBody
    {
        public string email;

        public static LoginRequestBody Create(String email)
        {
            return new LoginRequestBody
            {
                email = email
            };
        }
    }



    [Serializable]
    public class Oid
    {
        [JsonProperty("$oid")]
        public string oid;
    }

    [Serializable]
    public class LLMResponseData
    {
        [JsonProperty("_id")]
        public Oid id;
        public Oid chat_id;
        public string role;
        public int message_id;
        public string content;
    }

    [Serializable]
    public class LLMResponseBody
    {
        public LLMResponseData data;
        public string message;



        public static LLMResponseBody Get(string responseText)
        {
            return JsonConvert.DeserializeObject<LLMResponseBody>(responseText);
        }
    }


}
