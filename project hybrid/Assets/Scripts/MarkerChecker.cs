using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MarkerChecker : MonoBehaviour, ITrackableEventHandler
{
    StateManager sm = TrackerManager.Instance.GetStateManager();

    private Camera cam;

    private bool audioPlayed = false;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        IList<TrackableBehaviour> activeTrackables = (IList<TrackableBehaviour>)sm.GetActiveTrackableBehaviours();

        if (activeTrackables.Count == 0)
        {
            cam.cullingMask = 3 << 10;
            if (audioPlayed == false){
                GetComponent<AudioSource>().Play();
                audioPlayed = true;
            }
        }
        else
        {
            cam.cullingMask = 1 << 0 | 1 << 11;
            audioPlayed = false;
        }
    }


    public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
    }
}
