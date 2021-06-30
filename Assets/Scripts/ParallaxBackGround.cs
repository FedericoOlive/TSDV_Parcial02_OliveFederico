using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Vector3 = UnityEngine.Vector3;

public class ParallaxBackGround : MonoBehaviour
{
    private float initialPosPlayer;
    private float initialPos;
    private Transform player;
    public float speed;
    void Start()
    {
        player = FindObjectOfType<Ship>().transform;
        initialPosPlayer = player.position.x;
        initialPos = transform.position.x;
    }

    void Update()
    {
        float offset = (initialPosPlayer - player.position.x) / speed;
        Vector3 pos = transform.position;
        transform.position = new Vector3(initialPos - offset, pos.y, pos.z);
    }
}