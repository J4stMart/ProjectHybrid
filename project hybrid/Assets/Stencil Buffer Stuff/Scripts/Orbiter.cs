using UnityEngine;
using System.Collections;

public class Orbiter : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.RotateAround(target.transform.position, target.transform.up, 100 * Time.deltaTime);
    }
}