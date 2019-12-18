using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Multiplayer;

public class TankHitDetection : MonoBehaviourPun
{
    public GameObject onderstelFractures;
    public GameObject koepelFractures;
    public GameObject loopFractures;

    [HideInInspector]
    public bool gotHit = false;

    [SerializeField]
    private int explosionForce = 0;

    private List<GameObject> fractures;

    private void Awake()
    {
        fractures = new List<GameObject>();

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
        foreach (Transform fracture in loopFractures.transform)
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("b"))
            gotHit = true;

        if(gotHit || (photonView.IsMine && transform.position.y < -40f))
        {
            photonView.RPC("Explosion", RpcTarget.All);
        }
    }

    [PunRPC]
    private void Explosion()
    {
        foreach (var fracture in fractures)
        {
            fracture.SetActive(true);
            fracture.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            fracture.transform.parent = null;
        }

        Vector3 explosionPos = transform.position;
        float radius = GetComponent<CapsuleCollider>().radius;
        var colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (var hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb)
                rb.AddExplosionForce(explosionForce, explosionPos, radius, radius / 4);
        }

        if (photonView.IsMine)
        {
            StartCoroutine(RespawnTimer());
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(5);
        GameManager.respawn = true;
    }
}
