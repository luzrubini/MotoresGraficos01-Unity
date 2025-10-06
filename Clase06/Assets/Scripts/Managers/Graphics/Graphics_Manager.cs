using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class Graphics_Manager : MonoBehaviour
{

    [SerializeField]
    TMP_Dropdown qualitydropdown;

    [SerializeField]
    UniversalRenderPipelineAsset URPqualityHigh;
    [SerializeField]
    UniversalRenderPipelineAsset URPqualityMedium;
    [SerializeField]
    UniversalRenderPipelineAsset URPqualityLow;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChooseQuality(int index)
    {
        switch (index)
        {
            case 0:
                URPqualityLow.shadowDistance = 20;
                break;
            case 1:
                URPqualityMedium.shadowDistance = 50;
                break;
            case 2:
                URPqualityHigh.shadowDistance = 100;
                break;
        }

        QualitySettings.SetQualityLevel(index, false);
    }
}
