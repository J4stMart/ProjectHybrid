using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform tarckposition;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //transform.position = new Vector3(tarckposition.position.x + startPos.x, tarckposition.position.y + startPos.y, tarckposition.position.z + startPos.z);
        transform.LookAt(tarckposition);

    }
}
