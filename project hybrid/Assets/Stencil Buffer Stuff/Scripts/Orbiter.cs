﻿using UnityEngine;
using System.Collections;

public class Orbiter : MonoBehaviour
{
    public Transform target;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;

    void Start()
    {
        transform.position = (transform.position - target.position).normalized * radius + target.position;
        radius = 2.0f;
    }

    void Update()
    {
        transform.RotateAround(target.position, axis, rotationSpeed * Time.deltaTime);
        desiredPosition = (transform.position - target.position).normalized * radius + target.position;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }
}