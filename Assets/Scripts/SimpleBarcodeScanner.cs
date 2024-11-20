using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SimpleBarcodeScanner : MonoBehaviour
{
    public TMPro.TextMeshProUGUI barcodeAsText;
    BarcodeBehaviour mBarcodeBehaviour;
    public ProductInfo productInfo; // Reference to the ProductInfo script

    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>();
        productInfo.ShowOverlay(false);
    }


    void Update()
    {
        if (mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null)
        {
            barcodeAsText.text = mBarcodeBehaviour.InstanceData.Text;
            // Avatar

            // Simulated product data (replace with your API response)
            string productName = "Product ABC";
            string keyIngredients = "Ingredient 1, Ingredient 2";
            string benefits = "Benefit 1, Benefit 2";
            string sideEffects = "Side Effect 1, Side Effect 2";
            string usage = "Twice daily";
            string skinTypes = "All skin types";
            string sensitivities = "None";
            string skinConcerns = "Dryness, Acne";
            string price = "$19.99";

            // Update the UI with product information
            productInfo.setProductInfo(
                productName,
                keyIngredients,
                benefits,
                sideEffects,
                usage,
                skinTypes,
                sensitivities,
                skinConcerns,
                price
            );
        }
        else
        {
            barcodeAsText.text = "";
            productInfo.ShowOverlay(false);
        }
    }
}