using Photon.Pun;
using UnityEngine;

public class CarMultiplayer : MonoBehaviourPun
{
    [SerializeField]
    private float MotorForce, SteerForce, BrakeForce;
    [SerializeField]
    private WheelCollider FR_L_Wheel, FR_R_Wheel, RE_L_Wheel, RE_R_Wheel;
    [SerializeField]
    private Transform FR_L, FR_R , RE_L , RE_R;

    public bool controlable = true;

    public static GameObject localPlayerInstance;

    private SwipeControls input;

    //private void Start()
    //{
    //    RE_L_Wheel.ConfigureVehicleSubsteps(1, 12, 15);
    //    RE_R_Wheel.ConfigureVehicleSubsteps(1, 12, 15);
    //    FR_L_Wheel.ConfigureVehicleSubsteps(1, 12, 15);
    //    FR_R_Wheel.ConfigureVehicleSubsteps(1, 12, 15);
    //}

    private void Awake()
    {
        if(photonView.IsMine)
        {
            CarMultiplayer.localPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        input = camera.GetComponent<SwipeControls>();

        if(photonView.IsMine)
        {
            camera.GetComponent<Movement>().LookAt(transform);
        }
    }

    void Update()
    {
        if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (controlable)
        {
            float v = input.Vertical * MotorForce;
            float h = input.Horizontal * SteerForce;

            RE_L_Wheel.motorTorque = v;
            RE_R_Wheel.motorTorque = v;

            FR_L_Wheel.steerAngle = h;
            FR_R_Wheel.steerAngle = h;

            WheelPose(FR_L_Wheel, FR_L);
            WheelPose(FR_R_Wheel, FR_R);

            WheelPose(RE_L_Wheel, RE_L);
            WheelPose(RE_R_Wheel, RE_R);

            if (input.Vertical == 0)
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
