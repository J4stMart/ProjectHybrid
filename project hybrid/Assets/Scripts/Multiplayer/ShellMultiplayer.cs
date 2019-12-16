using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class ShellMultiplayer : MonoBehaviourPun
{
    private Rigidbody rb;

    public AudioClip explosionSound1;
    public AudioClip explosionSound2;
    public AudioClip explosionSound3;
    private AudioSource audioSource;

    public GameObject impactEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (PhotonNetwork.IsMasterClient)
            audioSource.Play();
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;

        transform.forward = rb.velocity.normalized;

        if (transform.position.z < -40)
        {
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
            StartCoroutine(Explosion());
        }
        //dirty check. change to tag later
        else if (collision.transform.name == "Tafel")
        {
            StartCoroutine(Explosion());
        }
    }

    IEnumerator Explosion()
    {
        photonView.RPC("PlayExplosionAudio", RpcTarget.MasterClient);

        GetComponent<CapsuleCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        PhotonNetwork.Instantiate(impactEffect.name, transform.position + transform.forward * 1.1f - transform.up * 0.2f, Quaternion.RotateTowards(Quaternion.LookRotation(Vector3.up), transform.rotation, 5));

        yield return new WaitForSeconds(0.01f);
        Vector3 explosionPos = transform.position + transform.forward * 1.1f;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 9f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(800, explosionPos, 9f, 3.0F);
        }

        yield return new WaitForSeconds(1f);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void PlayExplosionAudio()
    {
        AudioClip explosion;
        int randomNumber = Random.Range(0, 3);
        if (randomNumber == 0)
        {
            explosion = explosionSound1;
        }
        else if (randomNumber == 1)
        {
            explosion = explosionSound2;
        }
        else
        {
            explosion = explosionSound3;
        }
        audioSource.PlayOneShot(explosion, 1f);
    }
}
