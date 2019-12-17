using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUi : MonoBehaviour
{
    [SerializeField] private float maxtime;

    [SerializeField] private Image timeSlider;
    [SerializeField] private Text timeText;

    [SerializeField] private Image controlPadCenter;
    [SerializeField] private Image controlPadEdge;

    [SerializeField] private Sprite edge1;
    [SerializeField] private Sprite edge2;

    [HideInInspector] public Vector2 centerstartpos;

    private void Awake() {
        centerstartpos = controlPadCenter.rectTransform.position;
    }

    //void Update() {
    //    setTime(time);
    //    setControlPad(centerstartpos, false);

    //    if (Input.touchCount > 0) {
    //        Touch touch = Input.touches[0];
    //        setControlPad(touch.position, true);
    //    }   
    //}

    public void setTime(float time) {
        if (time != 0) {
            timeSlider.rectTransform.position = new Vector3(-.5f * Screen.width + (Screen.width * time), timeSlider.rectTransform.position.y, timeSlider.rectTransform.position.z);
        }
        else {
            timeSlider.rectTransform.position = new Vector3(-Screen.width, timeSlider.rectTransform.position.y, timeSlider.rectTransform.position.z);
        }
        
        timeText.text = ((int)(maxtime * time)).ToString() + " / " + maxtime.ToString();
    }

    public void setControlPad(Vector2 pos, bool isUsed) {
        if (pos.y < Screen.height * .3f) {
            controlPadCenter.rectTransform.position = pos;
        }
        else {
            controlPadCenter.rectTransform.position = new Vector2(pos.x, Screen.height * .3f);
        }

        if (isUsed) {
            controlPadEdge.sprite = edge1;
        }
        else {
            controlPadEdge.sprite = edge2;
        }
        controlPadEdge.rectTransform.rotation = Quaternion.Euler(0,0,(Mathf.Atan2(controlPadCenter.rectTransform.position.x - controlPadEdge.rectTransform.position.x, controlPadCenter.rectTransform.position.y - controlPadEdge.rectTransform.position.y) * -Mathf.Rad2Deg));
    }
}