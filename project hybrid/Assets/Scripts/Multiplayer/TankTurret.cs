using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TankTurret : MonoBehaviourPun
{
    [SerializeField]
    private GameObject shellPrefab;

    private Transform trackingPosition;

    [SerializeField] private Transform shootPosition;
    [SerializeField] private ArcPredictor arc;

    [SerializeField]
    private int shootForce = 105;
    [SerializeField]
    private float chargeUpSpeed = 15;
    private float chargeUp = 0;

    private InputManager inputManager;

    private bool isShooting = false;

    private void Start()
    {
        if(photonView.IsMine)
        {
            inputManager.startShooting += new InputManager.StartShooting(StartShooting);
            inputManager.endShooting += new InputManager.EndShooting(EndShooting);
        }
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;

        //temp input for shooting
        if (isShooting)
        {
            chargeUp += chargeUpSpeed * Time.deltaTime;
        }

        Vector3 dir = -(trackingPosition.position - transform.position);
        transform.rotation = Quaternion.LookRotation(dir, transform.up);
        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);

        Vector3 arcDir = shootPosition.position - transform.position;
        Vector3 horizontal = new Vector3(arcDir.x, 0, arcDir.z);
        arc.initialUpWardSpeed = Mathf.Sin(Vector3.Angle(arcDir, horizontal) * Mathf.Deg2Rad) * chargeUp;
        arc.initialForwardSpeed = Mathf.Cos(Vector3.Angle(arcDir, horizontal) * Mathf.Deg2Rad) * chargeUp;

        arc.aimDirection = transform.rotation.eulerAngles.y - 90;

        arc.offsetPosition = shootPosition.position - transform.position;
    }

    public void Shoot(float Distance)
    {
        var shell = PhotonNetwork.Instantiate(shellPrefab.name, shootPosition.position, shootPosition.rotation);
        // per unit of distance 105 units of force

        shell.GetComponent<Rigidbody>().AddForce(shell.transform.up * (Distance * shootForce));
    }

    public void SetVariables(Transform camera, InputManager input)
    {
        trackingPosition = camera;
        inputManager = input;
    }

    public void StartShooting()
    {
        isShooting = true;
    }

    public void EndShooting()
    {
        isShooting = false;
        Shoot(chargeUp);
        chargeUp = 0;
    }
}
