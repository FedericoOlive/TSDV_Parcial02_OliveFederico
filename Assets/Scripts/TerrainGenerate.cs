using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerate : MonoBehaviour
{
    public GameObject cuberef;
    private EdgeCollider2D edgeColl;

    public struct Limits
    {
        public Limits(float xMin, float xMax, float yMin, float yMax)
        {
            minX = xMin;
            maxX = xMax;
            minY = yMin;
            maxY = yMax;
        }

        public float minX;
        public float maxX;
        public float minY;
        public float maxY;
    }

    public Limits limitTerrain = new Limits(40, 450, 25, 70);
    public const int Length = 500;
    public int minHeight = 20;
    public int maxHeight = 50;
    public int scanRadius = 2;
    public int smoothCount = 3;

    Vector2[] mapPoints = new Vector2[Length];
    private int offset;

    public MeshFilter meshFilter;
    public Mesh mesh;

    void Awake()
    {
        edgeColl = GetComponent<EdgeCollider2D>();
        mesh = new Mesh {name = "Terrain mesh"};
        meshFilter.mesh = mesh;

        GenerateHeightmap();
        BuildMesh();
    }
    void AddBasesLanding()
    {
        int radiusPick = 3;
        int[] basesX = new int[10];
        int separate = 40;
        for (int i = 0; i < basesX.Length; i++)
        {
            basesX[i] = Random.Range(50 + separate * i, 50 + separate * (i + 1));
            for (int j = -radiusPick; j <= radiusPick; j++)
            {
                mapPoints[basesX[i] + j].y = mapPoints[basesX[i]].y;
            }
            GameObject baseLand = Instantiate(cuberef, new Vector3(basesX[i], mapPoints[basesX[i]].y, 0), Quaternion.identity);
            baseLand.GetComponent<BaseLanding>().indexDistance = i;
        }
    }
    void GenerateHeightmap()
    {
        //Random.InitState(0);
        for (int j = 0; j < offset + Length; j++)
        {
            float rnd = Random.Range(minHeight, maxHeight);
            if (j >= offset)
            {
                int i = j - offset;
                mapPoints[i].y = rnd;
            }
        }

        for (int s = 0; s < smoothCount; s++)
            Smooth();

        AddBasesLanding();

        GenerateEdgeCollider();
    }
    void Smooth()
    {
        for (int i = 0; i < mapPoints.Length; i++)
        {
            float height = mapPoints[i].y;

            float heightSum = 0;
            float heightCount = 0;

            for (int n = i - scanRadius; n < i + scanRadius + 1; n++)
            {
                if (n >= 0 && n < mapPoints.Length)
                {
                    float heightOfNeighbour = mapPoints[n].y;

                    heightSum += heightOfNeighbour;
                    heightCount++;
                }
            }

            float heightAverage = heightSum / heightCount;
            mapPoints[i].y = heightAverage;
            mapPoints[i].x = i;
        }
    }
    void BuildMesh()
    {
        mesh.Clear();
        List<Vector3> positions = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = 0; i < Length - 1; i++)
        {
            int newOffset = i * 4;

            float h = mapPoints[i].y;
            float hn = mapPoints[i + 1].y;
            positions.Add(new Vector3(i + 0, 0, 0));  //lower left - at index 0
            positions.Add(new Vector3(i + 1, 0, 0));  //lower right - at index 1
            positions.Add(new Vector3(i + 0, h, 0));  //upper left - at index 2
            positions.Add(new Vector3(i + 1, hn, 0)); //upper right - at index 3

            triangles.Add(newOffset + 0);
            triangles.Add(newOffset + 2);
            triangles.Add(newOffset + 1);

            triangles.Add(newOffset + 1);
            triangles.Add(newOffset + 2);
            triangles.Add(newOffset + 3);
        }

        mesh.vertices = positions.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
    void GenerateEdgeCollider()
    {
        GetComponent<EdgeCollider2D>().points = mapPoints;
    }
}