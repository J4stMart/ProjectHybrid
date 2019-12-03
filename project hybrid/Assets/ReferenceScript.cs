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

    private void Start()
    {
        newTargetLocation.position = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        arTargetReference.position = arTarget.position;
        arCameraReference.position = arCamera.position;
        referenceVector = arTargetReference.position - arCameraReference.position;
        Debug.Log(referenceVector);
    }
}
