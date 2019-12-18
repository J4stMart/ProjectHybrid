using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankBeakeon : MonoBehaviour
{
    [SerializeField] float startheight = 30;
    [SerializeField] public float endheight = 100;

    LineRenderer LineRenderer;

    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position + Vector3.up * startheight;
        positions[1] = transform.position + Vector3.up * endheight;
        LineRenderer.SetPositions(positions);
    }
}