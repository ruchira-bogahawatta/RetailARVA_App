using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using static JsonObjectMapper;

public class SimpleBarcodeScanner : MonoBehaviour
{
    public TMPro.TextMeshProUGUI barcodeAsText;
    BarcodeBehaviour mBarcodeBehaviour;
    public ProductInfo productInfo;
    public Button overlayCloseBtn;

    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>();
        productInfo.ShowOverlay(false);
        overlayCloseBtn.onClick.AddListener(closeProductOverlay);
    }


    void Update()
    {
        if (mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null)
        {
            barcodeAsText.text = mBarcodeBehaviour.InstanceData.Text;
            StartCoroutine(HttpUtil.GetProductInfo(mBarcodeBehaviour.InstanceData.Text, OnProductInfoSuccess, OnProductInfoError));
        }
        else
        {
            barcodeAsText.text = "";
        }
    }

    void closeProductOverlay()
    {
        productInfo.ShowOverlay(false);
    }

    private void OnProductInfoSuccess(ProductInfoResponseBody productData)
    {
        Debug.Log(productData.data.benefits);
        Debug.Log(productData.data.side_effects);
        Debug.Log(productData.data);

        productInfo.setProductInfo(
        productData.data.name,
        string.Join(", ", productData.data.key_ingredients),
        string.Join(", ", productData.data.benefits),
        string.Join(", ", productData.data.side_effects),
        productData.data.usage,
        string.Join(", ", productData.data.skin_types),
        string.Join(", ", productData.data.allergens),
        string.Join(", ", productData.data.skin_concerns),
        "Rs. " + productData.data.price.ToString(),
        string.Join(", ", productData.data.claims),
        productData.data.expert_review,
        productData.data.average_rating.ToString()
    );

    }
    private void OnProductInfoError(string error)
    {
        Debug.LogError("Error occurred: " + error);

    }

}