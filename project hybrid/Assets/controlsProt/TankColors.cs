using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankColors : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    [SerializeField] private Color[] tank0Colors;
    [SerializeField] private Color[] tank1Colors;
    [SerializeField] private Color[] tank2Colors;
    [SerializeField] private Color[] tank3Colors;

    public int tank;

    private void Update()
    {
        setColor(tank);
    }

    void setColor(int tank) {
        for (int i = 0; i < materials.Length; i++) {
            switch (tank)
            {
                case 0:
                    materials[i].color = tank0Colors[i];
                    break;
                case 1:
                    materials[i].color = tank1Colors[i];
                    break;
                case 2:
                    materials[i].color = tank2Colors[i];
                    break;
                case 3:
                    materials[i].color = tank3Colors[i];
                    break;
            }
        }
    }
}