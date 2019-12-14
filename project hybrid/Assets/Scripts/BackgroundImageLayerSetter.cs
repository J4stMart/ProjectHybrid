using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageLayerSetter : MonoBehaviour
{
    private Transform videoBackgroundTransform;

    void Update()
    {
        if (gameObject.transform.childCount == 3)
        {
            videoBackgroundTransform = gameObject.transform.GetChild(2);
            videoBackgroundTransform.gameObject.layer = 11;
            this.enabled = false;
        }
    }
}
