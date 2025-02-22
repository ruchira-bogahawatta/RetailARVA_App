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
        productInfo.setProductInfo(
        productData.data.name,
        productData.data.key_ingredients,
        productData.data.benefits,
        productData.data.side_effects == "NULL" ? "None" : productData.data.side_effects,
        productData.data.usage,
        productData.data.skin_types,
        productData.data.sensitivities == "NULL" ? "None" : productData.data.sensitivities,
        productData.data.skin_concerns,
        "Rs. " + productData.data.price.ToString()
    );

    }
    private void OnProductInfoError(string error)
    {
        Debug.LogError("Error occurred: " + error);

    }

}