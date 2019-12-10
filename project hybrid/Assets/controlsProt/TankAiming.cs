using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAiming : MonoBehaviour
{
    [SerializeField] private Transform trackingPosition;
    [SerializeField] private Transform TurretTransform;

    void Update() {
        TurretTransform.LookAt(trackingPosition);
    }
}
