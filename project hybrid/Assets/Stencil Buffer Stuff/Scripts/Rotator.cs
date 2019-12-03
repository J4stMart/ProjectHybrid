using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public float speed = 10f;


    void Update()
    {
        transform.Rotate(Vector3.forward + Vector3.up + Vector3.right, speed * Time.deltaTime);
    }
}
