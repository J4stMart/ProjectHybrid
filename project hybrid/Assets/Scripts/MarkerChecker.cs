using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MarkerChecker : MonoBehaviour, ITrackableEventHandler
{
    StateManager sm = TrackerManager.Instance.GetStateManager();

    private Camera cam;

    private bool audioPlayed = false;

    private const int mask = 3 << 10;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        IList<TrackableBehaviour> activeTrackables = (IList<TrackableBehaviour>)sm.GetActiveTrackableBehaviours();

        if (activeTrackables.Count == 0)
        {
            cam.cullingMask = mask;
            if (audioPlayed == false){
                GetComponent<AudioSource>().Play();
                audioPlayed = true;
            }
        }
        else
        {
            cam.cullingMask = ~mask | (1 << 11);
            audioPlayed = false;
        }
    }


    public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
    }
}
