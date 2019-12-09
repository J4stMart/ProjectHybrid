using UnityEngine;
using System.Collections;

public class ArcPredictor : MonoBehaviour
{
    // Creates a line renderer that follows a Sin() function
    // and animates it.

    public Color c1 = Color.white;
    public Color c2 = Color.white;
    public int lengthOfLineRenderer = 20;
    public float startHeight = 1f;
    public float initialUpWardSpeed = 5f;
    public float initialForwardSpeed = 1f;
    public float aimDirection = 0f;

    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.positionCount = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var points = new Vector3[lengthOfLineRenderer];
        var t = Time.time;
        float gravity = Physics.gravity.magnitude;
        Quaternion aimAngle = Quaternion.Euler(0f, aimDirection, 0f);
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            points[i] = new Vector3(i * initialForwardSpeed, startHeight + initialUpWardSpeed * (i* 0.1f) - 0.5f * gravity * (i * 0.1f) * (i * 0.1f), 0.0f);
            points[i] = aimAngle * points[i];
        }
        lineRenderer.SetPositions(points);
    }
}