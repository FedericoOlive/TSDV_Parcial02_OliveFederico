using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private Rigidbody2D player;
    public Vector2 playerVel;
    public GameObject[] backGroundMoon;
    private float backGroundMoonWidth = 81.3f;
    private int playerOverIndex = 0;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }
    void Update()
    {

    }
}