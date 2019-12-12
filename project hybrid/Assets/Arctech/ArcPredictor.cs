using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcPredictor : MonoBehaviour
{
    // Creates a line renderer that follows a Sin() function
    // and animates it.

    public Color c1 = Color.white;
    public Color c2 = Color.white;
    public int lengthOfLineRenderer;
    public float initialUpWardSpeed = 5f;
    public float initialForwardSpeed = 1f;
    public float aimDirection = 0f;
    public Vector3 offsetPosition;

    [SerializeField] private Transform targetIndicator;
    

    void Start() {
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

    void Update() {

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var points = new Vector3[lengthOfLineRenderer];

        if (initialForwardSpeed != 0)
        {
            targetIndicator.GetComponentInChildren<MeshRenderer>().enabled = true;
            lineRenderer.enabled = true;
        }
        else
        {
            targetIndicator.GetComponentInChildren<MeshRenderer>().enabled = false;
            lineRenderer.enabled = false;   
        }

        var t = Time.time;
        float gravity = Physics.gravity.magnitude;
        Quaternion aimAngle = Quaternion.Euler(0f, aimDirection, 0f);
        for (int i = 0; i < lengthOfLineRenderer; i++) {
            points[i] = new Vector3((i * 0.1f) * initialForwardSpeed, initialUpWardSpeed * (i* 0.1f) - 0.5f * gravity * (i * 0.1f) * (i * 0.1f), 0.0f);
            points[i] = (aimAngle * points[i]) + transform.position + offsetPosition;
        }

        for (int i = 0; i < lengthOfLineRenderer - 2; i += 2) {
            Ray ray = new Ray(points[i], points[i + 2] - points[i]);
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(ray, out hit, Vector3.Distance(points[i], points[i + 2]));

            if (hit.collider != null) {
                points[i] = hit.point;
                targetIndicator.gameObject.SetActive(true);
                targetIndicator.position = hit.point + (hit.normal / 100);

                //Vector3 rot = new Vector3(hit.normal.x, hit.normal.y,0);
                targetIndicator.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);// Quaternion.Euler(hit.normal * Mathf.Rad2Deg);

                //Debug.DrawLine(targetIndicator.position, hit.normal + targetIndicator.position);

                for (int p = i; p < lengthOfLineRenderer; p++) {
                    points[p] = hit.point;
                    //points.RemoveAt(points.Count - 1);
                }
                break;
            }

            if (i == lengthOfLineRenderer - 4) {
                targetIndicator.gameObject.SetActive(false);
            }
        }
        
        lineRenderer.SetPositions(points);
    }
}