using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using static JsonObjectMapper;

public class ProfileInfo : MonoBehaviour
{
    public TMP_InputField ageInput;
    public TMP_Dropdown genderDropdown;
    public TMP_Dropdown skinTypeDropdown;
    public TMP_Dropdown sensitiveSkinDropdown;

    public Toggle acneToggle, darkSpotsToggle, wrinklesToggle, drynessToggle, oilinessToggle, sensitivityToggle, poresToggle, sunToggle;
    public Toggle naturalToggle, fragrancesToggle, preservativesToggle, parabensToggle, dyesToggle, metalsToggle;
    public Toggle coconutOilToggle, aloeVeraToggle, sheaButterToggle, chamomileToggle, witchHazelToggle, sesameOilToggle, soyDerivativesToggle, nutOilsToggle;

    public TMP_InputField minPriceInput;
    public TMP_InputField maxPriceInput;

    public Toggle preferenceNaturalToggle, preferenceOrganicToggle, preferenceVeganToggle;

    public Button submitButton;
    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(SubmitFormData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SubmitFormData()
    {
        string age = ageInput.text;
        string gender = genderDropdown.options[genderDropdown.value].text;
        string skinType = skinTypeDropdown.options[skinTypeDropdown.value].text;
        string sensitiveSkin = sensitiveSkinDropdown.options[sensitiveSkinDropdown.value].text;

        List<string> skinConcerns = new List<string>();
        if (acneToggle.isOn) skinConcerns.Add("acne");
        if (darkSpotsToggle.isOn) skinConcerns.Add("darkspots");
        if (wrinklesToggle.isOn) skinConcerns.Add("wrinkles");
        if (drynessToggle.isOn) skinConcerns.Add("dryness");
        if (oilinessToggle.isOn) skinConcerns.Add("oiliness");
        if (sensitivityToggle.isOn) skinConcerns.Add("sensitivity");
        if (poresToggle.isOn) skinConcerns.Add("pores");
        if (sunToggle.isOn) skinConcerns.Add("sun");

        List<string> ingredientsToAvoid = new List<string>();
        if (naturalToggle.isOn) ingredientsToAvoid.Add("natural");
        if (fragrancesToggle.isOn) ingredientsToAvoid.Add("fragrances");
        if (preservativesToggle.isOn) ingredientsToAvoid.Add("preservatives");
        if (parabensToggle.isOn) ingredientsToAvoid.Add("parabens");
        if (dyesToggle.isOn) ingredientsToAvoid.Add("dyes");
        if (metalsToggle.isOn) ingredientsToAvoid.Add("metals");

        List<string> allergies = new List<string>();
        if (coconutOilToggle.isOn) allergies.Add("coconut_oil");
        if (aloeVeraToggle.isOn) allergies.Add("aloevera");
        if (sheaButterToggle.isOn) allergies.Add("sheabutter");
        if (chamomileToggle.isOn) allergies.Add("chamomile");
        if (witchHazelToggle.isOn) allergies.Add("witch_hazel");
        if (sesameOilToggle.isOn) allergies.Add("sesame_oil");
        if (soyDerivativesToggle.isOn) allergies.Add("soy_derivatives");
        if (nutOilsToggle.isOn) allergies.Add("nut_oils");

        string minPrice = minPriceInput.text;
        string maxPrice = maxPriceInput.text;

        List<string> preferences = new List<string>();
        if (preferenceNaturalToggle.isOn) preferences.Add("natural");
        if (preferenceOrganicToggle.isOn) preferences.Add("organic");
        if (preferenceVeganToggle.isOn) preferences.Add("vegan");

        ProfileData profileData = new ProfileData();

        profileData.age = age;
        profileData.gender = gender;
        profileData.skinType = skinType;
        profileData.sensitiveSkin = sensitiveSkin;
        profileData.skinConcerns = skinConcerns;
        profileData.ingredientsToAvoid = ingredientsToAvoid;
        profileData.knownAllergies = allergies;
        profileData.minPrice = minPrice;
        profileData.maxPrice = maxPrice;
        profileData.preferences = preferences;

        StartCoroutine(HttpUtil.SendProfileInfo(profileData, OnProfileInfoSuccess, OnProfileInfoError));

        Debug.Log("Age: " + age);
            Debug.Log("Gender: " + gender);
            Debug.Log("Skin Type: " + skinType);
            Debug.Log("Sensitive Skin: " + sensitiveSkin);
            Debug.Log("Skin Concerns: " + string.Join(", ", skinConcerns));
            Debug.Log("Ingredients to Avoid: " + string.Join(", ", ingredientsToAvoid));
            Debug.Log("Known Allergies: " + string.Join(", ", allergies));
            Debug.Log("Price Range: " + minPrice + " - " + maxPrice);
            Debug.Log("Preferences: " + string.Join(", ", preferences));
    }

    private void OnProfileInfoSuccess()
    {
        ToastNotification.Show("Profile Updated Successfully");
    }

    private void OnProfileInfoError(string error)
    {
        Debug.LogError("Error occurred: " + error);
    }

}
