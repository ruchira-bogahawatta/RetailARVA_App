using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SimpleBarcodeScanner : MonoBehaviour
{
    public TMPro.TextMeshProUGUI barcodeAsText;
    BarcodeBehaviour mBarcodeBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>();
    }


    void Update()
    {
        if (mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null)
        {
            barcodeAsText.text = mBarcodeBehaviour.InstanceData.Text;
            // Avatar
            
        }
        else
        {
            barcodeAsText.text = "";
        }
    }
}