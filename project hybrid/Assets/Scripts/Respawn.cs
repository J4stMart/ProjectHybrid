using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject tank;
    public GameObject tankPrefab;
    [SerializeField] GameObject spawnTargetPrefab;
    Transform spawntarget;
    //public Transform target;
    private bool isDestroyed = false;
    private LayerMask raycastLayerMask;


    void Start()
    {
        tank = GameObject.FindWithTag("TankRed");
        raycastLayerMask = LayerMask.GetMask("Level");

        spawntarget = GameObject.Instantiate(spawnTargetPrefab, transform).transform;
        
    }
    
    void Update()
    {
        if (tank)
        {
            gameObject.GetComponent<LineRenderer>().enabled = false;
            spawntarget.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        else
        {
            Respawner();
        }          
    }

    private void Respawner()
    {



        //gameObject.GetComponent<LineRenderer>().enabled = true;
        //Vector3 laserSpawn = transform.position - transform.up;
        //Vector3 laserAim = transform.forward;
        //LineRenderer LaserLineRenderer = gameObject.GetComponent<LineRenderer>();

        //LaserLineRenderer.SetPosition(0, laserSpawn);

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, raycastLayerMask))
        {
            spawntarget.GetComponentInChildren<MeshRenderer>().enabled = true;
            spawntarget.position = hit.point + (hit.normal / 100);
            spawntarget.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            //LaserLineRenderer.SetPosition(1, hit.point);
            //LaserLineRenderer.endColor = Color.green;
            //LaserLineRenderer.startColor = Color.green;
            if (Input.GetKey("v") || Input.touchCount > 0)
            {
                tank = Instantiate(tankPrefab, hit.point + new Vector3(0f, 20f,0f), Quaternion.identity);
            }
        }
        else
        {
            spawntarget.GetComponentInChildren<MeshRenderer>().enabled = false;

            //LaserLineRenderer.SetPosition(1, transform.TransformDirection(Vector3.forward) * 1000);
            //LaserLineRenderer.endColor = Color.red;
            //LaserLineRenderer.startColor = Color.red;
        }
    }

}
