using System;
using UnityEngine;
public class CameraSettings : MonoBehaviour
{
    public GameObject player;
    public TerrainGenerate terrain;
    private float minSize = 10;
    private float maxSize = 20;
    public float offsetDown = 10;
    public float z;
    public Ship ship;
    private float minAltitude = 20;

    private void Awake()
    {
        ship = player.GetComponent<Ship>();
    }

    void Start()
    {
        z = transform.position.z;
    }

    void Update()
    {
        UpdatePos();
    }

    void UpdatePos()
    {
        Vector2 playerPos = player.transform.position;

        if (playerPos.x > terrain.limitTerrain.minX && playerPos.x < terrain.limitTerrain.maxX)  // Update X cam
        {
            transform.position = new Vector3(playerPos.x, transform.position.y, z);
        }

        if (playerPos.y > terrain.limitTerrain.minY && playerPos.y < terrain.limitTerrain.maxY)  // Update Y cam
        {
            transform.position = new Vector3(transform.position.x, playerPos.y - offsetDown, z);
        }

        //UpdateZoom();
    }

    public void UpdateZoom()
    {
        float zoom = 0;
        float altitude = ship.AltitudeShip(false);
        if (altitude < minAltitude)
            zoom = minSize;

        GetComponent<Camera>().orthographicSize = zoom;
    }
}