using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MarkerChecker : MonoBehaviour, ITrackableEventHandler
{
    [SerializeField]
    private GameObject MainARMarker;
    /*[SerializeField]
    private GameObject SecondaryARMarker1;
    [SerializeField]
    private GameObject SecondaryARMarker2;
    [SerializeField]
    private GameObject SecondaryARMarker3;
    [SerializeField]
    private GameObject SecondaryARMarker4;*/

    private TrackableBehaviour mTrackableBehaviour;
    /*private TrackableBehaviour s1TrackableBehaviour;
    private TrackableBehaviour s2TrackableBehaviour;
    private TrackableBehaviour s3TrackableBehaviour;
    private TrackableBehaviour s4TrackableBehaviour;*/

    private Camera cam;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();

        mTrackableBehaviour = MainARMarker.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        /*s1TrackableBehaviour = SecondaryARMarker1.GetComponent<TrackableBehaviour>();
        if (s1TrackableBehaviour)
        {
            s1TrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        s2TrackableBehaviour = SecondaryARMarker1.GetComponent<TrackableBehaviour>();
        if (s2TrackableBehaviour)
        {
            s2TrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        s3TrackableBehaviour = SecondaryARMarker1.GetComponent<TrackableBehaviour>();
        if (s3TrackableBehaviour)
        {
            s3TrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        s4TrackableBehaviour = SecondaryARMarker1.GetComponent<TrackableBehaviour>();
        if (s4TrackableBehaviour)
        {
            s4TrackableBehaviour.RegisterTrackableEventHandler(this);
        }*/
    }

    void Update()
    {

        if (mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED || mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED || mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Main");
            cam.cullingMask = 1 << 0 | 1 << 11;
        }
        /*else if (s1TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED || s1TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED || s1TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("S1");
        }
        else if (s2TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED)
        {
            Debug.Log("S2");
        }
        else if (s3TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED)
        {
            Debug.Log("S3");
        }
        else if (s4TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED)
        {
            Debug.Log("S4");
        }*/
        else
        {
            Debug.LogWarning("No marker found.");
            cam.cullingMask = 3 << 10;
        }
    }


    public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
    }
}
