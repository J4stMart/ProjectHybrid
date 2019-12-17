using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageLayerSetter : MonoBehaviour
{
    [SerializeField] private int ammountOfChildren;

    private Transform videoBackgroundTransform;

    void Update()
    {
        if (gameObject.transform.childCount == ammountOfChildren)
        {
            videoBackgroundTransform = gameObject.transform.GetChild(ammountOfChildren-1);
            videoBackgroundTransform.gameObject.layer = 11;
            this.enabled = false;
        }
    }
}
