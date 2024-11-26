using System;
using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;
using System.IO;

public class Interaction : MonoBehaviour
{
    public GameObject avatar;
    public GameObject loadingIcon;
    // Start is called before the first frame update
    void Start()
    {
        loadingIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendMsg(AudioClip audioClip, string base64Data) 
    {
        StartCoroutine(HttpUtil.SendAudioToCloudSTT(base64Data, audioClip.frequency, audioClip.channels, STTOnSuccess, STTOnError));
    }

    private void STTOnSuccess(string transcript)
    {
        Debug.Log("Transcription: " + transcript);

        // send the text to LLM 
        transcript = "how many pieces are include with Rivaj UK Deep Cleansing Black Mask Whitening Complex?";
        StartCoroutine(HttpUtil.SendMsgToLlm(transcript, LLMOnSuccess, LLMOnError));
        loadingIcon.SetActive(true);

    }

    private void STTOnError(string error)
    {
        Debug.LogError("Error occurred: " + error);
    }

    private void LLMOnSuccess(string llmResponse)
    {
        Debug.LogError("LLM Response: " + llmResponse);
        StartCoroutine(HttpUtil.SendTextToCloudTTS(llmResponse, TSSOnSuccess, TSSOnError));
    }

    private void LLMOnError(string error)
    {
        Debug.LogError("Error occurred: " + error);
    }

    private void TSSOnSuccess(string base64Audio)
    {
        //Convert to Audio Clip
        AudioClip audioClip = AudioUtil.GetClipFromBase64(base64Audio);

        // Feed to avatar
        VoiceHandler voiceHandler = avatar.GetComponent<VoiceHandler>();

        loadingIcon.SetActive(false);
        voiceHandler.AudioSource.loop = false;
        voiceHandler.AudioSource.mute = false;
        voiceHandler.AudioSource.volume = 1.0f;
        voiceHandler.PlayAudioClip(audioClip);
    }

    private void TSSOnError(string error)
    {
        Debug.LogError("Error occurred: " + error);
    }

}
