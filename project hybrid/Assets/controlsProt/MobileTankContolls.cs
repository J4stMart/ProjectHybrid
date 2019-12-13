﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTankContolls : MonoBehaviour
{
    float leftAxis;
    float rightAxis;

    public float horizontal;
    public float vertical;

    TankAiming aiming;

    [Range(0, 1f), SerializeField] private float amountOfScreenUsedForControls = .3f;
    [SerializeField] bool debugControls = false;

    [SerializeField] bool raycastAiming;
    [SerializeField] Transform aimSourcePos;

    private void Awake() {
        aiming = GetComponent<TankAiming>();
    }

    void Update()
    {
        if (debugControls)
        {
            dbugControls();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            Vector2 pos = new Vector2((touch.position.x / (Screen.width / 5) * 2) - 5, (touch.position.y / Screen.height < amountOfScreenUsedForControls) ? (touch.position.y / (Screen.height / 10)) / amountOfScreenUsedForControls : 10);

            switch (touch.phase) {
                case TouchPhase.Ended:
                    setAxes(Vector2.zero);

                    if (touch.position.y / Screen.height > amountOfScreenUsedForControls) {
                        GetComponent<Tank_Fire>().shoot(aiming.aaa);
                        aiming.aaa = 0;
                    }
                    break;
                case TouchPhase.Began:

                    if (touch.position.y / Screen.height < amountOfScreenUsedForControls) {
                        setAxes(pos);
                    }
                    else {
                        raycastAim(touch.position);
                        aiming.aaa += aiming.chargeUp * Time.deltaTime;
                    }

                    break;
                case TouchPhase.Moved:
                    if (touch.position.y / Screen.height < amountOfScreenUsedForControls) {
                        setAxes(pos);
                    }
                    else {
                        raycastAim(touch.position);
                        aiming.aaa += aiming.chargeUp * Time.deltaTime;
                    }
                    break;
                case TouchPhase.Stationary:
                    if (touch.position.y / Screen.height < amountOfScreenUsedForControls) {
                        setAxes(pos);
                    }
                    else {
                        raycastAim(touch.position);
                        aiming.aaa += aiming.chargeUp * Time.deltaTime;
                    }
                    break;
                
            }
        }

        if (Input.touchCount > 1)
        {
            Touch touch = Input.touches[1];
            Vector2 pos = new Vector2((touch.position.x / (Screen.width / 5) * 2) - 5, (touch.position.y / Screen.height < amountOfScreenUsedForControls) ? (touch.position.y / (Screen.height / 10)) / amountOfScreenUsedForControls : 10);

            switch (touch.phase)
            {
                case TouchPhase.Ended:
                    setAxes(Vector2.zero);

                    if (touch.position.y / Screen.height > amountOfScreenUsedForControls)
                    {
                        GetComponent<Tank_Fire>().shoot(aiming.aaa);
                        aiming.aaa = 0;
                    }
                    break;
                case TouchPhase.Began:

                    if (touch.position.y / Screen.height < amountOfScreenUsedForControls)
                    {
                        setAxes(pos);
                    }
                    else
                    {
                        raycastAim(touch.position);
                        aiming.aaa += aiming.chargeUp * Time.deltaTime;
                    }

                    break;
                case TouchPhase.Moved:
                    if (touch.position.y / Screen.height < amountOfScreenUsedForControls)
                    {
                        setAxes(pos);
                    }
                    else
                    {

                        raycastAim(touch.position);
                        aiming.aaa += aiming.chargeUp * Time.deltaTime;
                    }
                    break;
                case TouchPhase.Stationary:
                    if (touch.position.y / Screen.height < amountOfScreenUsedForControls)
                    {
                        setAxes(pos);
                    }
                    else {
                        raycastAim(touch.position); 
                        aiming.aaa += aiming.chargeUp * Time.deltaTime;
                    }
                    break;

            }
        }
    }

    //void posToTankTracks(Vector2 pos)
    //{
    //    leftAxis = pos.y / 10;
    //    rightAxis = pos.y / 10;

    //    if (pos.x < 0)
    //    {
    //        leftAxis -= (-pos.x / 5);
    //    }
    //    if (pos.x > 0)
    //    {
    //        rightAxis -= (pos.x / 5);
    //    }
    //}

    void setAxes(Vector2 pos)
    {
        horizontal = pos.x / 5;
        vertical = pos.y / 10;
    }

    void dbugControls()
    {
        Vector2 pos = new Vector2();

        if (Input.GetKey(KeyCode.W))
        {
            pos.y = 4;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.x = -4;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x = 4;
        }

        setAxes(pos);
    }

    void raycastAim(Vector2 pos) {
        if (raycastAiming) {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit = new RaycastHit();
            Debug.Log("aa");
            if (Physics.Raycast(ray, out hit, 500)) {
                Debug.Log("hit");
                aimSourcePos.position = transform.position - (hit.point - transform.position);
            }
        }
    }
}