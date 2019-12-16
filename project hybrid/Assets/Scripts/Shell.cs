using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    Rigidbody rb;
    public AudioClip explosionSound1;
    public AudioClip explosionSound2;
    public AudioClip explosionSound3;
    public GameObject impactEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.forward = rb.velocity.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Hit");
            collision.transform.GetComponent<HitDetection>().gotHit = true;
            StartCoroutine(Explosion());
        }
        //dirty check. change to tag later
        else
        {
            StartCoroutine(Explosion());
        }
    }

    IEnumerator Explosion()
    {
        AudioClip explosion;
        int randomNumber = Random.Range(0, 3);
        if (randomNumber == 0) {
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
        GetComponent<AudioSource>().PlayOneShot(explosion, 1f);
        GetComponent<CapsuleCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Instantiate(impactEffect, transform.position + transform.forward *1.1f -transform.up*0.2f, Quaternion.RotateTowards(Quaternion.LookRotation(Vector3.up), transform.rotation, 5));

        yield return new WaitForSeconds(0.01f);
        Vector3 explosionPos = transform.position + transform.forward*1.1f;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 9f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(800, explosionPos, 9f, 3.0F);
        }

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);        
    }
}
