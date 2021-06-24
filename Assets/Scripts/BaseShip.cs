using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShip : MonoBehaviour
{
    [Serializable] private class PlayerPhysics
    {
        public float mass;
        public float linearDrag;
        public float angularDrag;
        public float gravityScale;
    }
    [Serializable] public class PlayerInputs
    {
        public bool defaultValue = true;
        public KeyCode propulsor = KeyCode.A;
        public KeyCode rotateLeft = KeyCode.A;
        public KeyCode rotateRight = KeyCode.A;

        public void SetDefaultValues()
        {
            propulsor = KeyCode.W;
            rotateLeft = KeyCode.A;
            rotateRight = KeyCode.D;
        }
    }

    [SerializeField] private PlayerPhysics playerPhisics;
    [SerializeField] private PlayerInputs playerInputs;

    private Rigidbody2D rb;
    public enum Dificult{ Easy, Normal, Hard, Extreme }
    public Dificult dificult;
    public float propulsorForce;
    public float rotateForce;

    public float fuel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (playerInputs.defaultValue)
        {
            playerInputs.SetDefaultValues();
        }

        TransferPhysicsProperties();
    }

    void Update()
    {
        PlayerInput();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision");
    }

    void PlayerInput()
    {
        if (Input.GetKey(playerInputs.propulsor))
        {
            rb.AddForce(transform.up * propulsorForce);
        }
        if (Input.GetKey(playerInputs.rotateLeft))
        {
            rb.AddTorque(rotateForce);
        }
        if (Input.GetKey(playerInputs.rotateRight))
        {
            rb.AddTorque(-rotateForce);
        }
    }

    private void TransferPhysicsProperties()
    {
        rb.mass = playerPhisics.mass;
        rb.drag = playerPhisics.linearDrag;
        rb.angularDrag = playerPhisics.angularDrag;
        rb.gravityScale = playerPhisics.gravityScale;
    }
}