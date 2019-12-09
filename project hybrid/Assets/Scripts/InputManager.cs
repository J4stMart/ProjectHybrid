using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Range(0, 1f), SerializeField]
    private float amountOfScreenUsedForControls = 3;
    [SerializeField]
    private bool debug = false;

    private float horizontal;
    private float vertical;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            DebugControls();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            Vector2 pos = new Vector2((touch.position.x / (Screen.width / 5) * 2) - 5, (touch.position.y / Screen.height < amountOfScreenUsedForControls) ? (touch.position.y / (Screen.height / 10)) / amountOfScreenUsedForControls : 10);
            Debug.Log(pos);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    setAxes(pos);
                    break;
                case TouchPhase.Moved:
                    setAxes(pos);
                    break;
                case TouchPhase.Ended:
                    setAxes(Vector2.zero);
                    break;
            }
        }
    }

    private void setAxes(Vector2 pos)
    {
        horizontal = pos.x / 5;
        vertical = pos.y / 10;
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
            pos.x = 4;
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
}
