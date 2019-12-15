using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAiming : MonoBehaviour
{
    private Transform trackingPosition;
    [SerializeField] private Transform TurretTransform;
    [SerializeField] private ArcPredictor arc;
    [SerializeField] Transform arcStartPos;
    public float rotationspeed = 1f;
    public float startFirepower = 3f;
    public Transform loopPivot;
    private float reloadTime = 1.5f;
    public ParticleSystem nozzleflash;
    private bool canShoot = true;
    private AudioSource audioSource;
    public AudioClip chargingSound;
    public AudioClip reloadSound;
    private bool canPlayCharge = true;
    public bool shootInput = false;
    public bool shootInputEnd = false;

    [HideInInspector]public float aaa;
    public float chargeUp = 15;

    private void Awake()
    {
        shootInput = false;
        shootInputEnd = false;
        audioSource = GetComponent<AudioSource>();
        canShoot = true;
        aaa = startFirepower;
        trackingPosition = GameObject.FindGameObjectWithTag("AimingSource").transform;
        nozzleflash.Pause();
    }

    void Update() {
        //temp input for shooting
        if (shootInput && canShoot) {
            aaa += chargeUp * Time.deltaTime;
            if(canPlayCharge) {
                canPlayCharge = false;
                audioSource.PlayOneShot(chargingSound, 1f);
            }
        }

        if ((shootInputEnd && canShoot) || (aaa > (chargeUp * reloadTime) && canShoot)) {
            audioSource.Stop();
            canPlayCharge = true;
            arc.targetIndicator.gameObject.SetActive(false); 
            arc.enabled = false;
            GetComponent<LineRenderer>().enabled = false;
            canShoot = false;
            GetComponent<Tank_Fire>().shoot(aaa);
            aaa = startFirepower;
            StartCoroutine(Shooting());
        }

        Vector3 dir = -(trackingPosition.position - transform.position);
               
        float offset = (TurretTransform.rotation.eulerAngles.y - loopPivot.rotation.eulerAngles.y) % 360;

        TurretTransform.rotation = Quaternion.Slerp(TurretTransform.rotation, Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(0, -offset, 0f), rotationspeed * Time.deltaTime);
        TurretTransform.localRotation = Quaternion.Euler(0, TurretTransform.localRotation.eulerAngles.y, 0);

        loopPivot.rotation = Quaternion.Slerp(loopPivot.rotation, Quaternion.LookRotation(dir, transform.up), 1.5f* rotationspeed * Time.deltaTime);
        float loopRotatieAngle = Mathf.Clamp((loopPivot.localRotation.eulerAngles.x + 180f) % 360, 140f, 220f);
        loopPivot.localRotation = Quaternion.Euler(loopRotatieAngle + 180, 0, 0);

        Vector3 arcDir = arcStartPos.up;
        Vector3 horizontal = new Vector3(arcDir.x, 0, arcDir.z);
        arc.initialUpWardSpeed = arcStartPos.up.y * aaa;
        arc.initialForwardSpeed = new Vector3(arcStartPos.up.x, 0, arcStartPos.up.z).magnitude * aaa;

        arc.aimDirection = TurretTransform.rotation.eulerAngles.y - 90;

        arc.offsetPosition = arcStartPos.position - transform.position;// transform.up * arc.startHeight;

        //arc.offsetRotation = Quaternion.Euler(arcDir);

        //arc.initialUpWardSpeed = TurretTransform.rotation.eulerAngles.x + upwardArcOffset;
    }

    IEnumerator Shooting() {
        nozzleflash.Play();
        yield return new WaitForSeconds(reloadTime - 0.2f);
        audioSource.PlayOneShot(reloadSound, 1f);
        yield return new WaitForSeconds(0.2f);
        arc.enabled = true;
        yield return new WaitForSeconds(0.1f);
        arc.targetIndicator.gameObject.SetActive(true);
        canShoot = true;
    }
}