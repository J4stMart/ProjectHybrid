using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Destruction : MonoBehaviour
{
    private GameObject tank;
    public GameObject onderstelFractures;
    public GameObject koepelFractures;
    public GameObject physicsColliders;
    public GameObject ArCamera;
    public bool isDestroyed;
    public bool gotHit;
    public int explosionForce = 0;
    public Transform target;
    [SerializeField] private List<GameObject> fractures;


    // Start is called before the first frame update
    void Awake()
    {
        tank = gameObject;
        ArCamera = GameObject.FindWithTag("MainCamera");

        foreach (Transform fracture in onderstelFractures.transform)
        {
            if (fracture.tag == "Fractures")
            {
                fractures.Add(fracture.gameObject);
            }
        }
        foreach (Transform fracture in koepelFractures.transform)
        {
            if (fracture.tag == "Fractures")
            {
                fractures.Add(fracture.gameObject);
            }
        }
        for (int i = 0; i < fractures.Count; i++)
        {
            fractures[i].SetActive(false);
        }
        isDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tank != null && ((Input.GetKey("b") && !isDestroyed) || (gotHit && !isDestroyed)) || tank.transform.position.y <-40f)
        {
            isDestroyed = true;

            for (int i = 0; i < fractures.Count; i++)
            {
                fractures[i].SetActive(true);
                fractures[i].GetComponent<Rigidbody>().velocity = tank.GetComponent<Rigidbody>().velocity;
                fractures[i].transform.parent = null;
            }
            Explosion();
            Destroy(tank);
        }
    }

    private void Explosion()
    {
        Vector3 explosionPos = tank.transform.position;
        float radius = tank.GetComponent<CapsuleCollider>().radius;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(explosionForce, explosionPos, radius, radius/4);
        }
    }
}
