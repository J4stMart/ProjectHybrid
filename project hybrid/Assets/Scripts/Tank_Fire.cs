using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Fire : MonoBehaviour
{
    [SerializeField]
    private GameObject shellPrefab;

    [SerializeField]
    private GameObject turretGameObject;

    [SerializeField]
    private int force;

    [SerializeField]
    Transform spawnLocation;

    public void shoot(float Distance){
        GameObject shell = Instantiate(shellPrefab, spawnLocation.position, spawnLocation.rotation);
        // per unit of distance 105 units of force
        Debug.Log(Distance);
        shell.GetComponent<Rigidbody>().AddForce(shell.transform.up * (Distance * force));
    }
}
