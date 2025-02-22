using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.Tilemaps;
using TMPro;

public class MicrophoneBehavior : MonoBehaviour
{
    public Button recordButton;
    public Interaction interaction;

    private AudioClip audioClip;
    private bool isRecording = false;
    private Color stopColor = new Color(38f / 255f, 38f / 255f, 38f / 255f); 
    private Color recordingColor = new Color(195f / 255f, 6f / 255f, 6f / 255f);  

    void Start()
    {
        recordButton.onClick.AddListener(ToggleRecording);
    }

    void ToggleRecording()
    {
        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    void StartRecording()
    {
        Debug.Log(Microphone.devices.Length);
        recordButton.GetComponent<Image>().color = recordingColor;
        audioClip = Microphone.Start(null, false, 15, 44100); // 10 seconds max, 44100 Hz
        isRecording = true;
    }

    void StopRecording()
    {
        if (!isRecording) return;

        Microphone.End(null);
        isRecording = false;
        recordButton.GetComponent<Image>().color = stopColor;
        ConvertAudioClip(audioClip);
    }

    void ConvertAudioClip(AudioClip audioClip)
    {
        AudioClip trimmedClip = AudioUtil.TrimSilence(audioClip, 0.005f);
        string base64DataLinear16 = AudioUtil.ConvertToBase64Linear16(trimmedClip);
        interaction.SendMsg(trimmedClip, base64DataLinear16);

    }

}
