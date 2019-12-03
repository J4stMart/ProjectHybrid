using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(TANK_INPUT_SCRIPT))]
public class Tank_Controller : MonoBehaviour
{
    [SerializeField]
    private float tankSpeed = 15f;
    [SerializeField]
    private float tankRotationSpeed = 20f;

    private Rigidbody rb;
    //private TANK_INPUT_SCRIPT input;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //input = GetComponent<TANK_INPUT_SCRIPT>();
    }

    void FixedUpdate()
    {
        if(rb /* && input*/)
        {
            HandleMovement();
        }
    }

    protected virtual void HandleMovement()
    {
        Vector3 wantedPosition = transform.position + (transform.forward * Input.GetAxis("Vertical") * tankSpeed * Time.deltaTime);
        rb.MovePosition(wantedPosition);

        Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (tankRotationSpeed * Input.GetAxis("Horizontal") * Time.deltaTime));
        rb.MoveRotation(wantedRotation);

       /* Vector3 wantedPosition = transform.position + ( transform.forward + input.ForwardInput * tankSpeed * Time.deltaTime);
        rb.MovePosition(wantedPosition);

        Vector3 wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * ( tankRotationSpeed * input.RotationInput * Time.deltaTime));
        rb.MovePosition(wantedRotation);*/
    }
}
