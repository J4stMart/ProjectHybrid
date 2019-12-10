using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCenterOfMass : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3 (0, -0.5f, 0);
        Debug.DrawLine(transform.position, GetComponent<Rigidbody>().centerOfMass);
    }
}