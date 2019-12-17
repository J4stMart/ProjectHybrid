using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticScript : MonoBehaviour
{
    public Sprite[] staticSprites;
    [SerializeField] private float fps = 10.0f;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        int index = (int)(Time.time * fps);
        index = index % staticSprites.Length;
        sr.sprite = staticSprites[index];
    }
}
