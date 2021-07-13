using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOriginalMode : MonoBehaviour
{
    private Rigidbody2D rb;
    private Ship player;

    void Awake()
    {
        player = GetComponent<Ship>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            player.enabled = true;
        }
    }
}