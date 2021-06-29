using UnityEngine;
public class DataPersistant : MonoBehaviourSingleton<DataPersistant>
{
    public float playerFuel;
    public int playerLevel;
    private Ship player;
    
    void Start()
    {
        player = FindObjectOfType<Ship>();
        player.PlayerExplode += DestroyDataPersistant;
        player.PlayerLandSuccesful += SaveData;
        playerFuel = player.fuel;
        playerLevel = player.level;
    }
    void SaveData()
    {
        playerFuel = player.fuel;
        playerLevel = player.level;
    }
    public void DestroyDataPersistant()
    {
        if (gameObject)
            Destroy(gameObject);
    }
}