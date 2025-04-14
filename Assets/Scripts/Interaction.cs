using System;
using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;


public class Interaction : MonoBehaviour
{
    public GameObject avatar;
    public GameObject loadingIcon;
    public Button avatarBtn;
    public GameObject planeFinder;
    public TMPro.TextMeshProUGUI transcriptText;
    public Animator animator;


    private bool isPlaneFinder = false;
    private Color activeBtnColor = new Color(1f, 1f, 1f, 0.353f);
    private Color defaultBtnColor = new Color(38f / 255f, 38f / 255f, 38f / 255f); 



    void Start()
    {
        StartCoroutine(HttpUtil.CreateChat(CreateChatOnSuccess, CreateChatOnError));
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
        transcriptText.text = "Transcript :  " + transcript;
        StartCoroutine(HttpUtil.SendMsgToLlm(transcript, LLMOnSuccess, LLMOnError));
        loadingIcon.SetActive(true);

    }

    private void STTOnError(string error)
    {
        Debug.LogError("Error occurred: " + error);
    }

    private void LLMOnSuccess(string llmResponse)
    {
        string cleanedResponse = Regex.Replace(llmResponse, @"(//|/n2|/n|/|\\{1,2}|\*\*|\*)", "");
        transcriptText.text = "LLM Response :  " + cleanedResponse;
        StartCoroutine(HttpUtil.SendTextToCloudTTS(cleanedResponse, TSSOnSuccess, TSSOnError));
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
        animator = avatar.GetComponent<Animator>();


        loadingIcon.SetActive(false);
        voiceHandler.AudioSource.loop = false;
        voiceHandler.AudioSource.mute = false;
        voiceHandler.AudioSource.volume = 1.0f;

        animator.SetBool("IsTalking", true);
        voiceHandler.PlayAudioClip(audioClip);

        StartCoroutine(StopTalkingWhenDone(audioClip.length, animator));    

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

    public void CreateChatOnSuccess(JsonObjectMapper.LLMResponseBody llmres) {
        SessionManager.ChatID = llmres.data.chat_id.oid;
        ToastNotification.Show("Conversation Initialized");
    }

    public void CreateChatOnError(string error)
    {
        Debug.Log(error);
        ToastNotification.Show("Error on chat initialization");
    }


    private IEnumerator StopTalkingWhenDone(float delay, Animator animator)
    {
        yield return new WaitForSeconds(delay);
        if (animator != null)
            animator.SetBool("IsTalking", false);
    }
}
