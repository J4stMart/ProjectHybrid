using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceScript : MonoBehaviour
{
    public Transform arTarget;
    public Transform arTargetReference;
    public Transform arCameraReference;
    public Transform arCamera;
    public Transform newTargetLocation;
    public Transform newCamera;

    private Vector3 referenceVector;

    // Update is called once per frame
    void Update()
    {
        arTargetReference.position = arTarget.position;
        arCameraReference.position = arCamera.position;
        referenceVector = arTargetReference.position - arCameraReference.position;
        Debug.Log(referenceVector);
    }
}
