using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviour
{
    private Transform trackposition = null;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //transform.position = new Vector3(tarckposition.position.x + startPos.x, tarckposition.position.y + startPos.y, tarckposition.position.z + startPos.z);
        if (transform != null)
            transform.LookAt(trackposition);
    }

    public void LookAt(Transform car)
    {
        trackposition = car;
    }
}
