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


    [SerializeField] bool UseCameraToAim = true;

    [HideInInspector]public float aaa;
    public float chargeUp = 15;

    private void Awake()
    {
        aaa = startFirepower;
        if (UseCameraToAim)
        {
            trackingPosition = GameObject.FindWithTag("MainCamera").transform;
        }
        else
        {
            trackingPosition = GameObject.FindGameObjectWithTag("AimingSource").transform;
        }
    }

    void Update() {

        //temp input for shooting
        if (Input.GetKey(KeyCode.Space))
        {
            aaa += chargeUp * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Tank_Fire>().shoot(aaa);
            aaa = startFirepower;
        }



        Vector3 dir = -(trackingPosition.position - transform.position);
               
        float offset = (TurretTransform.rotation.eulerAngles.y - loopPivot.rotation.eulerAngles.y) % 360;

        TurretTransform.rotation = Quaternion.Slerp(TurretTransform.rotation, Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(0, -offset, 0f), rotationspeed * Time.deltaTime);
        TurretTransform.localRotation = Quaternion.Euler(0, TurretTransform.localRotation.eulerAngles.y, 0);

        loopPivot.rotation = Quaternion.Slerp(loopPivot.rotation, Quaternion.LookRotation(dir, transform.up), rotationspeed * Time.deltaTime);
        float loopRotatieAngle = Mathf.Clamp((loopPivot.localRotation.eulerAngles.x + 180f) % 360, 140f, 220f);
        loopPivot.localRotation = Quaternion.Euler(loopRotatieAngle + 180, 0, 0);




        Vector3 arcDir = arcStartPos.up;
        Vector3 horizontal = new Vector3(arcDir.x, 0, arcDir.z);
        arc.initialUpWardSpeed = arcStartPos.up.y * aaa;
        arc.initialForwardSpeed = new Vector3(arcStartPos.up.x, 0, arcStartPos.up.z).magnitude * aaa;
        //arc.initialUpWardSpeed = Mathf.Sin(Vector3.Angle(arcDir, horizontal) * Mathf.Deg2Rad) * aaa;
        //arc.initialForwardSpeed = Mathf.Cos(Vector3.Angle(arcDir, horizontal) * Mathf.Deg2Rad) * aaa;
        Debug.Log(Vector3.Angle(arcDir, horizontal));

        arc.aimDirection = TurretTransform.rotation.eulerAngles.y - 90;

        arc.offsetPosition = arcStartPos.position - transform.position;// transform.up * arc.startHeight;

        //arc.offsetRotation = Quaternion.Euler(arcDir);

        //arc.initialUpWardSpeed = TurretTransform.rotation.eulerAngles.x + upwardArcOffset;
    }

    //Vector2 posOnCircle(int i, int totalPosses, float radius)
    //{
    //    float dir = ((2 * Mathf.PI) / totalPosses) * i;
    //    return new Vector2(Mathf.Cos(dir) * radius, Mathf.Sin(dir) * radius);
    //}

    //Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angles)
    //{
    //    Vector3 dir = point - pivot; // get point direction relative to pivot
    //    dir = angles * dir; // rotate it
    //    point = dir + pivot; // calculate rotated point
    //    return point; // return it
    //}
}