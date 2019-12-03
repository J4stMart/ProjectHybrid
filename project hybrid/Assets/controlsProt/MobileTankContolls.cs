using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTankContolls : MonoBehaviour
{
    public float leftAxis;
    public float rightAxis;

    [Range(0,1f),SerializeField] private float amountOfScreenUsedForControls = 3;

    void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.touches[0];
            Vector2 pos = new Vector2((touch.position.x / (Screen.width / 5) * 2) - 5, (touch.position.y / Screen.height < amountOfScreenUsedForControls) ? (touch.position.y / (Screen.height / 10))/amountOfScreenUsedForControls : 10);

            switch (touch.phase) {
                case TouchPhase.Began:
                    posToTankTracks(pos);
                    break;
                case TouchPhase.Moved:
                    posToTankTracks(pos);
                    break;
                case TouchPhase.Ended:
                    posToTankTracks(pos);
                    break;
            }
        }
    }

    void posToTankTracks(Vector2 pos) {
        leftAxis = pos.y / 10;
        rightAxis = pos.y/ 10;

        if (pos.x < 0) {
            leftAxis -= (-pos.x / 5);
        }
        if (pos.x > 0) {
            rightAxis -= (pos.x / 5);
        }
    }
}