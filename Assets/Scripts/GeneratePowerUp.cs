using UnityEngine;

public class GeneratePowerUp : MonoBehaviour
{
    public Transform groupBarrels;
    public GameObject barrelFuel;
    void Start()
    {
        int[] barrels = new int[3];
        int separate = 100;
        for (int i = 0; i < barrels.Length; i++)
        {
            barrels[i] = Random.Range(100 + separate * i, 100 + separate * (i + 1));

            GameObject BarrelFuel = Instantiate(barrelFuel, new Vector3(barrels[i], 55, 0), Quaternion.identity, groupBarrels);
        }
    }
}