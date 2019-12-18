using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //[Range(0, 1f), SerializeField]
    private float amountOfScreenUsedForControls = .42f;
    [SerializeField]
    private bool debug = false;

    private float horizontal;
    private float vertical;

    [SerializeField]
    private bool raycastAiming;
    [SerializeField]
    private bool aimByPointingCamera;

    private Transform aimSourcePos;
    private Transform aimingTarget;
    private LayerMask raycastLayerMask;
    [SerializeField] private InputUi inputUi;

    public Transform arCamera;
    public Transform tankTransform = null;

    [HideInInspector]
    public delegate void StartShooting();
    [HideInInspector]
    public event StartShooting startShooting;

    [HideInInspector]
    public delegate void EndShooting();
    [HideInInspector]
    public event EndShooting endShooting;

    // Start is called before the first frame update
    void Start()
    {
        raycastLayerMask = LayerMask.GetMask("Level", "AimCatcher");
        aimSourcePos = GameObject.FindGameObjectWithTag("AimingSource").transform;
        aimingTarget = GameObject.FindGameObjectWithTag("TargetGreen").transform;

        if (GameObject.FindGameObjectWithTag("UI").GetComponent<InputUi>() != null)
        {
            inputUi = GameObject.FindGameObjectWithTag("UI").GetComponent<InputUi>();
        }
        else
        {
            Debug.LogError("de ui met scrit inputUI heeft geet tag 'UI'");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (aimByPointingCamera)
        {
            raycastAim(Vector2.zero);
        }

        if (tankTransform == null)
            return;

        if (debug)
        {
            DebugControls();
        }

        inputUi.setControlPad(inputUi.centerstartpos, false);

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.touches[0];
            Vector2 pos = new Vector2((touch.position.x / (Screen.width / 5) * 2) - 5, (touch.position.y / Screen.height < amountOfScreenUsedForControls) ? ((touch.position.y / (Screen.height / 10)) - .6f) / amountOfScreenUsedForControls : 10);

            if (touch.position.y / Screen.height < amountOfScreenUsedForControls)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        setAxes(pos);
                        inputUi.setControlPad(touch.position, true);
                        break;
                    case TouchPhase.Moved:
                        setAxes(pos);
                        inputUi.setControlPad(touch.position, true);
                        break;
                    case TouchPhase.Ended:
                        setAxes(Vector2.zero);
                        break;
                    case TouchPhase.Stationary:
                        setAxes(pos);
                        inputUi.setControlPad(touch.position, true);
                        //if (debug)
                        //    setAxes(pos);
                        break;
                }

            }
            else
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (startShooting != null)
                            startShooting();
                        break;
                    case TouchPhase.Ended:
                        if (endShooting != null)
                            endShooting();
                        break;
                }
            }

        }
    }

    private void setAxes(Vector2 pos)
    {
        Vector2 temPos = new Vector2(pos.x / 5, pos.y / 7);
        if (temPos.magnitude > 1)
        {
            temPos.Normalize();
        }
        horizontal = temPos.x;
        vertical = temPos.y;
    }

    private void DebugControls()
    {
        Vector2 pos = new Vector2();

        if (Input.GetKey(KeyCode.W))
        {
            pos.y = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.x = -4;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x = 10;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            startShooting();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            endShooting();
        }

        setAxes(pos);
    }

    public float Horizontal
    {
        get { return horizontal; }
    }

    public float Vertical
    {
        get { return vertical; }
    }

    void raycastAim(Vector2 pos)
    {
        if (raycastAiming)
        {
            Ray ray;
            if (aimByPointingCamera)
            {
                if (arCamera == null)
                {
                    arCamera = Camera.main.transform;
                }

                ray = new Ray(arCamera.position, arCamera.forward);
                Debug.DrawLine(Camera.main.transform.position + Vector3.down, Camera.main.transform.position + Camera.main.transform.forward * 500, Color.green);
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(pos);
            }

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 1300, raycastLayerMask))
            {
                aimingTarget.position = hit.point + (hit.normal / 100);
                aimingTarget.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                if (tankTransform != null)
                    aimSourcePos.position = tankTransform.position - (hit.point - tankTransform.position);
            }
        }
    }
}
