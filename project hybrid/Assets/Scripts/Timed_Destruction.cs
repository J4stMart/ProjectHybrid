using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timed_Destruction : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 5);
    }
}
