using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTankContolls : MonoBehaviour
{
    float leftAxis;
    float rightAxis;

    public float horizontal;
    public float vertical;

    [Range(0, 10f), SerializeField] private float amountOfScreenUsedForControls = 3;
    [SerializeField] bool debugControls = false;

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

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    setAxes(pos);
                    break;
                case TouchPhase.Moved:
                    setAxes(pos);
                    break;
                case TouchPhase.Stationary:
                    setAxes(pos);
                    break;
                case TouchPhase.Ended:
                    setAxes(Vector2.zero);
                    break;
            }
        }
    }

    void posToTankTracks(Vector2 pos)
    {
        leftAxis = pos.y / 10;
        rightAxis = pos.y / 10;

        if (pos.x < 0)
        {
            leftAxis -= (-pos.x / 5);
        }
        if (pos.x > 0)
        {
            rightAxis -= (pos.x / 5);
        }
    }

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
}
