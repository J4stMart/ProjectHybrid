using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class TankMultiplayer : MonoBehaviourPun
{
    [SerializeField]
    private float tankSpeed = 15f;
    [SerializeField]
    private float tankRotationSpeed = 20f;

    private InputManager controls;
    private Rigidbody rb;

    [HideInInspector]
    public static GameObject localPlayerInstance = null;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            localPlayerInstance = this.gameObject;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;

        if (rb)
        {
            HandleMovement();
        }
    }

    protected virtual void HandleMovement()
    {
        Vector3 wantedPosition = transform.position + (transform.forward * controls.Vertical * tankSpeed * Time.deltaTime);
        rb.MovePosition(wantedPosition);

        Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (tankRotationSpeed * controls.Horizontal * Time.deltaTime));
        rb.MoveRotation(wantedRotation);
    }

    public void SetInputManager(InputManager im)
    {
        controls = im;
    }
}
