using UnityEngine;
using System.IO;

[System.Serializable]
public class Config
{
    public APIKeys APIKeys;
}

[System.Serializable]
public class APIKeys
{
    public string SttAPIKey;
}

public class ConfigManager : MonoBehaviour
{
    private static Config _config;

    // Load the config file
    private static void LoadConfig()
    {
        if (_config != null) return;

        // Load the JSON file from Resources
        TextAsset configFile = Resources.Load<TextAsset>("config");
        if (configFile == null)
        {
            Debug.LogError("Config file not found in Resources!");
            return;
        }

        // Deserialize JSON to Config object
        _config = JsonUtility.FromJson<Config>(configFile.text);
    }

    public static string GetAPIKey(string serviceName)
    {
        LoadConfig();

        if (_config == null || _config.APIKeys == null)
        {
            Debug.LogError("Config is not loaded or APIKeys section is missing.");
            return null;
        }

        switch (serviceName)
        {
            case "SttAPIKey":
                return _config.APIKeys.SttAPIKey;
            default:
                Debug.LogError($"API key for {serviceName} not found.");
                return null;
        }
    }
}
