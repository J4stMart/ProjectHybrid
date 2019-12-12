using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public bool gotHit = false;

    void Update()
    {
        if(gotHit){
            Debug.Log("Got hit");
        }
    }
}
