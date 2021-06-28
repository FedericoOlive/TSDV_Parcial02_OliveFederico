using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    private Rigidbody2D player;
    public Vector2 playerVel;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        playerVel = player.velocity;
    }
    void Update()
    {
        transform.position += new Vector3(playerVel.x, playerVel.y, transform.position.z);
    }
}