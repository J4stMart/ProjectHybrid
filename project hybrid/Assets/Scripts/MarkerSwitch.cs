using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MarkerSwitch : MonoBehaviour, ITrackableEventHandler
{
    [SerializeField]
    private GameObject GameWorld;
    [SerializeField]
    private GameObject MainARMarker;
    [SerializeField]
    private GameObject SecondaryARMarker1;
    /*[SerializeField]
    private GameObject SecondaryARMarker2;
    [SerializeField]
    private GameObject SecondaryARMarker3;
    [SerializeField]
    private GameObject SecondaryARMarker4;*/

    private TrackableBehaviour mTrackableBehaviour;
    private TrackableBehaviour s1TrackableBehaviour;
    /*private TrackableBehaviour s2TrackableBehaviour;
    private TrackableBehaviour s3TrackableBehaviour;
    private TrackableBehaviour s4TrackableBehaviour;*/

    void Start()
    {
        mTrackableBehaviour = MainARMarker.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        s1TrackableBehaviour = SecondaryARMarker1.GetComponent<TrackableBehaviour>();
        if (s1TrackableBehaviour)
        {
            s1TrackableBehaviour.RegisterTrackableEventHandler(this);
        }/*
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
        GameWorld.transform.position = Vector3.zero;

        if (mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED || mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED || mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Main");
            //GameWorld.gameObject.GetComponent<BoxCollider>().enabled = true;
            foreach(Transform child in GameWorld.transform)
            {
                GameWorld.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
            GameWorld.transform.SetParent(MainARMarker.transform);
        }
        else if (s1TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.DETECTED || s1TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED || s1TrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("S1");
            //GameWorld.gameObject.GetComponent<BoxCollider>().enabled = true;
            foreach (Transform child in GameWorld.transform)
            {
                GameWorld.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
            GameWorld.transform.SetParent(SecondaryARMarker1.transform);
        }/*
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
            //GameWorld.gameObject.GetComponent<BoxCollider>().enabled = true;
            //GameWorld.gameObject.GetComponent<MeshRenderer>().enabled = true;
            GameWorld.transform.SetParent(null);
        }
    }

    public void OnTrackableStateChanged(
    TrackableBehaviour.Status previousStatus,
    TrackableBehaviour.Status newStatus)
    {
    }
}