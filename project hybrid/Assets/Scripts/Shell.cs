using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0,1,0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Hit");
            collision.transform.GetComponent<HitDetection>().gotHit = true;
            Destroy(gameObject);
        }
        //dirty check. change to tag later
        else if (collision.transform.name == "Tafel")
        {
            Destroy(gameObject);
        }
    }
}
