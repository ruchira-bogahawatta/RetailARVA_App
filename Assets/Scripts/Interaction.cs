using System;
using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Interaction : MonoBehaviour
{
    public GameObject avatar;
    public GameObject loadingIcon;
    public Button avatarBtn;
    public GameObject planeFinder;
    public TMPro.TextMeshProUGUI transcriptText;

    private bool isPlaneFinder = false;
    private Color activeBtnColor = new Color(1f, 1f, 1f, 0.353f);
    private Color defaultBtnColor = new Color(38f / 255f, 38f / 255f, 38f / 255f); 



    void Start()
    {
        transcriptText.text = "";
        loadingIcon.SetActive(false);
        planeFinder.SetActive(isPlaneFinder);
        avatarBtn.GetComponent<Image>().color = defaultBtnColor;
        avatarBtn.onClick.AddListener(TogglePlaneFinder);
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
        transcriptText.text = "Transcript :  " + transcript;
        // send the text to LLM 
        StartCoroutine(HttpUtil.SendMsgToLlm(transcript, LLMOnSuccess, LLMOnError));
        loadingIcon.SetActive(true);
    }

    private void STTOnError(string error)
    {
        Debug.LogError("Error occurred: " + error);
    }

    private void LLMOnSuccess(string llmResponse)
    {
        Debug.Log("LLM Response: " + llmResponse);
        transcriptText.text = "LLM Response :  " + llmResponse;
        StartCoroutine(HttpUtil.SendTextToCloudTTS(llmResponse, TSSOnSuccess, TSSOnError));
    }

    private void LLMOnError(string error)
    {
        Debug.LogError("Error occurred: " + error);
        transcriptText.text = "LLM Error : " + error;
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
        transcriptText.text = "TSS error : " + error;
    }

    private void TogglePlaneFinder()
    {
        // Toggle the state of isPlaneFinder
        isPlaneFinder = !isPlaneFinder;
        // Enable or disable the Plane Finder accordingly

        if (isPlaneFinder)
        {
            avatarBtn.GetComponent<Image>().color = activeBtnColor;
            planeFinder.SetActive(isPlaneFinder);
        }
        else 
        { 
            avatarBtn.GetComponent<Image>().color = defaultBtnColor;
            planeFinder.SetActive(isPlaneFinder);
        }
    }
}
