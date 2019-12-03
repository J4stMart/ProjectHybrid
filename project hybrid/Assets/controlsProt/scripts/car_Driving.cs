using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class car_Driving : MonoBehaviour
{
    [SerializeField]
    private float MotorForce, SteerForce, BrakeForce;
    [SerializeField]
    private WheelCollider FR_L_Wheel, FR_R_Wheel, RE_L_Wheel, RE_R_Wheel;
    [SerializeField]
    private Transform FR_L, FR_R , RE_L , RE_R;

    public bool controlable = true;

    [SerializeField] SwipeControls Input;

    //private void Start()
    //{
    //    RE_L_Wheel.ConfigureVehicleSubsteps(1, 12, 15);
    //    RE_R_Wheel.ConfigureVehicleSubsteps(1, 12, 15);
    //    FR_L_Wheel.ConfigureVehicleSubsteps(1, 12, 15);
    //    FR_R_Wheel.ConfigureVehicleSubsteps(1, 12, 15);
    //}

    void Update()
    {
        if (controlable)
        {
            float v = Input.Vertical * MotorForce;
            float h = Input.Horizontal * SteerForce;

            RE_L_Wheel.motorTorque = v;
            RE_R_Wheel.motorTorque = v;

            FR_L_Wheel.steerAngle = h;
            FR_R_Wheel.steerAngle = h;

            WheelPose(FR_L_Wheel, FR_L);
            WheelPose(FR_R_Wheel, FR_R);

            WheelPose(RE_L_Wheel, RE_L);
            WheelPose(RE_R_Wheel, RE_R);

            if (Input.Vertical == 0)
            {
                RE_L_Wheel.brakeTorque = BrakeForce;
                RE_R_Wheel.brakeTorque = BrakeForce;
            }
            else
            {
                RE_L_Wheel.brakeTorque = 0;
                RE_R_Wheel.brakeTorque = 0;
            }
        }
    }

    private void WheelPose(WheelCollider col,Transform mesh)
    {
        Vector3 pos = mesh.position;
        Quaternion qat = mesh.rotation;

        col.GetWorldPose(out pos, out qat);

        mesh.position = pos;
        mesh.rotation = qat;
    }
}
