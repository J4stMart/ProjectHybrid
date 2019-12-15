﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Fire : MonoBehaviour
{
    [SerializeField]
    private GameObject shellPrefab;

    [SerializeField]
    private GameObject turretGameObject;
    
    [SerializeField]
    Transform spawnLocation;
    [SerializeField] private TankAiming aim;
    
    public void shoot(float Distance){
        GameObject shell = Instantiate(shellPrefab, spawnLocation.position, spawnLocation.rotation);
        // per unit of distance 105 units of force
        Debug.Log(spawnLocation.up);
        shell.GetComponent<Rigidbody>().velocity = spawnLocation.up * aim.aaa;

    }
}
