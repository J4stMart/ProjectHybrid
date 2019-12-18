using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(AudioSource))]
public class TankTurret : MonoBehaviourPun
{
    [SerializeField]
    private GameObject shellPrefab;

    private Transform trackingPosition;

    [SerializeField] private Transform loopPivot;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float rotationspeed = 1f;
    private ArcPredictor arc;

    [SerializeField]
    private float chargeUpSpeed = 15;
    [SerializeField]
    private float startChargeUp = 3f;
    private float chargeUp = 0;
    private float reloadTime = 1.5f;
    private bool canShoot = true;

    [SerializeField]
    private Color arcColor1 = Color.white;
    [SerializeField]
    private Color arcColor2 = Color.white;

    [SerializeField]
    private ParticleSystem nozzleflash;

    private InputManager inputManager;

    private bool isShooting = false;

    private AudioSource audioSource;
    private AudioClip chargingSound, reloadSound;
    private bool canPlayCharge = true;

    private void Start()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            inputManager.startShooting += StartShooting;
            inputManager.endShooting += EndShooting;
            inputManager.tankTransform = transform;

            arc = gameObject.AddComponent<ArcPredictor>();
            arc.c1 = arcColor1;
            arc.c2 = arcColor2;

            chargeUp = startChargeUp;
            trackingPosition = GameObject.FindGameObjectWithTag("AimingSource").transform;
        }
        audioSource = GetComponent<AudioSource>();
        nozzleflash.Pause();
    }

    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            inputManager.startShooting -= StartShooting;
            inputManager.endShooting -= EndShooting;
        }
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;

        //temp input for shooting
        if (isShooting && canShoot)
        {
            chargeUp += chargeUpSpeed * Time.deltaTime;

            if (canPlayCharge)
            {
                canPlayCharge = false;
                photonView.RPC("PlayChargingAudio", RpcTarget.MasterClient);
            }
        }

        if (chargeUp > (1.5f * chargeUp * reloadTime))
        {
            EndShooting();
        }

        Vector3 dir = -(trackingPosition.position - transform.position);

        float offset = (transform.rotation.eulerAngles.y - loopPivot.rotation.eulerAngles.y) % 360;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(0, -offset, 0f), rotationspeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);

        loopPivot.rotation = Quaternion.Slerp(loopPivot.rotation, Quaternion.LookRotation(dir, transform.up), 1.5f * rotationspeed * Time.deltaTime);
        float loopRotatieAngle = Mathf.Clamp((loopPivot.localRotation.eulerAngles.x + 180f) % 360, 140f, 220f);
        loopPivot.localRotation = Quaternion.Euler(loopRotatieAngle + 180, 0, 0);

        Vector3 arcDir = shootPosition.up;
        Vector3 horizontal = new Vector3(arcDir.x, 0, arcDir.z);
        arc.initialUpWardSpeed = shootPosition.up.y * chargeUp;
        arc.initialForwardSpeed = new Vector3(shootPosition.up.x, 0, shootPosition.up.z).magnitude * chargeUp;

        arc.aimDirection = transform.rotation.eulerAngles.y - 90;

        arc.offsetPosition = shootPosition.position - transform.position;
    }

    public void Shoot(float Distance)
    {
        var shell = PhotonNetwork.Instantiate(shellPrefab.name, shootPosition.position, shootPosition.rotation);

        shell.GetComponent<Rigidbody>().velocity = shootPosition.up * chargeUp;
    }

    public void SetVariables(Transform camera, InputManager input)
    {
        trackingPosition = camera;
        inputManager = input;
    }

    public void SetAudio(AudioClip charge, AudioClip reload)
    {
        chargingSound = charge;
        reloadSound = reload;
    }

    public void StartShooting()
    {
        isShooting = true;
    }

    public void EndShooting()
    {
        if (canShoot)
        {
            audioSource.Stop();
            canPlayCharge = true;

            foreach (var renderer in arc.targetIndicator.GetComponentsInChildren<MeshRenderer>())
                renderer.enabled = false;
            arc.enabled = false;
            arc.lineRenderer.enabled = false;

            canShoot = false;
            isShooting = false;
            Shoot(chargeUp);
            chargeUp = startChargeUp;

            StartCoroutine(Shooting());
        }
    }

    private IEnumerator Shooting()
    {
        photonView.RPC("PlayNozzleFlash", RpcTarget.All);
        yield return new WaitForSeconds(reloadTime - 0.2f);
        photonView.RPC("PlayReloadAudio", RpcTarget.MasterClient);
        yield return new WaitForSeconds(0.2f);
        arc.enabled = true;
        yield return new WaitForSeconds(0.1f);
        foreach (var renderer in arc.targetIndicator.GetComponentsInChildren<MeshRenderer>())
            renderer.enabled = false;
        canShoot = true;
    }

    [PunRPC]
    void PlayNozzleFlash()
    {
        nozzleflash.Play();
    }

    [PunRPC]
    void PlayChargingAudio()
    {
        audioSource.PlayOneShot(chargingSound, 1f);
    }

    [PunRPC]
    void PlayReloadAudio()
    {
        audioSource.PlayOneShot(reloadSound, 1f);
    }
}
