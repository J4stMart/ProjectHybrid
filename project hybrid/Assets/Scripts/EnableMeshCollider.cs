using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMeshCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BoxCollider>())
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        else if (GetComponent<MeshCollider>())
        {
            GetComponent<MeshCollider>().enabled = true;
        }

    }
}
