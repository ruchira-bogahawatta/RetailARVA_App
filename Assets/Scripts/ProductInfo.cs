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

    // Reference to the overlay GameObject
    [SerializeField] private GameObject overlay;

    public void setProductInfo(string productNameValue, string keyIngredientsValue, string benefitsValue,
                          string sideEffectsValue, string usageValue, string skinTypesValue,
                                 string sensitivitiesValue, string skinConcernsValue, string priceValue) {
        productName.text = productNameValue;
        keyIngredients.text = keyIngredientsValue;
        benefits.text = benefitsValue;
        sideEffects.text = sideEffectsValue;
        usage.text = usageValue;
        skinTypes.text = skinTypesValue;
        sensitivities.text = sensitivitiesValue;
        skinConcerns.text = skinConcernsValue;
        price.text = priceValue;

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
