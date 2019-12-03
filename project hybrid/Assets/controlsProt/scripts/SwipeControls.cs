using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControls : MonoBehaviour
{

    [SerializeField]private bool debugControls;

    public float Vertical;
    public float Horizontal;

    [SerializeField] private float sensitivity;

    Vector2 swipeStartPos;

    private void Update() {
        

        if (Input.touchCount > 0) {
            Vertical = 1;

            Touch touch = Input.touches[0];
            switch (touch.phase) {
                case TouchPhase.Began:
                    swipeStartPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    Horizontal = (touch.position.x - swipeStartPos.x) * sensitivity;
                    break;
                case TouchPhase.Ended:
                    Horizontal = 0;
                    break;
            }
        }
        else {
            Vertical = 0;
        }

        if (debugControls) {
            arrows();
        }
    }

    void arrows() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            Horizontal = -.5f;
            Vertical = 1;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            Horizontal = .5f;
            Vertical = 1;
        }
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) {
            Vertical = 0;
        }
    }
}