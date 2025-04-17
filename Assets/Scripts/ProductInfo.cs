using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductInfo : MonoBehaviour
{

    public TMPro.TextMeshProUGUI productName;
    public TMPro.TextMeshProUGUI keyIngredients;
    public TMPro.TextMeshProUGUI benefits;
    public TMPro.TextMeshProUGUI sideEffects;
    public TMPro.TextMeshProUGUI usage;
    public TMPro.TextMeshProUGUI skinTypes;
    public TMPro.TextMeshProUGUI sensitivities;
    public TMPro.TextMeshProUGUI skinConcerns;
    public TMPro.TextMeshProUGUI price;
    public TMPro.TextMeshProUGUI expertReview;
    public TMPro.TextMeshProUGUI claims;
    public TMPro.TextMeshProUGUI averageRating;

    // Reference to the overlay GameObject
    [SerializeField] private GameObject overlay;

    public void setProductInfo(string productNameValue, string keyIngredientsValue, string benefitsValue,
                          string sideEffectsValue, string usageValue, string skinTypesValue,
                                 string allergensValue, string skinConcernsValue, string priceValue, string claimsValue, string expertReviewValue, string average_rating) {
        productName.text = productNameValue;
        keyIngredients.text = keyIngredientsValue;
        benefits.text = benefitsValue;
        sideEffects.text = sideEffectsValue;
        usage.text = usageValue;
        skinTypes.text = skinTypesValue;
        sensitivities.text = allergensValue;
        skinConcerns.text = skinConcernsValue;
        price.text = priceValue;
        claims.text = claimsValue;
        expertReview.text = expertReviewValue;
        averageRating.text = "Average Rating : " + average_rating;

        ShowOverlay(true);
    }

    public void ShowOverlay(bool isVisible)
    {
        if (overlay != null)
        {
            overlay.SetActive(isVisible);
        }
    }

}
