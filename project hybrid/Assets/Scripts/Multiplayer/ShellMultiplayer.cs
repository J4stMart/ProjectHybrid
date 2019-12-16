using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShellMultiplayer : MonoBehaviourPun
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;

        transform.forward = rb.velocity.normalized;

        if (transform.position.z < -40)
        {
            Debug.Log("hit");
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;

        if (collision.transform.tag == "Player")
        {
            Debug.Log("Hit");
            collision.transform.GetComponent<HitDetection>().gotHit = true;
            PhotonNetwork.Destroy(gameObject);
        }
        //dirty check. change to tag later
        else if (collision.transform.name == "Tafel")
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
